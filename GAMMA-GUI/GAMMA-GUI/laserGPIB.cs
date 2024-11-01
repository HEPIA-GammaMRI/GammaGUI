using System.IO.Ports;
using System.IO;
using System.Threading;
using System;
using System.Text;
using System.Windows.Forms;
using ScottPlot.Drawing.Colormaps;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


/*GPIB-USB Settings
++eos 2
++auto 1
++eoi 1
++eot_enable 0
.NewLine = "\n";
*/
namespace GAMMA_GUI
{
    public enum UnitCOMMSErrors
    {
        Found,
        FoundButOFF,
        NoCOMPort,
        COMPortOnly,
        NotPresent,
        Unknown,
    }

    partial class laserGPIB
    {
        string sendString;
        string commandString;

        public int rampEnd;
        public int rampCurrentValue;
        public bool ramp;

        public bool ReadbacksAvailable;

        public string replyFromGPIB;

        public String RB_laserMode;
        public String RB_laserOut ;
        public String RB_laserThermistorResistance;
        public String RB_laserCurrentLimit;
        public String RB_ERROR;
        public String RB_laserCondition;
        public String RB_laserCurrentPhotoDiode ;
        public String RB_laserCurrentSetpoint;
        public String RB_laserTemperature;
        public String RB_laserPower;

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

        public laserGPIB()
        {
            string sUserProfilePATH = Environment.GetEnvironmentVariable("USERPROFILE");
            m_sLogDirectory = sUserProfilePATH + "\\My Documents\\GAMMA_GUI\\LaserGPIB" + LOG_FOLDER + "\\";
            m_sLogFilePath = m_sLogDirectory + "LaserGPIB-IO.txt";
            LogIOToFile = false;
            LogFrequencyMS = 10000; // log every 10s
            m_dtLastDataDump = DateTime.Now;
            DumpPropertiesToFile = false;
        }

        public void Init()
        {
            ReadbacksAvailable = false;

            commandString = "";
            ramp = false;

            RB_laserMode = "";
            RB_laserOut = "";
            RB_laserThermistorResistance = "";
            RB_laserCurrentLimit = "";
            RB_ERROR = "";
            RB_laserCondition = "";
            RB_laserCurrentPhotoDiode = "";
            RB_laserCurrentSetpoint = "";
            RB_laserTemperature = "";
            RB_laserPower = "";
        
            m_thIOThread = new Thread(MainIOThread);
            m_thIOThread.Name = "LaserGPIB_IOThread";

            UnitCOMMSErrors LaserGPIBStatus = UnitCOMMSErrors.Unknown;
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

                           
                            m_ioSPIO.DataBits = 8;
                            m_ioSPIO.Parity = Parity.None;
                            m_ioSPIO.StopBits = StopBits.One;

                            // RTS/CTS handshaking
                            m_ioSPIO.Handshake = Handshake.RequestToSend;
                            m_ioSPIO.DtrEnable = true;

                            // Error handling
                            m_ioSPIO.DiscardNull = false;
                            m_ioSPIO.ParityReplace = 0;

                           

                            m_ioSPIO.NewLine = "\n";


                            m_ioSPIO.Open();

                            LaserGPIBStatus = LaserGPIBUnitPresent();
                            if (LaserGPIBStatus == UnitCOMMSErrors.Found)
                            {
                                LogError("LaserGPIB IO - Initialised()");
                                m_bOnline = (LaserGPIBStatus == UnitCOMMSErrors.Found);
                                Connected = m_bOnline;
                                StartIOThread();
                                break;
                            }
                            else
                            {
                                m_ioSPIO.Close();
                                LogError("LaserGPIB IO - Failed to Init()");
                                m_bOnline = false;
                                LaserGPIBStatus = UnitCOMMSErrors.NotPresent;
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
               
                
            }
            //return LaserGPIBStatus;
        }

        private UnitCOMMSErrors LaserGPIBUnitPresent()
        {
            UnitCOMMSErrors LaserGPIBStatus = UnitCOMMSErrors.FoundButOFF;
            m_bOnline = true; // COMMS are possible via SerialPort 
            string rv = SendToUnit("++ver");
            if (rv != null)
            {
                
               if (rv.Contains("GPIB-USB Controller version 6.107"))  // >LaserGPIB Plasma Control Unit-1 
                {
                    LogError("LaserGPIB Unit Found!");
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

            
            try
            {

                m_ioSPIO.WriteTimeout = 1000;
                m_ioSPIO.ReadTimeout = 1000;

                m_ioSPIO.WriteLine(sCmd); 

                //Thread.Sleep(500);

                // Read from port until TIMEOUT time has elapsed since
                // last successful read
             
                    rv = m_ioSPIO.ReadLine();
                Thread.Sleep(50);

                //m_ioSPIO.WriteTimeout = 1;
                //m_ioSPIO.ReadTimeout = 1;

                m_ioSPIO.WriteLine("++rst");
                


                Thread.Sleep(50);
                String crap = m_ioSPIO.ReadLine();




            }
            catch (Exception ex)
            {
                //Console.WriteLine("SendToUnit(): IO Status = {0}");
               //m_ioSPIO.Close();
                //m_ioSPIO.Open();
             
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
                
     

                lock (m_oIOLock)

                {
                    LaserRamp();

                    sendString = "LAS:MODE?; LAS:OUT?; R?; LAS:LIM:I?; ERR?; LAS:COND?; LAS:IPD?; LAS:LDI?; T?; LAS:P?;LAS:F?;LAS:DC?;" + commandString;
                

                    commandString = "";
                }

                replyFromGPIB = SendToUnit(sendString); 
               
                String[] readbacks = replyFromGPIB.Split(';');

                Console.WriteLine(replyFromGPIB);

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
            
             
        }//IOThread()

        public void QueueCommand(string sCommand)
        {
            lock (m_oIOLock)
            {
                commandString = commandString + "; " + sCommand;
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

        private void LaserRamp()
        { 
            if(ramp==true)
            {
                if (rampEnd == int.Parse(RB_laserCurrentSetpoint.Split('.')[0]))
                {
                    ramp = false;
                }
                else
                {
                    if (rampCurrentValue == int.Parse(RB_laserCurrentSetpoint.Split('.')[0])) //Ramp Up
                    {
                        if (rampEnd > int.Parse(RB_laserCurrentSetpoint.Split('.')[0]))
                        {
                            rampCurrentValue++;
                        }
                        else
                        {
                            rampCurrentValue--;
                        }
                    }
                    
                    QueueCommand("LAS:LDI " + rampCurrentValue);
                    //Console.WriteLine("LAS:LDI " + rampCurrentValue);
                }
                
            }
                
        }
    }
}