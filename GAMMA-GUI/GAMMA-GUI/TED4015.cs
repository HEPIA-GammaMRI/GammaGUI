using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Threading;

// drivers from  here

//https://www.thorlabs.com/software_pages/viewsoftwarepage.cfm?code=4000_Series


namespace GAMMA_GUI
{

    partial class TED4015
    {
        string sendString;
        string commandString;

        public bool ReadbacksAvailable;

        public string replyFromted4015;

        public String ted4015BeeperOn;
        public String ted4015OutputOn;
        public String ted4015PeltierFansValue;
        public String ted4015PeltierSpareValue;
        public String RB_ERROR;
        public String ted4015PeltierTemperature;


        private SerialPort m_ioSPIO;
        private bool m_bOnline = false;
        private bool m_bConnected;


        private Thread m_thIOThread;

        public static Object m_oIOLock = new Object();


        private string m_sLogDirectory;
        private string m_sLogFilePath;
        private const string LOG_FOLDER = "\\Log";
        private bool m_bLogToDebugFile;
        private bool m_bDataDumpToFile;
        private int m_iDataDumpFrequencyMS;
        private DateTime m_dtLastDataDump;
        private bool m_ContinueMonitoring;

        public TED4015()
        {
            string sUserProfilePATH = Environment.GetEnvironmentVariable("USERPROFILE");
            m_sLogDirectory = sUserProfilePATH + "\\My Documents\\GAMMA_GUI\\ted4015" + LOG_FOLDER + "\\";
            m_sLogFilePath = m_sLogDirectory + "ted4015-IO.txt";
            LogIOToFile = false;
            LogFrequencyMS = 10000; // log every 10s
            m_dtLastDataDump = DateTime.Now;
            DumpPropertiesToFile = false;
        }

        public void Init()
        {
            ReadbacksAvailable = false;

            commandString = "";



            ted4015BeeperOn = "";
            ted4015OutputOn = "";
            RB_ERROR = "";
         

            m_thIOThread = new Thread(MainIOThread);
            m_thIOThread.Name = "ted4015_IOThread";

            UnitCOMMSErrors ted4015Status = UnitCOMMSErrors.Unknown;
            m_bOnline = false;
            m_bConnected = false;

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
                            m_ioSPIO.NewLine = "\r\n";
                            //m_ioSPIO.DtrEnable = true;
                            //m_ioSPIO.RtsEnable = true;
                            //m_ioSPIO.Handshake = Handshake.RequestToSend;
                            m_ioSPIO.WriteTimeout = 500;
                            m_ioSPIO.ReadTimeout = 500;
                            m_ioSPIO.ReadBufferSize = 255;
                            m_ioSPIO.WriteBufferSize = 255;
                            m_ioSPIO.Open();


                            ted4015Status = ted4015UnitPresent();
                            if (ted4015Status == UnitCOMMSErrors.Found)
                            {
                                LogError("ted4015 IO - Initialised()");
                                m_bOnline = (ted4015Status == UnitCOMMSErrors.Found);
                                Connected = m_bOnline;
                                break;
                            }
                            else
                            {
                                m_ioSPIO.Close();
                                LogError("ted4015 IO - Failed to Init()");
                                m_bOnline = false;
                                ted4015Status = UnitCOMMSErrors.NotPresent;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
            if (Connected)
            {
                StartIOThread();
            }
            //return ted4015Status;
        }

        private UnitCOMMSErrors ted4015UnitPresent()
        {
            UnitCOMMSErrors LaserGPIBStatus = UnitCOMMSErrors.FoundButOFF;
            m_bOnline = true; // COMMS are possible via SerialPort 
            string rv = SendToUnit("*IDN?");
            if (rv != null)
            {
                if (rv.Contains("THORLABS,"))
                {
                    LogError("TED4015 Unit Found!");
                    LaserGPIBStatus = UnitCOMMSErrors.Found;
                }
                else
                {
                    LogError(">>>>> It's all gone a bit Pete Tong!");
                }
            }
            return LaserGPIBStatus;
        }

        private string SendToUnit(string sCmd)
        {
            string rv;

            rv = "";


            //m_ioSPIO.WriteTimeout = 50;
            //m_ioSPIO.ReadTimeout = 50;

            try
            {


                m_ioSPIO.WriteLine(sCmd);

                Thread.Sleep(200);
                rv = m_ioSPIO.ReadLine();
                // m_ioSPIO.DiscardInBuffer();
                // m_ioSPIO.DiscardOutBuffer();





                if (sCmd.Contains("*IDN?"))
                {

                    //m_ioSPIO.WriteLine("++read");
                    Thread.Sleep(200);
                    rv = m_ioSPIO.ReadLine();
                    m_ioSPIO.DiscardOutBuffer();
                    m_ioSPIO.DiscardInBuffer();
                }
                else
                {
                    Thread.Sleep(200);
                    m_ioSPIO.DiscardInBuffer();

                }

                Connected = true;
                Console.WriteLine("Reply: " + rv);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendToUnit(): IO Status = {0}", ex.Message);
                //m_ioSPIO.Close();
                //m_ioSPIO.Open();
                //m_ioSPIO.DiscardOutBuffer();
                //m_ioSPIO.DiscardInBuffer();
                //ReportEvent( MonitoringEvents.COMMSError_Write, true);
                //Connected = false;
                //m_bOnline = false;
            }
            SaveToDebugFile(sCmd);
            //  Console.WriteLine("SendToUnit(): {0}", sCmd);

            return rv;
        }

        public void StartIOThread()
        {
            if (m_thIOThread.IsAlive == false)
            {
                Console.WriteLine("RC_IO: Starting IOThread");
                m_thIOThread.Start();
            }
        }

        public void Closedown()
        {
            m_ContinueMonitoring = false; // Signal to IOThread() that it should terminate
            if (m_thIOThread.IsAlive)
            {
                m_thIOThread.Join(2000);
            }
            m_bOnline = false;
            m_ioSPIO.Close();
            m_bConnected = false;
        }//Closedown()

        private void LogError(string sError)

        {
            Console.WriteLine("RC_IO - " + sError);
            SaveToDebugFile(sError);
        }

        public bool Connected
        {
            set
            {
                if (m_bConnected == value)
                {
                    return;
                }
                m_bConnected = value;
            }
            get { return m_bConnected; }
        }

        private void MainIOThread()
        {
            while (m_bOnline)
            {
                //m_ioSPIO.DiscardOutBuffer();
                //m_ioSPIO.DiscardInBuffer();
                //m_ioSPIO.WriteTimeout = 500;
                //m_ioSPIO.ReadTimeout = 500;

                lock (m_oIOLock)

                {


                    sendString = "SYSTem:BEEPer:STATe?;:OUT?" + commandString;

                    commandString = "";
                }
                replyFromted4015 = SendToUnit(sendString); //0;9167.3;24.0;302,123;0;5.0;20.0;27.0;1.09
                //Console.WriteLine(replyFromted4015);
                String[] readbacks = replyFromted4015.Split(new string[] { ";:" }, StringSplitOptions.None);

                if (readbacks.Length == 2)
                {

                    ted4015BeeperOn = readbacks[0].Split(new char[0])[1];
                    ted4015OutputOn = readbacks[1].Split(new char[0])[1];
                    

                    ReadbacksAvailable = true;

                }





            }


        }//IOThread()

        public void QueueCommand(string sCommand)
        {
            lock (m_oIOLock)
            {
                commandString = commandString + ";" + sCommand;
            }

        }//SendDGCommand()

        public bool LogIOToFile
        {
            set { m_bLogToDebugFile = value; }
            get { return m_bLogToDebugFile; }
        }

        private void SaveToDebugFile(string sText)
        {
            if (!m_bLogToDebugFile)
            {
                return;
            }
            try
            {
                StreamWriter file = new System.IO.StreamWriter(m_sLogDirectory + "RC-IO.txt", true); // Append if present, create if not
                file.WriteLine(string.Format("{0} : {1}", DateTime.Now, sText));
                file.Close();
            }
            catch { } // ## TODO - report fault on saving to file!
        }

        public bool DumpPropertiesToFile
        {
            set { m_bDataDumpToFile = value; }
            get { return m_bDataDumpToFile; }
        }

        public int LogFrequencyMS
        {
            set { m_iDataDumpFrequencyMS = value; }
            get { return m_iDataDumpFrequencyMS; }
        }

    }


}
