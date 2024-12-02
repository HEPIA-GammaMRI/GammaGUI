using System;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Text;

namespace GAMMA_GUI
{
    // Class for managing communication with the Arduino device
    partial class gammaARDUINO
    {
        // Communication Strings
        private StringBuilder commandQueue = new StringBuilder();    // Queue of commands to be sent to Arduino

        // Status and reply variables
        public bool ReadbacksAvailable;
        public string replyFromArduino;

        // Arduino-specific variables
        public string arduinoPumpEnabled;
        public string arduinoPeltierValue;
        public string arduinoPeltierFansValue;
        public string arduinoPeltierSpareValue;
        public string RB_ERROR;
        public string arduinoPeltierTemperature;
        public string arduinoRelayFansLaser;
        public string arduinoTC0;
        public string arduinoTC1;
        public string arduinoTC0SP;
        public string arduinoKu;
        public string arduinoINTLK;
        public string arduinoWARNINGLIGHT;

        // Serial port communication settings
        private SerialPort m_ioSPIO;
        private bool m_bOnline = false;
        private bool m_bConnected = false;

        private Thread m_thIOThread;
        private static readonly object m_oIOLock = new object();

        // Log settings
        private string m_sLogDirectory;
        private string m_sLogFilePath;
        private const string LOG_FOLDER = "\\Log";
        private bool m_bLogToDebugFile;
        private int m_iDataDumpFrequencyMS = 10000; // Log every 10 seconds
        private DateTime m_dtLastDataDump;
        private bool m_ContinueMonitoring;

        // Constructor: Initializes log directory and file paths
        public gammaARDUINO()
        {
            string sUserProfilePATH = Environment.GetEnvironmentVariable("USERPROFILE");
            m_sLogDirectory = sUserProfilePATH + "\\My Documents\\GAMMA_GUI\\gammaArduino" + LOG_FOLDER + "\\";
            m_sLogFilePath = m_sLogDirectory + "gammaArduino-IO.txt";
            m_bLogToDebugFile = false;
            m_dtLastDataDump = DateTime.Now;
        }

        // Initializes communication with the Arduino and starts the I/O thread
        public void Init()
        {
            ReadbacksAvailable = false;

            // Initialize Arduino communication variables
            commandQueue.Clear();
            arduinoPumpEnabled = "";
            arduinoPeltierValue = "";
            arduinoPeltierFansValue = "";
            arduinoPeltierSpareValue = "";
            RB_ERROR = "";
            arduinoPeltierTemperature = "";
            arduinoRelayFansLaser = "";
            arduinoINTLK = "0";
            arduinoWARNINGLIGHT = "";

            // Start I/O thread
            m_thIOThread = new Thread(MainIOThread)
            {
                Name = "gammaArduino_IOThread"
            };

            UnitCOMMSErrors gammaArduinoStatus = UnitCOMMSErrors.Unknown;
            m_bOnline = false;
            m_bConnected = false;

            // Check available serial ports
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                if (port.Contains("/dev/cu.usbmodem") || port.Contains("/dev/tty.usbmodem") || port.Contains("COM"))
                {
                    m_ioSPIO = new SerialPort(port, 115200);
                    if (!m_ioSPIO.IsOpen)
                    {
                        try
                        {
                            // Set serial port communication parameters
                            m_ioSPIO.NewLine = "\r\n";
                            m_ioSPIO.WriteTimeout = 500;
                            m_ioSPIO.ReadTimeout = 500;
                            m_ioSPIO.ReadBufferSize = 4096;
                            m_ioSPIO.WriteBufferSize = 4096;
                            m_ioSPIO.Open();

                            // Check if the Arduino is responding
                            gammaArduinoStatus = gammaArduinoUnitPresent();
                            if (gammaArduinoStatus == UnitCOMMSErrors.Found)
                            {
                                LogError("gammaArduino IO - Initialized()");
                                m_bOnline = true;
                                m_bConnected = true;
                                break;
                            }
                            else
                            {
                                m_ioSPIO.Close();
                                LogError("gammaArduino IO - Failed to Init()");
                                m_bOnline = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }

            if (m_bConnected)
            {
                StartIOThread();
            }
        }

        // Check if the Arduino is present and responding
        private UnitCOMMSErrors gammaArduinoUnitPresent()
        {
            UnitCOMMSErrors status = UnitCOMMSErrors.FoundButOFF;
            m_bOnline = true; // Assume communication is possible

            string rv = SendToUnit("FWRev?");
            if (rv != null && rv.Contains("arduinoGAMMA1.0"))
            {
                LogError("arduinoGamma Unit Found!");
                status = UnitCOMMSErrors.Found;
            }

            return status;
        }

        // Sends a command to the Arduino and receives the reply
        private string SendToUnit(string command)
        {
            string response = string.Empty;

            try
            {
                // Clear buffers before sending and receiving
                m_ioSPIO.DiscardInBuffer();
                m_ioSPIO.DiscardOutBuffer();

                // Send the command and read the response
                m_ioSPIO.WriteLine(command);
                response = m_ioSPIO.ReadLine();

                if (command.Contains("FWRev?"))
                {
                    Thread.Sleep(200);
                    response = m_ioSPIO.ReadLine(); // Read firmware version reply
                }

                m_bConnected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendToUnit() Error: {ex.Message}");
            }

            SaveToDebugFile(command);
            return response;
        }

        // Starts the I/O thread to handle communication with the Arduino
        public void StartIOThread()
        {
            if (!m_thIOThread.IsAlive)
            {
                m_thIOThread.Start();
            }
        }

        // Shuts down communication and stops the I/O thread
        public void Closedown()
        {
            m_ContinueMonitoring = false;
            if (m_thIOThread.IsAlive)
            {
                m_thIOThread.Join(2000); // Wait for thread termination
            }
            m_bOnline = false;
            m_ioSPIO.Close();
            m_bConnected = false;
        }

        // Logs an error and saves it to the debug file
        private void LogError(string errorMessage)
        {
            Console.WriteLine("RC_IO - " + errorMessage);
            SaveToDebugFile(errorMessage);
        }

        // Property to control connection status
        public bool Connected
        {
            set => m_bConnected = value;
            get => m_bConnected;
        }

        // Main I/O thread method for communication with the Arduino
        private void MainIOThread()
        {
            while (m_bOnline)
            {
                lock (m_oIOLock)
                {
                    string command = "Pu?;Pe?;PF?;LF?;T0?;S0?;Ku?;LK?;WL?" + commandQueue.ToString();
                    commandQueue.Clear(); // Clear the command queue after sending
                    replyFromArduino = SendToUnit(command);

                    // Parse the response
                    var readbacks = replyFromArduino.Split(';');
                    if (readbacks.Length == 10)
                    {
                        arduinoPumpEnabled = readbacks[0].Split(' ')[1];
                        arduinoPeltierValue = readbacks[1].Split(' ')[1];
                        arduinoPeltierFansValue = readbacks[2].Split(' ')[1];
                        arduinoRelayFansLaser = readbacks[3].Split(' ')[1];
                        arduinoTC0 = readbacks[4].Split(' ')[1];
                        arduinoTC0SP = readbacks[5].Split(' ')[1];
                        arduinoKu = readbacks[6].Split(' ')[1];
                        arduinoINTLK = readbacks[7].Split(' ')[1];
                        arduinoWARNINGLIGHT = readbacks[8].Split(' ')[1];
                        ReadbacksAvailable = true;
                    }
                }

                // Use a timed loop to reduce unnecessary CPU consumption
                Thread.Sleep(100); // This reduces the load compared to busy-waiting
            }
        }

        // Queues a command for sending to the Arduino
        public void QueueCommand(string command)
        {
            lock (m_oIOLock)
            {
                commandQueue.Append(";" + command);
            }
        }

        // Save a message to the debug file
        private void SaveToDebugFile(string text)
        {
            if (!m_bLogToDebugFile) return;

            try
            {
                using (StreamWriter file = new StreamWriter(m_sLogDirectory + "RC-IO.txt", true))
                {
                    file.WriteLine($"{DateTime.Now}: {text}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveToDebugFile() Error: {ex.Message}");
            }
        }

        // Property to control logging to debug file
        public bool LogIOToFile
        {
            set => m_bLogToDebugFile = value;
            get => m_bLogToDebugFile;
        }
    }
}
