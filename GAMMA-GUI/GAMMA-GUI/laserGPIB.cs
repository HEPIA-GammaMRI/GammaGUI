using System;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Text;

namespace GAMMA_GUI
{
    // Enum to represent different communication statuses
    public enum UnitCOMMSErrors
    {
        Found,
        FoundButOFF,
        NoCOMPort,
        COMPortOnly,
        NotPresent,
        Unknown
    }

    // Main class for managing Laser GPIB (General Purpose Interface Bus) communication
    partial class laserGPIB
    {
        // Communication strings and laser control variables
        private string sendString;
        private string commandString;
        public int rampEnd;
        public int rampCurrentValue;
        public bool ramp;
        public bool ReadbacksAvailable;
        public string replyFromGPIB;
        public string RB_laserMode, RB_laserOut, RB_laserThermistorResistance, RB_laserCurrentLimit;
        public string RB_ERROR, RB_laserCondition, RB_laserCurrentPhotoDiode, RB_laserCurrentSetpoint;
        public string RB_laserTemperature, RB_laserPower;

        // Serial communication and threading setup
        private SerialPort m_ioSPIO;
        private bool m_bOnline = false;
        private bool m_bConnected;
        private Thread m_thIOThread;
        private static readonly object m_oIOLock = new object();

        // Logging settings
        private string m_sLogDirectory;
        private string m_sLogFilePath;
        private const string LOG_FOLDER = "\\Log";
        private bool m_bLogToDebugFile;
        private int m_iDataDumpFrequencyMS = 10000;  // Log every 10 seconds
        private DateTime m_dtLastDataDump;
        private bool m_ContinueMonitoring;

        // Constructor to initialize logging and set the directory paths
        public laserGPIB()
        {
            string sUserProfilePATH = Environment.GetEnvironmentVariable("USERPROFILE");
            m_sLogDirectory = sUserProfilePATH + "\\My Documents\\GAMMA_GUI\\LaserGPIB" + LOG_FOLDER + "\\";
            m_sLogFilePath = m_sLogDirectory + "LaserGPIB-IO.txt";
            LogIOToFile = false;
            m_dtLastDataDump = DateTime.Now;
        }

        // Initializes serial port communication and checks for connected device
        public void Init()
        {
            // Reset variables
            ReadbacksAvailable = false;
            commandString = "";
            ramp = false;
            ResetReadbacks();

            m_thIOThread = new Thread(MainIOThread) { Name = "LaserGPIB_IOThread" };
            UnitCOMMSErrors LaserGPIBStatus = UnitCOMMSErrors.Unknown;
            m_bOnline = false;
            m_bConnected = false;

            // Try to find a connected device via available COM ports
            foreach (string port in SerialPort.GetPortNames())
            {
                if (port.Contains("/dev/cu.usbmodem") || port.Contains("/dev/tty.usbmodem") || port.Contains("COM"))
                {
                    m_ioSPIO = new SerialPort(port, 115200);
                    InitializeSerialPort();
                    LaserGPIBStatus = LaserGPIBUnitPresent();

                    if (LaserGPIBStatus == UnitCOMMSErrors.Found)
                    {
                        LogError("LaserGPIB IO - Initialised()");
                        m_bOnline = true;
                        Connected = true;
                        StartIOThread();
                        break;
                    }
                    else
                    {
                        m_ioSPIO.Close();
                        LogError("LaserGPIB IO - Failed to Init()");
                    }
                }
            }
        }

        // Initializes the serial port settings
        private void InitializeSerialPort()
        {
            if (!m_ioSPIO.IsOpen)
            {
                try
                {
                    m_ioSPIO.DataBits = 8;
                    m_ioSPIO.Parity = Parity.None;
                    m_ioSPIO.StopBits = StopBits.One;
                    m_ioSPIO.Handshake = Handshake.RequestToSend;
                    m_ioSPIO.DtrEnable = true;
                    m_ioSPIO.DiscardNull = false;
                    m_ioSPIO.ParityReplace = 0;
                    m_ioSPIO.NewLine = "\n";
                    m_ioSPIO.Open();
                }
                catch (Exception ex)
                {
                    LogError($"Error initializing serial port: {ex.Message}");
                }
            }
        }

        // Checks if the Laser GPIB unit is present and responding
        private UnitCOMMSErrors LaserGPIBUnitPresent()
        {
            string rv = SendToUnit("++ver");
            if (rv.Contains("GPIB-USB Controller version 6.107"))
            {
                LogError("LaserGPIB Unit Found!");
                return UnitCOMMSErrors.Found;
            }
            else
            {
                LogError("LaserGPIB Unit Not Found");
                return UnitCOMMSErrors.NotPresent;
            }
        }

        // Sends a command to the unit and receives a response
        private string SendToUnit(string sCmd)
        {
            string response = "";
            try
            {
                m_ioSPIO.WriteTimeout = 1000;
                m_ioSPIO.ReadTimeout = 1000;
                m_ioSPIO.WriteLine(sCmd);
                response = m_ioSPIO.ReadLine();
                m_ioSPIO.WriteLine("++rst");
            }
            catch (Exception ex)
            {
                LogError($"SendToUnit Error: {ex.Message}");
            }
            SaveToDebugFile(sCmd);
            return response;
        }

        // Starts the I/O thread to handle continuous communication
        public void StartIOThread()
        {
            if (!m_thIOThread.IsAlive)
            {
                m_thIOThread.Start();
            }
        }

        // Closes down the communication and stops the I/O thread
        public void Closedown()
        {
            m_ContinueMonitoring = false;  // Signal IOThread to terminate
            if (m_thIOThread.IsAlive)
            {
                m_thIOThread.Join(2000);  // Wait for thread to finish
            }
            m_bOnline = false;
            m_ioSPIO.Close();
            m_bConnected = false;
        }

        // Logs error messages to the console and debug file
        private void LogError(string sError)
        {
            Console.WriteLine("RC_IO - " + sError);
            SaveToDebugFile(sError);
        }

        // Property for connection status
        public bool Connected
        {
            set { m_bConnected = value; }
            get { return m_bConnected; }
        }

        // Main I/O thread that continuously queries the Laser GPIB unit
        private void MainIOThread()
        {
            while (m_bOnline)
            {
                lock (m_oIOLock)
                {
                    // Prepare and send commands to the unit
                    sendString = $"LAS:MODE?; LAS:OUT?; R?; LAS:LIM:I?; ERR?; LAS:COND?; LAS:IPD?; LAS:LDI?; T?; LAS:P?;LAS:F?;LAS:DC?;{commandString}";
                    commandString = "";  // Reset command string after sending
                }

                replyFromGPIB = SendToUnit(sendString);
                ParseReadbacks(replyFromGPIB);
            }
        }

        // Parses the reply from the Laser GPIB unit and updates the readbacks
        private void ParseReadbacks(string reply)
        {
            string[] readbacks = reply.Split(';');
            if (readbacks.Length == 12)
            {
                RB_laserMode = readbacks[0];
                RB_laserOut = readbacks[1];
                RB_laserThermistorResistance = readbacks[2];
                RB_laserCurrentLimit = readbacks[3];
                RB_ERROR = readbacks[4];
                RB_laserCondition = readbacks[5];
                RB_laserCurrentPhotoDiode = readbacks[6];
                RB_laserCurrentSetpoint = readbacks[7];
                RB_laserTemperature = readbacks[8];
                RB_laserPower = readbacks[9];
                ReadbacksAvailable = true;
            }
        }

        // Adds a new command to the queue
        public void QueueCommand(string sCommand)
        {
            lock (m_oIOLock)
            {
                commandString = $"{commandString}; {sCommand}";
            }
        }

        // Saves debug information to a file
        private void SaveToDebugFile(string sText)
        {
            if (!m_bLogToDebugFile) return;
            try
            {
                using (StreamWriter file = new StreamWriter(m_sLogDirectory + "RC-IO.txt", true))
                {
                    file.WriteLine($"{DateTime.Now}: {sText}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to debug file: {ex.Message}");
            }
        }

        // Property to control whether to log I/O to file
        public bool LogIOToFile
        {
            set { m_bLogToDebugFile = value; }
            get { return m_bLogToDebugFile; }
        }

        // Resets all readback variables
        private void ResetReadbacks()
        {
            RB_laserMode = RB_laserOut = RB_laserThermistorResistance = RB_laserCurrentLimit = "";
            RB_ERROR = RB_laserCondition = RB_laserCurrentPhotoDiode = RB_laserCurrentSetpoint = "";
            RB_laserTemperature = RB_laserPower = "";
        }
    }
}
