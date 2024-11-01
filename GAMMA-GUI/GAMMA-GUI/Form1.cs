using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Thorlabs.TL4000;
using System.Linq;
using ScottPlot.Renderable;
using ScottPlot;
using Tinkerforge;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace GAMMA_GUI
{
    public partial class Form1 : Form
    {
        laserGPIB ldx36000;
        gammaARDUINO arduinoGamma;
        TL4000 ted4015;

        private static string HOST = "localhost";
        private static int PORT = 4223;
        private static string UID = "RkH"; // Change XYZ to the UID of your Thermal Imaging Bricklet

        private static int WIDTH = 80;
        private static int HEIGHT = 60;
        private static int SCALE = 3;

        private byte[] imageData = new byte[80 * 60];

        // Creates standard thermal image color palette (blue=cold, red=hot)
        private byte[] paletteR = new byte[256];
        private byte[] paletteG = new byte[256];
        private byte[] paletteB = new byte[256];

        byte[] regionOfInterest = new byte[4];

        IPConnection ipcon;
        BrickletThermalImaging ti;

        bool INTLKENABLE;

        bool tedTECOut;
        double tedTemp,tedTempSetpoint,tedTECCurrentReading,tedTECVoltageReading;

        bool firstReadCurrentSetpoint;
        bool firstReadCurrentLimit;

        double[] arduinoT0Array = new double[] { };
        double[] arduinoS0Array = new double[] { };
        double[] arduinoPeArray = new double[] { };
        double[] dateTimeArray = new double[] { };

        double[] ted4015tempArray = new double[] { };

        double[] lastempArray = new double[] { };

        const int maxPlotPoints = 100;


        bool axisPeInited;
        int peAxisIndex;
        bool plotArduinoLive;

        int laserLoopCount;
        Stopwatch swLaserLoop = new Stopwatch();
        string laserLastCmd;

        public Form1()
        {

            InitializeComponent();

            this.FormClosing += Form1_FormClosing;

            firstReadCurrentSetpoint =true;
            firstReadCurrentLimit =true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            laserLoopCount = 0;
            INTLKENABLE = false;

            plotArduinoLive = true;
            axisPeInited = false;

            ldx36000 = new laserGPIB();
            ldx36000.Init();
            comboBoxLaserMode.SelectedIndex = 2;
            //ldx36000.QueueCommand("LAS:MODE:PULSE");

            arduinoGamma = new gammaARDUINO();
            arduinoGamma.Init();

            //arduinoGamma.QueueCommand("S0 30");

            ted4015 = new TL4000("USB::4883::32840::M00429417::INSTR", true, false);

            timerReadbacks.Interval = 1000;
            timerReadbacks.Enabled = true;

            pictureBoxLASER.Visible = false;

            timerUI.Interval = 10;
            timerUI.Enabled = true;

            checkboxRampEnable.Checked = false;
            checkboxRampEnable.Text = "Ramp";

          
                buttonLasFan.Text = "Turn Fan On";
            FLIR_Load();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string formattedDateTime = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            string formattedShortDateTime = DateTime.Now.ToString("HH:mm:ss");


            //if(arduinoGamma.arduinoINTLK != "1" && ldx36000.RB_laserOut == "1")
            //{
            //     ldx36000.QueueCommand("LAS:OUT 0");
            //     MessageBox.Show("Laser Hood opened, LASER turned OFF!");
            //     arduinoGamma.QueueCommand("WL 0");
            //}

            String filepath = "C:/Users/Gamma_MRI/Desktop/GammaAshData/" + DateTime.Now.ToString("dd-MMMM-yyyy-HH-mm") +".csv";


            using (Stream stream2 = File.Open(filepath, FileMode.Append))
            using (StreamWriter sWriter1 = new StreamWriter(stream2))
            {

                sWriter1.WriteLine(formattedDateTime + " - TempRead : " + tedTemp.ToString("+0.##;-0.##;(0)") + " C");
       
               
            }
            try
            {
                ted4015.measTemp(out tedTemp);
                labelTedTemp.Text = "T reading: " + tedTemp.ToString("+0.##;-0.##;(0)") + " C";

                ted4015.getTempSetpoint(0, out tedTempSetpoint);
                labelTedTempSetpoint.Text = "T setpoint: " + tedTempSetpoint.ToString("+0.##;-0.##;(0)") + " C";

                ted4015.getTecOutputState(out tedTECOut);

                ted4015.measTecCurr(out tedTECCurrentReading);
                labelTedTECCurrent.Text = "TEC Current: " + tedTECCurrentReading.ToString("+0.##;-0.##;(0)") + " A";

                ted4015.measTecVolt(out tedTECVoltageReading);
                labelTedTECVoltage.Text = "TEC Voltage: " + tedTECVoltageReading.ToString("+0.##;-0.##;(0)") + " V";


                if (tedTECOut)
                {
                    buttonTECOut.BackColor = Color.Green;
                    buttonTECOut.Text = "TEC ON";

                }
                else
                {


                    buttonTECOut.BackColor = Color.LightGray;
                    buttonTECOut.Text = "TEC OFF";
                }






                if (dateTimeArray.Length > 100)
                {
                    for(var i = 0;i<dateTimeArray.Length-1;i++) {
                        dateTimeArray[i] = dateTimeArray[i+1];
                    }
                }
                else
                {
                    Array.Resize<double>(ref dateTimeArray, dateTimeArray.Length + 1);
                }

                double nowTime = DateTime.Now.ToOADate();
                
                dateTimeArray[dateTimeArray.Length - 1] = nowTime;






                if (ted4015tempArray.Length > 100)
                {
                    for (var i = 0; i < ted4015tempArray.Length - 1; i++)
                    {
                        ted4015tempArray[i] = ted4015tempArray[i + 1];
                    }
                }
                else
                {
                    Array.Resize<double>(ref ted4015tempArray, ted4015tempArray.Length + 1);
                }


               
                ted4015tempArray[ted4015tempArray.Length - 1] = tedTemp;



                //formsPlotTED4015.Plot.AddPoint(nowTime, tedTemp,Color.Red,3,ScottPlot.MarkerShape.filledCircle,"Temp");

                formsPlotTED4015.Plot.Clear();

                var plotT0 = formsPlotTED4015.Plot.AddScatterLines(dateTimeArray, ted4015tempArray, Color.Red, 2, ScottPlot.LineStyle.Solid, "T0");
                plotT0.YAxisIndex = formsPlotTED4015.Plot.LeftAxis.AxisIndex;
                formsPlotTED4015.Plot.YAxis.Label("Temperature");

                formsPlotTED4015.Plot.XAxis.DateTimeFormat(true);
                formsPlotTED4015.Plot.AxisAuto(0, 0.2); 
                formsPlotTED4015.Refresh(false,true);

                

                if (arduinoT0Array.Length > maxPlotPoints)
                {
                    for (var i = 0; i < arduinoT0Array.Length - 1; i++)
                    {
                        arduinoT0Array[i] = arduinoT0Array[i + 1];
                    }
                }
                else
                {
                    Array.Resize<double>(ref arduinoT0Array, arduinoT0Array.Length + 1);
                }
                arduinoT0Array[arduinoT0Array.Length - 1] = double.Parse(arduinoGamma.arduinoTC0);

                if (arduinoS0Array.Length > maxPlotPoints)
                {
                    for (var i = 0; i < arduinoS0Array.Length - 1; i++)
                    {
                        arduinoS0Array[i] = arduinoS0Array[i + 1];
                    }
                }
                else
                {
                    Array.Resize<double>(ref arduinoS0Array, arduinoS0Array.Length + 1);
                }
                arduinoS0Array[arduinoS0Array.Length - 1] = double.Parse(arduinoGamma.arduinoTC0SP);

                if (arduinoPeArray.Length > maxPlotPoints)
                {
                    for (var i = 0; i < arduinoPeArray.Length - 1; i++)
                    {
                        arduinoPeArray[i] = arduinoPeArray[i + 1];
                    }
                }
                else
                {
                    Array.Resize<double>(ref arduinoPeArray, arduinoPeArray.Length + 1);
                }

                arduinoPeArray[arduinoPeArray.Length - 1] = double.Parse(arduinoGamma.arduinoPeltierValue);
                
                formsPlotArduino.Plot.Clear();

                var plotTedTemp = formsPlotArduino.Plot.AddScatterLines(dateTimeArray, arduinoT0Array,Color.Red,2,ScottPlot.LineStyle.Solid,"T0");
                plotT0.YAxisIndex = formsPlotArduino.Plot.LeftAxis.AxisIndex;
                formsPlotArduino.Plot.YAxis.Label("Temperature");

                var plotS0 = formsPlotArduino.Plot.AddScatterLines(dateTimeArray, arduinoS0Array, Color.Blue, 2, ScottPlot.LineStyle.Solid, "S0");
                plotS0.YAxisIndex = formsPlotArduino.Plot.LeftAxis.AxisIndex;

                

                var plotPe = formsPlotArduino.Plot.AddScatterLines(dateTimeArray, arduinoPeArray, Color.Orange, 1, ScottPlot.LineStyle.Solid, "Pelt0");
                if (!axisPeInited)
                {
                    var PeAxis = formsPlotArduino.Plot.AddAxis(Edge.Right);
                    plotPe.YAxisIndex = PeAxis.AxisIndex;
                    peAxisIndex = PeAxis.AxisIndex;
                    PeAxis.Label("Peltier %");
                    axisPeInited = true;
                }
                else
                {
                    plotPe.YAxisIndex = peAxisIndex;
                }
                formsPlotArduino.Plot.XAxis.DateTimeFormat(true);
                formsPlotArduino.Plot.AxisAuto(0, 0.2);

                if (plotArduinoLive) { formsPlotArduino.Refresh(false, true); }

                if (lastempArray.Length > maxPlotPoints)
                {
                    for (var i = 0; i < lastempArray.Length - 1; i++)
                    {
                        lastempArray[i] = lastempArray[i + 1];
                    }
                }
                else
                {
                    Array.Resize<double>(ref lastempArray, lastempArray.Length + 1);
                }

                if (ldx36000.RB_laserTemperature != "")
                {
                 lastempArray[lastempArray.Length - 1] = double.Parse(ldx36000.RB_laserTemperature);
                }
               

                formsPlotLasTemp.Plot.Clear();

                var plotLasTemp = formsPlotLasTemp.Plot.AddScatterLines(dateTimeArray, lastempArray, Color.Red, 2, ScottPlot.LineStyle.Solid, "Temp");
                plotLasTemp.YAxisIndex = formsPlotLasTemp.Plot.LeftAxis.AxisIndex;
                formsPlotLasTemp.Plot.YAxis.Label("Temperature");

                formsPlotLasTemp.Plot.XAxis.DateTimeFormat(true);
                formsPlotLasTemp.Plot.AxisAuto(0, 0.2);

             formsPlotLasTemp.Refresh(false, true); 


                


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                
            }




            if (ldx36000.ReadbacksAvailable)
            {
                //TEMPERATURE WARNING!!!
                if (int.Parse(ldx36000.RB_laserTemperature.Split('.')[0]) > 50 && int.Parse(ldx36000.RB_laserOut.Split('.')[0]) == 1)
                {
                    ldx36000.QueueCommand("LAS:OUT 0");
                    MessageBox.Show("LASER WAS TURNED OFF AS TEMPERATURE EXCEEDED 50C. \n PLEASE CHECK TEMPERATURE CONTROLLER AND FANS!");
                }
                if (int.Parse(ldx36000.RB_laserTemperature.Split('.')[0]) > 50 && int.Parse(ldx36000.RB_laserOut.Split('.')[0]) == 0)
                {
                    ldx36000.QueueCommand("LAS:OUT 0");
                    MessageBox.Show("LASER TEMPERATURE HAS EXCEEDED 50C. \n PLEASE CHECK TEMPERATURE CONTROLLER AND FANS!");
                }
            }
            



            //---------------------------
            

            labelReplyFromGPIB.Text = formattedShortDateTime + " " + ldx36000.replyFromGPIB;
           

            if (ldx36000.RB_laserOut == "1")
            {
                panelLaser.BackColor = Color.PaleVioletRed;
                labelLaserOut.Text = "Laser Output ON";
                pictureBoxLASER.Visible = true;
            }
            else
            {
               
                panelLaser.BackColor = SystemColors.Control; ;
                labelLaserOut.Text = "Laser Output OFF";
                pictureBoxLASER.Visible = false;
            }
            labelLaserMode.Text = "Laser Mode: " + ldx36000.RB_laserMode;
            labelLasFanOn.Text = "Laser Fans: " + arduinoGamma.arduinoRelayFansLaser ;

     

            //.Text.Insert(0, "SORT BY NAME");
            //labelLaserERR.Text = "ERRORS: ";

            if (ldx36000.RB_ERROR != "0")
            {
                if (ldx36000.RB_ERROR != "")
                {
                    //richTextBoxERRORS.Text += ldx36000.RB_laserERR + "\n";
                    if (ldx36000.RB_ERROR.Contains("123"))
                    {
                        ldx36000.RB_ERROR = ldx36000.RB_ERROR.Replace("123", "");
                    }
                    if(ldx36000.RB_ERROR != "")
                    {
                        richTextBoxERRORS.Text = richTextBoxERRORS.Text.Insert(0, ldx36000.RB_ERROR + " \n");
                    }
                    
                }
            }
            

            if (ldx36000.RB_ERROR.Contains("599"))
            {
                richTextBoxERRORS.Text = richTextBoxERRORS.Text.Insert(0, formattedDateTime + " : Laser open circuit 2 \n");
            }
            if (ldx36000.RB_ERROR.Contains("501"))
            {
                richTextBoxERRORS.Text = richTextBoxERRORS.Text.Insert(0, formattedDateTime + " : Laser interlock #1 disabled output. \n");
            }
            if (ldx36000.RB_ERROR.Contains("502"))
            {
                richTextBoxERRORS.Text = richTextBoxERRORS.Text.Insert(0, formattedDateTime + " : Laser interlock #2 disabled output. \n");
            }
            if (ldx36000.RB_ERROR.Contains("301"))
            {
                richTextBoxERRORS.Text = richTextBoxERRORS.Text.Insert(0, formattedDateTime + " :  A <RESPONSE MESSAGE> was ready, but controller failed to read it. (query error) \n");
            }
            if (ldx36000.RB_ERROR.Contains("302"))
            {
                richTextBoxERRORS.Text = richTextBoxERRORS.Text.Insert(0, formattedDateTime + " : Query error. Device was addressed to talk, but controller failed to read all of the <RESPONSE MESSAGE>. \n");
            }


            //richTextBoxERRORS.SelectionStart = richTextBoxERRORS.Text.Length;
            //richTextBoxERRORS.ScrollToCaret();
            
            labelLaserCurrentLimit.Text = "I limit: " + ldx36000.RB_laserCurrentLimit + " A";
            labelLaserCurrentPhotoDiode.Text = "IPhotoDiode :" + ldx36000.RB_laserCurrentPhotoDiode + " A";
            labelLaserCurrentSetpoint.Text = "I Setpoint :" + ldx36000.RB_laserCurrentSetpoint + " A";
            labelLaserTemperature.Text = "Temperature :" + ldx36000.RB_laserTemperature + " C";
            labelLaserPower.Text = "Power :" + ldx36000.RB_laserPower + " W";


            /*
            if (ldx36000.RB_laserTemperature != "")
            {
                chartLaserLive.Series["Temperature"].Points.AddXY(formattedShortDateTime, double.Parse(ldx36000.RB_laserTemperature));
            }
            
            if(ldx36000.RB_laserPower != "")
            {
                chartLaserLive.Series["Power"].Points.AddXY(formattedShortDateTime,double.Parse(ldx36000.RB_laserPower));
            }

            if(chartLaserLive.Series["Power"].Points.Count > 120)
            {
                chartLaserLive.Series["Power"].Points.RemoveAt(0);
                chartLaserLive.Series["Temperature"].Points.RemoveAt(0);
                chartLaserLive.ResetAutoValues();
            }
            */

            if (ldx36000.RB_laserCurrentLimit != "" && !ldx36000.RB_laserCurrentLimit.Contains(","))
            {
                //trackBarLaserCurrentLimit.Value = 1;
                if (int.Parse(ldx36000.RB_laserCurrentLimit.Split('.')[0]) == trackBarLaserCurrentLimit.Value)
                {
                    buttonCurrentLimit.BackColor = Color.LightGreen;
                }
                else
                {
                    if (firstReadCurrentLimit == true)
                    {
                        trackBarLaserCurrentLimit.Value = int.Parse(ldx36000.RB_laserCurrentLimit.Split('.')[0]);
                        firstReadCurrentLimit = false;
                    }
                    buttonCurrentLimit.BackColor = Color.PaleVioletRed;
                }

                
            }

            if (ldx36000.RB_laserCurrentSetpoint != "")
            {
                if (int.Parse(ldx36000.RB_laserCurrentSetpoint.Split('.')[0]) == trackBarLaserCurrentSetpoint.Value)
                {
                    buttonLaserCurrentSetpoint.BackColor = Color.LightGreen;
                }
                else
                {
                    if(firstReadCurrentSetpoint == true)
                    {
                        trackBarLaserCurrentSetpoint.Value = int.Parse(ldx36000.RB_laserCurrentSetpoint.Split('.')[0]);
                        firstReadCurrentSetpoint = false;
                    }
                    buttonLaserCurrentSetpoint.BackColor = Color.PaleVioletRed;
                }

                
                
            }
            //Console.WriteLine("laserMode:"+ldx36000.RB_laserCondition);

            if (ldx36000.Connected)
            {
                buttonCONN.Text = "Connected";
                buttonCONN.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                buttonCONN.Text = "Disconnected";
                buttonCONN.BackColor = System.Drawing.Color.Red;
            }
            

            if (ldx36000.RB_laserOut == "0")
            {
                buttonLaserOut.Text = "Turn On";
            }
            else
            {
                buttonLaserOut.Text = "Turn Off";
            }

            if(arduinoGamma.arduinoPumpEnabled == "0")
            {
                buttonPeltFan.Text = "Pump Off";
            }
            else
            {
                buttonPeltFan.Text = "Pump On";
            }

            if(arduinoGamma.arduinoPeltierValue != "") 
            { 
                labelPeltierCurrent.Text = "Peltier Out: " + Math.Round(double.Parse(arduinoGamma.arduinoPeltierValue)); 
            }
            
            labelKu.Text = arduinoGamma.arduinoKu;
            labelPumpPower.Text = "Pump: " + arduinoGamma.arduinoPumpEnabled;
            labelFanCurrent.Text = "Fan: " + arduinoGamma.arduinoPeltierFansValue;
            labelTempSetpoint.Text = "SP0: " + arduinoGamma.arduinoTC0SP;
            labelTC1.Text = "TC0: " + arduinoGamma.arduinoTC0;
            labelINTLK.Text = "Interlock " + arduinoGamma.arduinoINTLK;
            labelWarningLight.Text = "Warning Light " + arduinoGamma.arduinoWARNINGLIGHT;

        }

       

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            while (ldx36000.RB_laserOut == "1")
            {
                ldx36000.QueueCommand("LAS:OUT 0");
                break;
            }
            ldx36000.Closedown();
            arduinoGamma.Closedown();
        }


            private void buttonLaserOut_Click(object sender, EventArgs e)
        {
            if (swLaserLoop.IsRunning)
            {
                swLaserLoop.Stop();
                swLaserLoop.Restart();
               
            }
            else
            {
                swLaserLoop.Start();
            }
            
            if (ldx36000.RB_laserOut == "1") 
            {
                ldx36000.QueueCommand("LAS:OUT 0");

                arduinoGamma.QueueCommand("WL 0");
                laserLastCmd = "OFF";
                //arduinoGamma.QueueCommand("LF 0");

               // ted4015.switchTecOutput(false);
                //tedTECOut = false;

            }
            else
            {
                if(buttonCurrentLimit.BackColor == Color.LightGreen && buttonLaserCurrentSetpoint.BackColor == Color.LightGreen)
                {
                    ted4015.switchTecOutput(true);
                    tedTECOut = true;
                    if (tedTECOut)
                    {
                        if (tedTemp < 30)
                        {   
                            //if(arduinoGamma.arduinoINTLK == "1" && INTLKENABLE)
                            //{
                                if (int.Parse(ldx36000.RB_laserTemperature.Split('.')[0]) < 30)
                                {
                                    panelLaser.BackColor = Color.PaleVioletRed;
                                    labelLaserOut.Text = "Laser Output ON";
                                    arduinoGamma.QueueCommand("WL 1");
                                    arduinoGamma.QueueCommand("LF 1");
                                    ldx36000.QueueCommand("LAS:OUT 1");
                                    laserLastCmd = "ON";

                            }
                                else
                                {
                                    MessageBox.Show("Laser seems too hot, LASER not turned on!");
                                }
                            //}
                            //else
                           // {
                            //    MessageBox.Show("Laser Hood not closed, LASER not turned on!");
                            //}

                           



                        }
                        else
                        {
                            MessageBox.Show("TED4015 seems too hot, LASER not turned on!");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("TED4015 is not operational, LASER not turned on!");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Current Limit or Setpoint not applied: Please Check before turning on!");
                }
                
            }
        }

        private void buttonCurrentLimit_Click(object sender, EventArgs e)
        {
            ldx36000.QueueCommand("LAS:LIM:I "+trackBarLaserCurrentLimit.Value);
        }


        private void timerUI_Tick(object sender, EventArgs e)
        {
            buttonCurrentLimit.Text = trackBarLaserCurrentLimit.Value + " A";
            buttonLaserCurrentSetpoint.Text = trackBarLaserCurrentSetpoint.Value + " A";

            buttonTempSetpoint.Text = trackBarTempSetpoint.Value + " C";

            TimeSpan time = TimeSpan.FromSeconds(swLaserLoop.ElapsedMilliseconds / 1000);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            string strTime = time.ToString(@"hh\:mm\:ss");

            if (checkBoxLasSeq.Checked)
            {
                labelLasSeqTimeRemaining.Text = strTime + " , loop: " + (laserLoopCount+1).ToString() + ", Step: " + laserLastCmd;

            }
            else
            {
                if (ldx36000.RB_laserOut == "1")
                {
                    labelLasSeqTimeRemaining.Text = strTime + " , loop: " + (laserLoopCount + 1).ToString() + ", Step: " + laserLastCmd;

                }
                else
                {
                    labelLasSeqTimeRemaining.Text = "";
                }
            }
        }

        private void buttonLaserCurrentSetpoint_Click(object sender, EventArgs e)
        {
            if (ldx36000.RB_laserOut == "1")
            {
                if (checkboxRampEnable.Checked)
                {
                    ldx36000.rampCurrentValue = int.Parse(ldx36000.RB_laserCurrentSetpoint.Split('.')[0]);
                    ldx36000.rampEnd = trackBarLaserCurrentSetpoint.Value;
                    if (ldx36000.ramp == true)
                    {
                        ldx36000.ramp = false;
                    }
                    else
                    {
                        ldx36000.ramp = true;
                    }
                }
                else
                {
                    ldx36000.QueueCommand("LAS:LDI " + trackBarLaserCurrentSetpoint.Value);
                }
                
               
            }
            else
            {
                if (int.Parse(ldx36000.RB_laserCurrentLimit.Split('.')[0]) < int.Parse(ldx36000.RB_laserCurrentSetpoint.Split('.')[0])) 
                {
                    MessageBox.Show("Current Limit is below Setpoint!");
                }
                ldx36000.QueueCommand("LAS:LDI " + trackBarLaserCurrentSetpoint.Value);
            }
        }

        private void buttonCONN_Click(object sender, EventArgs e)
        {
            if (buttonCONN.Text == "Connected")
            {
                ldx36000.Closedown();
            }
            else
            {
                ldx36000.Init();
            }
        }

        private void buttonClearErrors_Click(object sender, EventArgs e)
        {
            richTextBoxERRORS.Text = "";
        }

        

        private void buttonLaserCurrentLimitIncrease_Click(object sender, EventArgs e)
        {
            trackBarLaserCurrentLimit.Value += 1;
        }

        private void buttonLaserCurrentLimitDecrease_Click(object sender, EventArgs e)
        {
            trackBarLaserCurrentLimit.Value -= 1;
        }

        private void buttonTECOut_Click(object sender, EventArgs e)
        {
            if(ldx36000.RB_laserOut != "")
            {
                if (tedTECOut && int.Parse(ldx36000.RB_laserOut.Split('.')[0]) == 1)
                {
                    MessageBox.Show("Cannot turn off TEC when Laser is ON!");
                }
                else
                {
                    ted4015.switchTecOutput(!tedTECOut);
                }

            }

        }

        private void buttonLasFan_Click(object sender, EventArgs e)
        {
            if(int.Parse(arduinoGamma.arduinoRelayFansLaser.Split('.')[0]) == 1)
            {
                arduinoGamma.QueueCommand("LF 0");
                buttonLasFan.Text = "Turn Fan On";
            }
            else
            {
                arduinoGamma.QueueCommand("LF 1");
                buttonLasFan.Text = "Turn Fan Off";
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.Parse(arduinoGamma.arduinoPumpEnabled.Split('.')[0]) == 1)
            {
                arduinoGamma.QueueCommand("Pu 0");
            }
            else
            {
                arduinoGamma.QueueCommand("Pu 1");
            }
            
        }

        private void formsPlotArduino_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void formsPlotArduino_MouseLeave(object sender, EventArgs e)
        {
            plotArduinoLive = true;
        }

        private void formsPlotArduino_MouseEnter(object sender, EventArgs e)
        {
            plotArduinoLive = false;
        }

        private void buttonKu_Click(object sender, EventArgs e)
        {
            arduinoGamma.QueueCommand("Ku " + textBoxKu.Text);
        }

        private void panelLaser_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonLaserCurrentSetpointIncrease_Click(object sender, EventArgs e)
        {
            trackBarLaserCurrentSetpoint.Value += 1;
        }

        private void buttonLaserCurrentSetpointDecrease_Click(object sender, EventArgs e)
        {
            trackBarLaserCurrentSetpoint.Value -= 1;
        }

        private void buttonTempSetpoint_Click(object sender, EventArgs e)
        {
            arduinoGamma.QueueCommand("S0 " + trackBarTempSetpoint.Value);
        }

        void CreateThermalImageColorPalette()
        {
            // The palette is gnuplot's PM3D palette.
            // See here for details: https://stackoverflow.com/questions/28495390/thermal-imaging-palette
            for (int x = 0; x < 256; x++)
            {
                paletteR[x] = System.Convert.ToByte(255 * Math.Sqrt(x / 255.0));
                paletteG[x] = System.Convert.ToByte(255 * Math.Pow(x / 255.0, 3));
                if (Math.Sin(2 * Math.PI * (x / 255.0)) >= 0)
                {
                    paletteB[x] = System.Convert.ToByte(255 * Math.Sin(2 * Math.PI * (x / 255.0)));
                }
                else
                {
                    paletteB[x] = 0;
                }
            }
        }

        private void timerFLIR_Tick(object sender, EventArgs e)
        {
            //imageData = ti.GetHighContrastImage();

            

            pictureBoxFLIR.Image = ThermalImagePaint();
            if (checkBoxMirror.Checked) { pictureBoxFLIR.Image.RotateFlip(RotateFlipType.RotateNoneFlipX); }



            pictureBoxFLIR.Size = new Size(WIDTH * SCALE, HEIGHT * SCALE);

            

            ti.GetStatistics(out int[] spotmeterStatistics, out int[] temperatures, out byte resolution, out byte ffcStatus, out bool[] temperatureWarning);

            labelFLIRTemp.Text = "Mean : " + ((spotmeterStatistics[0] / 10.0) - 273.15).ToString("0.00") + " C";
            labelFLIRTempMax.Text = "Max  : " + ((spotmeterStatistics[1] / 10.0) - 273.15).ToString("0.00") + " C";
            labelFLIRTempMin.Text = "Min  : " + ((spotmeterStatistics[2] / 10.0) - 273.15).ToString("0.00") + " C";

            // byte[] spotmetercfg = new byte[4];

            // spotmetercfg = ti.GetSpotmeterConfig();
            //  Console.WriteLine(spotmetercfg[0] + " " + spotmetercfg[1] + " " + spotmetercfg[2] + " " + spotmetercfg[3]);

            
        }

        private void pictureBoxFLIR_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;

            int xcoord = coordinates.X / SCALE;
            int ycoord = coordinates.Y / SCALE;

            if (checkBoxMirror.Checked)
            {
                xcoord = WIDTH - xcoord;
            }

            switch (me.Button)
            {

                case MouseButtons.Left:
                    regionOfInterest[0] = (byte)(xcoord - 1);
                    regionOfInterest[1] = (byte)(ycoord-1);
                    regionOfInterest[2] = (byte)xcoord;
                    regionOfInterest[3] = (byte)ycoord;
                    break;

                case MouseButtons.Right:
                    regionOfInterest[0] = (byte)(0);
                    regionOfInterest[1] = (byte)(0);
                    regionOfInterest[2] = (byte)79;
                    regionOfInterest[3] = (byte)59;

                    break;
                    
            }

            
          
            //

            int[] cliplim = new int[2];
            cliplim[0] = 4800;
            cliplim[1] = 512;

            ti.SetSpotmeterConfig(regionOfInterest);
            //SetHighContrastConfig(byte[] regionOfInterest, int dampeningFactor, int[] clipLimit, int emptyCounts)
            //ti.SetHighContrastConfig(regionOfInterest, 64, cliplim, 2);

        }

        private void buttonSaveFLIRImage_Click(object sender, EventArgs e)
        {
            pictureBoxFLIR.Image.Save(@"C:\Users\Gamma_MRI\Desktop\GammaAshData\FLIR" + DateTime.Now.ToString("dd-MMMM-yyyy-HH-mm") + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void comboBoxLaserMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxLaserMode.SelectedIndex == 0)
            {
                ldx36000.QueueCommand("LAS:MODE:CW");
            }
            else if (comboBoxLaserMode.SelectedIndex == 1)
            {
                ldx36000.QueueCommand("LAS:MODE:HPULSE");
            }
            else if (comboBoxLaserMode.SelectedIndex == 2)
            {
                ldx36000.QueueCommand("LAS:MODE:PULSE");
            }
            else if (comboBoxLaserMode.SelectedIndex == 3)
            {
                ldx36000.QueueCommand("LAS:MODE:TRIG");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ldx36000.QueueCommand("LAS:DC " + textBoxLASDC.Text);
        }

        private void buttonLASPWP_Click(object sender, EventArgs e)
        {
            ldx36000.QueueCommand("LAS:PWP "+ textBoxLASPWP.Text);
        }

        private void buttonLASF_Click(object sender, EventArgs e)
        {
            ldx36000.QueueCommand("LAS:F " + textBoxLASF.Text);
        }

        private void timerLaserSeq_Tick(object sender, EventArgs e)
        {
            
            if (ldx36000.RB_laserOut == "1")
            {

                timerLaserSeq.Interval = 1000 * Int32.Parse(textBoxLasOFFTime.Text);
                

                laserLoopCount++;
                if (Int32.Parse(textBoxLasONOFFLoops.Text) > 0)
                {
                    if (laserLoopCount > Int32.Parse(textBoxLasONOFFLoops.Text) - 1)
                    {
                        timerLaserSeq.Enabled = false;
                        checkBoxLasSeq.Checked = false;
                    }
                }




            }
            else
            {
                timerLaserSeq.Interval = 1000 * Int32.Parse(textBoxLasONTime.Text);
            }
            buttonLaserOut.PerformClick();
         


        }

        private void buttonLASPWF_Click(object sender, EventArgs e)
        {
            ldx36000.QueueCommand("LAS:PWF " + textBoxLASPWF.Text);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxLasSeq_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBoxLasSeq.Checked)
            {

                
                
                


                laserLoopCount = 0;
                if (ldx36000.RB_laserOut == "1")
                {
                    timerLaserSeq.Interval = 1000 * Int32.Parse(textBoxLasONTime.Text);
                }
                else
                {
                    timerLaserSeq.Interval = 5000;
                    ldx36000.QueueCommand("LAS:MODE:CW");
                    comboBoxLaserMode.SelectedIndex = 0;
                }

                timerLaserSeq.Enabled = true;

            }
            else
            {
                timerLaserSeq.Enabled = false;
            }

            
        }


        

        Bitmap ThermalImagePaint()
        {
            // Create PNG with Bitmap from System.Drawing
            Bitmap bitmap = new Bitmap(WIDTH, HEIGHT);
            for (int row = 0; row < WIDTH; row++)
            {
                for (int col = 0; col < HEIGHT; col++)
                {
                    int color = imageData[row + col * WIDTH];
                    bitmap.SetPixel(row, col, Color.FromArgb(paletteR[color], paletteG[color], paletteB[color]));
                }
            }

            /*
         * regionOfInterest – Type: byte[], Length: 4
         0: firstColumn – Type: byte, Range: [0 to 79], Default: 39
         1: firstRow – Type: byte, Range: [0 to 59], Default: 29
         2: lastColumn – Type: byte, Range: [1 to 80], Default: 40
         3: lastRow – Type: byte, Range: [1 to 60], Default: 30
         */

           bitmap.SetPixel(regionOfInterest[0], regionOfInterest[1], Color.FromArgb(255, 255, 255));
           bitmap.SetPixel(regionOfInterest[2], regionOfInterest[1], Color.FromArgb(255, 255, 255));
           bitmap.SetPixel(regionOfInterest[0], regionOfInterest[3], Color.FromArgb(255, 255, 255));
           bitmap.SetPixel(regionOfInterest[2], regionOfInterest[3], Color.FromArgb(255, 255, 255));


            // Scale to bigger size (can be changed with SCALE constant)!
            bitmap = new Bitmap(bitmap, new Size(WIDTH * SCALE, HEIGHT * SCALE));




            return bitmap;
        }

        void TICallback(BrickletThermalImaging sender, byte[] image)
        {
            // Save image and trigger paint event handler
            imageData = image;



        }

        void FLIR_Load()
        {

            regionOfInterest[0] = (byte)39;
            regionOfInterest[1] = (byte)29;
            regionOfInterest[2] = (byte)40;
            regionOfInterest[3] = (byte)30;

            ipcon = new IPConnection(); // Create IP connection
            ti = new BrickletThermalImaging(UID, ipcon); // Create device object

            ipcon.Connect(HOST, PORT); // Connect to brickd
                                       // Don't use device before ipcon is connected

            ti.SetImageTransferConfig(BrickletThermalImaging.IMAGE_TRANSFER_CALLBACK_HIGH_CONTRAST_IMAGE);

            ti.SetResolution(BrickletThermalImaging.RESOLUTION_0_TO_6553_KELVIN);

            CreateThermalImageColorPalette();

            ti.HighContrastImageCallback += TICallback;

            timerFLIR.Interval = 200;

            timerFLIR.Enabled = true;

            pictureBoxFLIR.SizeMode = PictureBoxSizeMode.StretchImage;


        }

    }
}
