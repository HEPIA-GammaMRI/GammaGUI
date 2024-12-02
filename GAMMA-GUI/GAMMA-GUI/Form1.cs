// System namespaces for core functionality
using System;                          // Fundamental types and base class libraries
using System.IO;                       // File and directory operations
using System.Net.Http;                 // HTTP request and response handling
using System.Diagnostics;              // Event logging, performance counters, etc.
using System.Linq;                     // LINQ queries and operations on collections
using System.Runtime.CompilerServices; // Support for compiler-specific features
using System.Threading.Tasks;

// Libraries for Windows Forms (UI-based applications)
using System.Windows.Forms;           // Windows Forms for building desktop applications
using System.Windows.Forms.DataVisualization.Charting; // Charting controls in Windows Forms

// Libraries for scientific or hardware interactions
using Thorlabs.TL4000;                 // Thorlabs TL4000 series optical devices and controllers
using Tinkerforge;                     // Tinkerforge modular hardware platform for sensors, motors, etc.
using ScottPlot;                       // ScottPlot library for creating plots and visualizations
using ScottPlot.Renderable;            // Access to renderable components in ScottPlot

// Utility libraries
using System.Drawing;                  // Graphics, images, and drawing


namespace GAMMA_GUI
{
    public partial class Form1 : Form
    {
        // Class-level variables and configurations for managing various devices and data

        // Devices and communication interfaces
        private laserGPIB ldx36000;                // Laser GPIB communication object
        private gammaARDUINO arduinoGamma;         // Arduino communication object
        private TL4000 ted4015;                    // Thorlabs TL4000 device controller

        // Network configuration for connecting to devices (e.g., Tinkerforge)
        private static string HOST = "localhost";  // Host address for the device connection
        private static int PORT = 4223;            // Port number for the device connection
        private static string UID = "RkH";         // UID of the Thermal Imaging Bricklet (Tinkerforge)

        // Thermal imaging configuration (resolution and scaling)
        private static int WIDTH = 80;             // Image width in pixels
        private static int HEIGHT = 60;            // Image height in pixels
        private static int SCALE = 3;              // Image scaling factor

        // Arrays for storing thermal image data
        private byte[] imageData = new byte[WIDTH * HEIGHT];  // Image data (80x60 pixels)

        // Color palette for thermal image (blue = cold, red = hot)
        private byte[] paletteR = new byte[256];    // Red channel values for color palette
        private byte[] paletteG = new byte[256];    // Green channel values for color palette
        private byte[] paletteB = new byte[256];    // Blue channel values for color palette

        // Region of interest (ROI) for thermal image processing
        private byte[] regionOfInterest = new byte[4];  // A 4-byte array for ROI (e.g., X, Y, Width, Height)

        // Tinkerforge connection and device initialization
        private IPConnection ipcon;                // Tinkerforge IP connection object
        private BrickletThermalImaging ti;          // Thermal imaging bricklet object

        // Control flags for various operations
        private bool INTLKENABLE;                  // Enable/Disable interlock

        // Temperature and current readings for the Thorlabs TED4015 TEC controller
        private bool tedTECOut;                    // Flag to indicate TED4015 TEC output state
        private double tedTemp;                    // Current temperature reading from TED4015
        private double tedTempSetpoint;            // Setpoint temperature for TED4015
        private double tedTECCurrentReading;       // Current reading of the TED4015 TEC current
        private double tedTECVoltageReading;       // Current reading of the TED4015 TEC voltage

        // Flags for managing the reading of temperature and current limits
        private bool firstReadCurrentSetpoint;     // Flag for the first reading of current setpoint
        private bool firstReadCurrentLimit;        // Flag for the first reading of current limit

        // Arrays for storing readings from Arduino and other devices
        private double[] arduinoT0Array = new double[] { };  // Array for storing Arduino T0 readings
        private double[] arduinoS0Array = new double[] { };  // Array for storing Arduino S0 readings
        private double[] arduinoPeArray = new double[] { };  // Array for storing Arduino Pe readings
        private double[] dateTimeArray = new double[] { };   // Array for storing corresponding date-time data

        // Arrays for storing temperature readings from TED4015
        private double[] ted4015tempArray = new double[] { };  // Temperature readings from TED4015

        // Array for storing the last temperature readings
        private double[] lastempArray = new double[] { };  // Last known temperature readings

        // Plot configuration
        private const int maxPlotPoints = 100; // Maximum number of points to plot

        // Flags for axis and plot initialization
        private bool axisPeInited;              // Flag to check if PE axis is initialized
        private int peAxisIndex;                // Index for the PE axis in plots
        private bool plotArduinoLive;           // Flag to indicate whether to plot Arduino live data

        // Laser loop configuration and timing
        private int laserLoopCount;             // Counter for laser loop iterations
        private Stopwatch swLaserLoop = new Stopwatch();  // Stopwatch to time the laser loop
        private string laserLastCmd;            // Store the last laser command sent


        public Form1()
        {
            // Initialize the components and form properties
            InitializeComponent();

            // Attach event handler for form closing
            this.FormClosing += Form1_FormClosing;

            // Initialize flags for the first read of current setpoint and limit
            firstReadCurrentSetpoint = true;
            firstReadCurrentLimit = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize laser loop count and interlock enable flag
                laserLoopCount = 0;
                INTLKENABLE = false;

                // Flags for plotting data and initializing PE axis
                plotArduinoLive = true;
                axisPeInited = false;

                // Initialize the laser controller
                ldx36000 = new laserGPIB();
                ldx36000.Init();
                comboBoxLaserMode.SelectedIndex = 2;  // Default laser mode, ensure this index is valid

                // Initialize Arduino communication
                arduinoGamma = new gammaARDUINO();
                arduinoGamma.Init();

                // Initialize the TED4015 controller with the provided connection string
                ted4015 = new TL4000("USB::4883::32840::M00429417::INSTR", true, false);

                // Set the timer interval for reading data (1 second)
                timerUIReadbacks.Interval = 1000;
                timerUIReadbacks.Enabled = true;

                // Initially hide the laser picture box
                pictureBoxLASER.Visible = false;

                // Set the UI update timer interval (10 ms) and enable it
                timerUILaser.Interval = 10;
                timerUILaser.Enabled = true;

                // Set the default state of the Ramp checkbox to unchecked and set text
                checkboxRampEnable.Checked = false;
                checkboxRampEnable.Text = "Ramp";

                // Initialize laser fan button text
                buttonLasFan.Text = "Turn Fan On";

                // Load FLIR camera settings (assuming FLIR_Load handles initialization)
                FLIR_Load();

                // Enable the ThingSpeak timer for data transmission
                timerThingSpeak.Enabled = true;
            }
            catch (Exception ex)
            {
                // Handle any errors during the initialization process
                MessageBox.Show($"An error occurred during initialization: {ex.Message}", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Optionally log the error for further debugging
                LogError(ex);
            }
        }

        /// <summary>
        /// Logs error messages to a file for further analysis. This method can be expanded to use logging libraries like log4net or NLog.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        private void LogError(Exception ex)
        {
            try
            {
                string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "error_log.txt");
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine($"[{DateTime.Now}] Error: {ex.Message}");
                    sw.WriteLine($"Stack Trace: {ex.StackTrace}");
                }
            }
            catch (Exception logEx)
            {
                // If logging fails, show the error to the user
                MessageBox.Show($"An error occurred while logging the original error: {logEx.Message}", "Logging Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Handles the Tick event of the timer. This method is executed at regular intervals to update the UI and log data.
        /// It logs the current temperature and updates various readings, graphs, and device statuses.
        /// Additionally, it handles any potential errors and updates the UI with the latest information.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Timer.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void timerUIReadbacks_Tick(object sender, EventArgs e)
        {
            string formattedDateTime = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            string formattedShortDateTime = DateTime.Now.ToString("HH:mm:ss");
            string filepath = $"C:/Users/Gamma_MRI/Desktop/GammaAshData/{DateTime.Now:dd-MMMM-yyyy-HH-mm}.csv";

            // Logging temperature to file
            LogTemperatureToFile(formattedDateTime);

            try
            {
                // Fetch and display temperature readings
                UpdateTemperatureReadings();
                UpdateDeviceReadings();

                // Handle graphing of temperature data
                UpdateTemperatureGraph();

                // Handle device error checking and warnings
                CheckDeviceErrors();

                // Update status labels and controls
                UpdateUI(formattedShortDateTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        /// <summary>
        /// Logs the temperature reading to a CSV file with the current timestamp.
        /// </summary>
        /// <param name="formattedDateTime">The formatted date and time string to log with the temperature.</param>
        private void LogTemperatureToFile(string formattedDateTime)
        {
            // Define the file path for logging the temperature data
            string filepath = "C:/Users/Gamma_MRI/Desktop/GammaAshData/" + DateTime.Now.ToString("dd-MMMM-yyyy-HH-mm") + ".csv";

            // Open the file in append mode and write the temperature log
            using (Stream stream = File.Open(filepath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                // Write the formatted log entry with temperature reading
                writer.WriteLine($"{formattedDateTime} - TempRead: {tedTemp:+0.##;-0.##;(0)} C");
            }
        }

        /// <summary>
        /// Updates the temperature readings and displays them on corresponding labels.
        /// </summary>
        private void UpdateTemperatureReadings()
        {
            // Measure and update the current temperature reading from the TED device
            ted4015.measTemp(out tedTemp);
            labelTedTemp.Text = $"T reading: {tedTemp:+0.##;-0.##;(0)} C";

            // Get and display the temperature setpoint from the TED device
            ted4015.getTempSetpoint(0, out tedTempSetpoint);
            labelTedTempSetpoint.Text = $"T setpoint: {tedTempSetpoint:+0.##;-0.##;(0)} C";

            // Get the TEC output state and current reading
            ted4015.getTecOutputState(out tedTECOut);
            ted4015.measTecCurr(out tedTECCurrentReading);
            labelTedTECCurrent.Text = $"TEC Current: {tedTECCurrentReading:+0.##;-0.##;(0)} A";

            // Measure and display the TEC voltage reading
            ted4015.measTecVolt(out tedTECVoltageReading);
            labelTedTECVoltage.Text = $"TEC Voltage: {tedTECVoltageReading:+0.##;-0.##;(0)} V";

            // Update the TEC button state based on the TEC output state (ON or OFF)
            buttonTECOut.BackColor = tedTECOut ? Color.Green : Color.LightGray;
            buttonTECOut.Text = tedTECOut ? "TEC ON" : "TEC OFF";
        }

        /// <summary>
        /// Updates device readings by adding the most recent values to time series data arrays.
        /// </summary>
        private void UpdateDeviceReadings()
        {
            // Update time series data with the latest readings
            UpdateArrayData(ref dateTimeArray, DateTime.Now.ToOADate());
            UpdateArrayData(ref ted4015tempArray, tedTemp);
            UpdateArrayData(ref arduinoT0Array, double.Parse(arduinoGamma.arduinoTC0));
            UpdateArrayData(ref arduinoS0Array, double.Parse(arduinoGamma.arduinoTC0SP));
            UpdateArrayData(ref arduinoPeArray, double.Parse(arduinoGamma.arduinoPeltierValue));
            UpdateArrayData(ref lastempArray, double.Parse(ldx36000.RB_laserTemperature));
        }

        /// <summary>
        /// Updates the array by adding a new value and ensuring the array length does not exceed a maximum value.
        /// </summary>
        /// <param name="array">The array to be updated.</param>
        /// <param name="value">The new value to add to the array.</param>
        private void UpdateArrayData(ref double[] array, double value)
        {
            // If the array exceeds the maximum number of points, shift all elements to the left
            if (array.Length > maxPlotPoints)
            {
                Array.Copy(array, 1, array, 0, array.Length - 1);
            }
            else
            {
                // Otherwise, resize the array to accommodate the new value
                Array.Resize(ref array, array.Length + 1);
            }

            // Add the new value to the end of the array
            array[array.Length - 1] = value;
        }


        /// <summary>
        /// Updates the temperature graphs by plotting data for TED4015, Arduino, and Laser temperature readings.
        /// </summary>
        private void UpdateTemperatureGraph()
        {
            // Plot TED4015 temperature data on the formsPlotTED4015 graph
            formsPlotTED4015.Plot.Clear();  // Clear previous plots
            formsPlotTED4015.Plot.AddScatterLines(dateTimeArray, ted4015tempArray, Color.Red, 2, ScottPlot.LineStyle.Solid, "T0");  // Add TED4015 temperature data (T0)
            formsPlotTED4015.Plot.YAxis.Label("Temperature");  // Label the Y axis as Temperature
            formsPlotTED4015.Plot.XAxis.DateTimeFormat(true);  // Set X axis to display dates/times
            formsPlotTED4015.Plot.AxisAuto(0, 0.2);  // Auto-scale the axes
            formsPlotTED4015.Refresh(false, true);  // Refresh the plot to show the updates

            // Plot Arduino temperature data on the formsPlotArduino graph
            formsPlotArduino.Plot.Clear();  // Clear previous plots
            formsPlotArduino.Plot.AddScatterLines(dateTimeArray, arduinoT0Array, Color.Red, 2, ScottPlot.LineStyle.Solid, "T0");  // Add Arduino T0 data (Temperature)
            formsPlotArduino.Plot.AddScatterLines(dateTimeArray, arduinoS0Array, Color.Blue, 2, ScottPlot.LineStyle.Solid, "S0");  // Add Arduino S0 data (Setpoint)
            formsPlotArduino.Plot.AddScatterLines(dateTimeArray, arduinoPeArray, Color.Orange, 1, ScottPlot.LineStyle.Solid, "Pelt0");  // Add Arduino Peltier data
            formsPlotArduino.Plot.YAxis.Label("Temperature");  // Label the Y axis as Temperature
            formsPlotArduino.Plot.XAxis.DateTimeFormat(true);  // Set X axis to display dates/times
            formsPlotArduino.Plot.AxisAuto(0, 0.2);  // Auto-scale the axes
            formsPlotArduino.Refresh(false, true);  // Refresh the plot to show the updates

            // Plot Laser temperature data on the formsPlotLasTemp graph
            formsPlotLasTemp.Plot.Clear();  // Clear previous plots
            formsPlotLasTemp.Plot.AddScatterLines(dateTimeArray, lastempArray, Color.Red, 2, ScottPlot.LineStyle.Solid, "Temp");  // Add Laser temperature data
            formsPlotLasTemp.Plot.YAxis.Label("Temperature");  // Label the Y axis as Temperature
            formsPlotLasTemp.Plot.XAxis.DateTimeFormat(true);  // Set X axis to display dates/times
            formsPlotLasTemp.Plot.AxisAuto(0, 0.2);  // Auto-scale the axes
            formsPlotLasTemp.Refresh(false, true);  // Refresh the plot to show the updates
        }

        /// <summary>
        /// Checks for errors in the laser device and handles them by turning off the laser and displaying an error message.
        /// </summary>
        private void CheckDeviceErrors()
        {
            // Check if laser readbacks are available
            if (ldx36000.ReadbacksAvailable)
            {
                // Parse the laser temperature from the readbacks and check if it exceeds 50C
                if (int.TryParse(ldx36000.RB_laserTemperature.Split('.')[0], out int temp) && temp > 50)
                {
                    // Turn off the laser if the temperature exceeds the threshold
                    ldx36000.QueueCommand("LAS:OUT 0");

                    // Check if the laser output is on and display the appropriate error message
                    string errorMessage = int.Parse(ldx36000.RB_laserOut.Split('.')[0]) == 1
                        ? "Laser Hood opened, LASER turned OFF!"
                        : "LASER TEMPERATURE HAS EXCEEDED 50C. \n PLEASE CHECK TEMPERATURE CONTROLLER AND FANS!";

                    // Show an error message to the user
                    MessageBox.Show(errorMessage);
                }
            }
        }


        /// <summary>
        /// Updates the UI with the current device status and readings.
        /// </summary>
        private void UpdateUI(string formattedShortDateTime)
        {
            // Update the reply message from GPIB
            labelReplyFromGPIB.Text = $"{formattedShortDateTime} {ldx36000.replyFromGPIB}";

            // Update laser output and mode status
            panelLaser.BackColor = ldx36000.RB_laserOut == "1" ? Color.PaleVioletRed : SystemColors.Control;
            labelLaserOut.Text = ldx36000.RB_laserOut == "1" ? "Laser Output ON" : "Laser Output OFF";
            pictureBoxLASER.Visible = ldx36000.RB_laserOut == "1";
            labelLaserMode.Text = $"Laser Mode: {ldx36000.RB_laserMode}";
            labelLaserCurrentLimit.Text = $"I limit: {ldx36000.RB_laserCurrentLimit} A";
            labelLaserCurrentPhotoDiode.Text = $"IPhotoDiode: {ldx36000.RB_laserCurrentPhotoDiode} A";
            labelLaserCurrentSetpoint.Text = $"I Setpoint: {ldx36000.RB_laserCurrentSetpoint} A";
            labelLaserTemperature.Text = $"Temperature: {ldx36000.RB_laserTemperature} C";
            labelLaserPower.Text = $"Power: {ldx36000.RB_laserPower} W";

            // Update laser current limit control
            if (!string.IsNullOrEmpty(ldx36000.RB_laserCurrentLimit) && !ldx36000.RB_laserCurrentLimit.Contains(","))
            {
                // Check if the current limit value matches the trackbar value
                if (int.Parse(ldx36000.RB_laserCurrentLimit.Split('.')[0]) == trackBarLaserCurrentLimit.Value)
                {
                    buttonCurrentLimit.BackColor = Color.LightGreen;  // Light Green when match
                }
                else
                {
                    if (firstReadCurrentLimit)
                    {
                        // Set the trackbar value to match the current limit on first read
                        trackBarLaserCurrentLimit.Value = int.Parse(ldx36000.RB_laserCurrentLimit.Split('.')[0]);
                        firstReadCurrentLimit = false;
                    }
                    buttonCurrentLimit.BackColor = Color.PaleVioletRed;  // Pale Violet Red when not matching
                }
            }

            // Update laser current setpoint control
            if (!string.IsNullOrEmpty(ldx36000.RB_laserCurrentSetpoint))
            {
                // Check if the current setpoint matches the trackbar value
                if (int.Parse(ldx36000.RB_laserCurrentSetpoint.Split('.')[0]) == trackBarLaserCurrentSetpoint.Value)
                {
                    buttonLaserCurrentSetpoint.BackColor = Color.LightGreen;  // Light Green when match
                }
                else
                {
                    if (firstReadCurrentSetpoint)
                    {
                        // Set the trackbar value to match the current setpoint on first read
                        trackBarLaserCurrentSetpoint.Value = int.Parse(ldx36000.RB_laserCurrentSetpoint.Split('.')[0]);
                        firstReadCurrentSetpoint = false;
                    }
                    buttonLaserCurrentSetpoint.BackColor = Color.PaleVioletRed;  // Pale Violet Red when not matching
                }
            }

            // Update connection status
            buttonCONN.Text = ldx36000.Connected ? "Connected" : "Disconnected";
            buttonCONN.BackColor = ldx36000.Connected ? Color.Green : Color.Red;

            // Update laser output control button (Turn On/Off)
            buttonLaserOut.Text = ldx36000.RB_laserOut == "0" ? "Turn On" : "Turn Off";

            // Update pump control button
            buttonPeltFan.Text = arduinoGamma.arduinoPumpEnabled == "0" ? "Pump Off" : "Pump On";

            // Update various device parameters (Peltier, Temperature Setpoint, etc.)
            labelPeltierCurrent.Text = $"Peltier Out: {Math.Round(double.Parse(arduinoGamma.arduinoPeltierValue))}";
            labelKu.Text = arduinoGamma.arduinoKu;
            labelPumpPower.Text = $"Pump: {arduinoGamma.arduinoPumpEnabled}";
            labelFanCurrent.Text = $"Fan: {arduinoGamma.arduinoPeltierFansValue}";
            labelTempSetpoint.Text = $"SP0: {arduinoGamma.arduinoTC0SP}";
            labelTC1.Text = $"TC0: {arduinoGamma.arduinoTC0}";
            labelINTLK.Text = $"Interlock {arduinoGamma.arduinoINTLK}";
            labelWarningLight.Text = $"Warning Light {arduinoGamma.arduinoWARNINGLIGHT}";
        }



        // Method to send data to ThingSpeak asynchronously
        private async void sendDataTS(string input)
        {
            // Base URL for ThingSpeak API request
            string baseUrl = "https://api.thingspeak.com/update?api_key=Q8WGCZUGGCQ221SV";

            // Construct the full URL by appending the input parameters to the base URL
            string url = $"{baseUrl}&{input}";

            try
            {
                // Send an HTTP GET request to ThingSpeak and await the response asynchronously
                string response = await ThingSpeak.SendGetRequest(url);

                // Log the URL and server response for debugging purposes
                Console.WriteLine($"{url}  Server Response: {response}");
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the request for debugging purposes
                Console.WriteLine($"{url}  An error occurred: {ex.Message}");
            }
        }

        // Event handler to ensure the laser is turned off and connections are closed when the form is closing
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the laser is on before turning it off
            if (ldx36000.RB_laserOut == "1")
            {
                // Send the command to turn off the laser output
                ldx36000.QueueCommand("LAS:OUT 0");

                // Wait for the laser to turn off by repeatedly checking the laser state
                int timeout = 5000; // Timeout in milliseconds (e.g., 5 seconds)
                int elapsedTime = 0;
                int checkInterval = 500; // Interval between checks (e.g., 500 ms)

                // Poll the laser status every checkInterval milliseconds until the laser turns off
                while (ldx36000.RB_laserOut == "1" && elapsedTime < timeout)
                {
                    // Wait for a short interval before checking again
                    await Task.Delay(checkInterval);
                    elapsedTime += checkInterval;
                }

                // If the laser didn't turn off after the timeout, log a message or handle it
                if (ldx36000.RB_laserOut == "1")
                {
                    Console.WriteLine("Warning: Laser did not turn off within the expected time.");
                }
                else
                {
                    Console.WriteLine("Laser turned off successfully.");
                }
            }

            // Safely close the laser device connection
            ldx36000.Closedown();

            // Safely close the Arduino connection
            arduinoGamma.Closedown();
        }



        /// <summary>
        /// Handles the click event to toggle the laser output on or off.
        /// </summary>
        private void buttonLaserOut_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the laser loop stopwatch is running
                if (swLaserLoop.IsRunning)
                {
                    // Stop and restart the stopwatch to reset the timing
                    swLaserLoop.Stop();
                    swLaserLoop.Restart();
                }
                else
                {
                    // If the stopwatch isn't running, start it
                    swLaserLoop.Start();
                }

                // Check the current status of the laser output
                if (ldx36000.RB_laserOut == "1")
                {
                    // If the laser output is already ON, turn it off
                    TurnOffLaser();
                }
                else
                {
                    // Laser output is OFF, proceed with checks to turn it ON
                    TryTurnOnLaser();
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors during laser control
                Console.WriteLine($"Error during laser control: {ex.Message}");
                MessageBox.Show("An error occurred while toggling the laser output.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Turns off the laser output and related components.
        /// </summary>
        private void TurnOffLaser()
        {
            // Queue the command to turn off the laser output
            ldx36000.QueueCommand("LAS:OUT 0");
            arduinoGamma.QueueCommand("WL 0"); // Turn off the laser light

            // Update the command tracking and UI
            laserLastCmd = "OFF";
            panelLaser.BackColor = SystemColors.Control; // Reset laser panel color
            labelLaserOut.Text = "Laser Output OFF"; // Update label status

            // Optionally switch off other components if needed (commented out for now)
            // ted4015.switchTecOutput(false);
            // tedTECOut = false;
        }

        /// <summary>
        /// Attempts to turn on the laser output after validating various conditions.
        /// </summary>
        private void TryTurnOnLaser()
        {
            // Check if both the current limit and the current setpoint are applied (Light Green means valid)
            if (buttonCurrentLimit.BackColor == Color.LightGreen && buttonLaserCurrentSetpoint.BackColor == Color.LightGreen)
            {
                // Check if TED4015 is operational (its output should be ON)
                // Assuming switchTecOutput returns an int (success or error code), e.g., 0 for success, non-zero for failure
                int result = ted4015.switchTecOutput(true);

                if (result == 0)  // or check for a specific success value based on the return code
                {
                    tedTECOut = true;

                    // If TED4015 is operational, proceed with further checks
                    if (tedTemp < 30)
                    {
                        // Check if the laser temperature is under control (less than 30°C)
                        if (int.Parse(ldx36000.RB_laserTemperature.Split('.')[0]) < 30)
                        {
                            // If both temperatures are acceptable, turn on the laser output
                            TurnOnLaser();
                        }
                        else
                        {
                            // If laser temperature is too high, show a warning and do not turn on the laser
                            MessageBox.Show("Laser seems too hot, LASER not turned on!");
                        }
                    }
                    else
                    {
                        // If TED4015 is too hot, show a warning and do not turn on the laser
                        MessageBox.Show("TED4015 seems too hot, LASER not turned on!");
                    }
                }
                else
                {
                    // If TED4015 is not operational, show a warning and do not turn on the laser
                    MessageBox.Show("TED4015 is not operational, LASER not turned on!");
                }
            }
            else
            {
                // If current limit or setpoint isn't properly applied, show a warning
                MessageBox.Show("Current Limit or Setpoint not applied: Please Check before turning on!");
            }
        }

        /// <summary>
        /// Turns on the laser output and related components.
        /// </summary>
        private void TurnOnLaser()
        {
            // Indicate that the laser is being turned on
            panelLaser.BackColor = Color.PaleVioletRed; // Color for "turning on"
            labelLaserOut.Text = "Laser Output ON"; // Update label status

            // Queue commands to turn on the laser and associated components
            arduinoGamma.QueueCommand("WL 1");  // Turn on laser light
            arduinoGamma.QueueCommand("LF 1");  // Turn on laser fan (or related functionality)
            ldx36000.QueueCommand("LAS:OUT 1"); // Turn on laser output

            // Update the command tracking and UI
            laserLastCmd = "ON"; // Track the last command sent
        }


        /// <summary>
        /// Handles the click event for setting the laser current limit.
        /// </summary>
        private void buttonCurrentLimit_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current value from the trackbar (laser current limit)
                int currentLimit = trackBarLaserCurrentLimit.Value;

                // Optional: Add a validation step for the current limit value if necessary
                // Example validation (uncomment if needed):
                // if (currentLimit < MIN_LIMIT || currentLimit > MAX_LIMIT)
                // {
                //     MessageBox.Show("Current limit is out of valid range.");
                //     return;
                // }

                // Construct the command to set the laser current limit
                string command = $"LAS:LIM:I {currentLimit}";
                ldx36000.QueueCommand(command);

                // Optionally, log the successful setting of the current limit
                Console.WriteLine($"Laser current limit set to: {currentLimit}");

                // Provide visual feedback on the button to indicate success
                buttonCurrentLimit.BackColor = Color.LightGreen; // Green color for success
                buttonCurrentLimit.Text = "Limit Set"; // Change text to indicate action completion
            }
            catch (Exception ex)
            {
                // Log the error for troubleshooting purposes
                Console.WriteLine($"Error setting laser current limit: {ex.Message}");

                // Provide feedback to the user that an error occurred
                MessageBox.Show("An error occurred while setting the laser current limit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Change button color to indicate failure (error)
                buttonCurrentLimit.BackColor = Color.PaleVioletRed; // Red color for failure
                buttonCurrentLimit.Text = "Error"; // Update text to indicate error
            }
        }


        /// <summary>
        /// Handles the Tick event of the laser timer to update the user interface. It updates button texts with current values of trackbars,
        /// calculates and displays elapsed time in the format hh:mm:ss, and updates the status of the laser output and sequence information.
        /// The method also handles possible errors that may occur during the update process.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Timer.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void timerUILaser_Tick(object sender, EventArgs e)
        {
            try
            {
                // Update the text on buttons to reflect the current values of trackbars
                buttonCurrentLimit.Text = $"{trackBarLaserCurrentLimit.Value} A";
                buttonLaserCurrentSetpoint.Text = $"{trackBarLaserCurrentSetpoint.Value} A";
                buttonTempSetpoint.Text = $"{trackBarTempSetpoint.Value} C";

                // Calculate elapsed time in seconds and format it as hh:mm:ss
                TimeSpan time = TimeSpan.FromSeconds(swLaserLoop.ElapsedMilliseconds / 1000);
                string strTime = time.ToString(@"hh\:mm\:ss");

                // Update the remaining time and loop/step information based on checkBoxLasSeq
                if (checkBoxLasSeq.Checked)
                {
                    labelLasSeqTimeRemaining.Text = $"{strTime} , loop: {laserLoopCount + 1}, Step: {laserLastCmd}";
                }
                else
                {
                    // Display only time if laser output is ON, otherwise hide the label
                    if (ldx36000.RB_laserOut == "1")
                    {
                        labelLasSeqTimeRemaining.Text = $"{strTime} , loop: {laserLoopCount + 1}, Step: {laserLastCmd}";
                    }
                    else
                    {
                        labelLasSeqTimeRemaining.Text = string.Empty; // Clear the label if laser output is off
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions and provide user feedback
                Console.WriteLine($"Error in timerUILaser_Tick: {ex.Message}");
                MessageBox.Show("An error occurred while updating the UI.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// Handles the click event for the Laser Current Setpoint button. This method updates the laser current setpoint value based on user input.
        /// It checks whether the laser output is ON, and if ramping is enabled. If ramping is enabled, the current value is ramped to the target setpoint.
        /// Otherwise, it directly sets the laser current to the desired setpoint. It also ensures that the setpoint does not exceed the current limit.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void buttonLaserCurrentSetpoint_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if laser output is ON
                if (ldx36000.RB_laserOut == "1")
                {
                    // If ramping is enabled, prepare for current ramping
                    if (checkboxRampEnable.Checked)
                    {
                        // Set the current ramp start value and the target end value
                        ldx36000.rampCurrentValue = int.Parse(ldx36000.RB_laserCurrentSetpoint.Split('.')[0]);
                        ldx36000.rampEnd = trackBarLaserCurrentSetpoint.Value;

                        // Toggle ramping state
                        ldx36000.ramp = !ldx36000.ramp;
                    }
                    else
                    {
                        // Directly set the laser current to the new setpoint
                        ldx36000.QueueCommand($"LAS:LDI {trackBarLaserCurrentSetpoint.Value}");
                    }
                }
                else
                {
                    // If laser output is OFF, check if the setpoint exceeds the current limit
                    int currentLimit = int.Parse(ldx36000.RB_laserCurrentLimit.Split('.')[0]);
                    int currentSetpoint = int.Parse(ldx36000.RB_laserCurrentSetpoint.Split('.')[0]);

                    if (currentLimit < currentSetpoint)
                    {
                        // Show a message if the current setpoint exceeds the current limit
                        MessageBox.Show("Current Limit is below Setpoint!");
                    }

                    // Set the laser current to the desired setpoint
                    ldx36000.QueueCommand($"LAS:LDI {trackBarLaserCurrentSetpoint.Value}");
                }
            }
            catch (FormatException ex)
            {
                // Handle any parsing errors for current values (e.g., if the value isn't a valid number)
                Console.WriteLine($"Error parsing values: {ex.Message}");
                MessageBox.Show("Invalid format for current value or limit. Please check the inputs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Catch any other errors
                Console.WriteLine($"Error in buttonLaserCurrentSetpoint_Click: {ex.Message}");
                MessageBox.Show("An error occurred while setting the laser current. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Handles the click event for the "Connect" button. It toggles the connection state. If the device is connected, it calls the Closedown method to disconnect.
        /// If the device is not connected, it calls the Init method to establish a connection with the device.
        /// </summary>
        /// <param name="sender">The source of the event, typically the "Connect" button that was clicked.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void buttonCONN_Click(object sender, EventArgs e)
        {
            try
            {
                // Check the current state of the connection button
                if (buttonCONN.Text == "Connected")
                {
                    // If the device is already connected, disconnect by calling Closedown
                    ldx36000.Closedown();
                    buttonCONN.Text = "Disconnected";  // Update the button text to reflect the new state
                }
                else
                {
                    // If the device is not connected, establish the connection by calling Init
                    ldx36000.Init();
                    buttonCONN.Text = "Connected";  // Update the button text to reflect the new state
                }
            }
            catch (Exception ex)
            {
                // Log the error if an exception occurs during the connection process
                Console.WriteLine($"Error in buttonCONN_Click: {ex.Message}");
                MessageBox.Show("An error occurred while trying to connect/disconnect. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Handles the click event for the "Clear Errors" button. It clears the contents of the rich text box that displays errors.
        /// This is typically used to reset the error log or clear the error display for a fresh start.
        /// </summary>
        /// <param name="sender">The source of the event, typically the "Clear Errors" button that was clicked.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void buttonClearErrors_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear the error display by setting the richTextBox's text to an empty string
                richTextBoxERRORS.Clear();
            }
            catch (Exception ex)
            {
                // Log any unexpected exceptions that might occur during the clear operation
                Console.WriteLine($"Error in buttonClearErrors_Click: {ex.Message}");
                MessageBox.Show("An error occurred while clearing the error log. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// Handles the click event for the "Increase Laser Current Limit" button. 
        /// It increases the value of the laser current limit by 1. This is linked to the track bar for laser current limit adjustment.
        /// </summary>
        /// <param name="sender">The source of the event, typically the "Increase Laser Current Limit" button that was clicked.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void buttonLaserCurrentLimitIncrease_Click(object sender, EventArgs e)
        {
            try
            {
                // Increase the value of the trackBarLaserCurrentLimit by 1
                // Ensure the value does not exceed the maximum limit (assuming 100 is the max, adjust as needed)
                if (trackBarLaserCurrentLimit.Value < trackBarLaserCurrentLimit.Maximum)
                {
                    trackBarLaserCurrentLimit.Value += 1;
                }
                else
                {
                    MessageBox.Show("Maximum laser current limit reached.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected exceptions that might occur
                Console.WriteLine($"Error in buttonLaserCurrentLimitIncrease_Click: {ex.Message}");
                MessageBox.Show("An error occurred while increasing the laser current limit. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the click event for the "Decrease Laser Current Limit" button. 
        /// It decreases the value of the laser current limit by 1. This is linked to the track bar for laser current limit adjustment.
        /// </summary>
        /// <param name="sender">The source of the event, typically the "Decrease Laser Current Limit" button that was clicked.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void buttonLaserCurrentLimitDecrease_Click(object sender, EventArgs e)
        {
            try
            {
                // Decrease the value of the trackBarLaserCurrentLimit by 1
                // Ensure the value does not go below the minimum limit (assuming 0 is the minimum, adjust as needed)
                if (trackBarLaserCurrentLimit.Value > trackBarLaserCurrentLimit.Minimum)
                {
                    trackBarLaserCurrentLimit.Value -= 1;
                }
                else
                {
                    MessageBox.Show("Minimum laser current limit reached.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected exceptions that might occur
                Console.WriteLine($"Error in buttonLaserCurrentLimitDecrease_Click: {ex.Message}");
                MessageBox.Show("An error occurred while decreasing the laser current limit. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Handles the click event for the "TEC Output" button. 
        /// This toggles the state of the TEC (Thermoelectric Cooler) output, ensuring it can't be turned off while the laser is on.
        /// </summary>
        /// <param name="sender">The source of the event, typically the "TEC Output" button that was clicked.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void buttonTECOut_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if laser output (RB_laserOut) is not empty
                if (!string.IsNullOrEmpty(ldx36000.RB_laserOut))
                {
                    // Check if TEC output is on and laser output is also on (value of 1)
                    if (tedTECOut && int.Parse(ldx36000.RB_laserOut.Split('.')[0]) == 1)
                    {
                        // Prevent turning off TEC if laser is on
                        MessageBox.Show("Cannot turn off TEC when Laser is ON!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // Toggle the TEC output (switch it to the opposite state)
                        ted4015.switchTecOutput(!tedTECOut);
                    }
                }
                else
                {
                    MessageBox.Show("Laser output is not initialized. Please ensure the system is properly connected.",
                                     "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected exceptions and notify the user
                Console.WriteLine($"Error in buttonTECOut_Click: {ex.Message}");
                MessageBox.Show("An error occurred while toggling TEC output. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Toggles the laser fan state between ON and OFF when the button is clicked.
        /// Sends a command to the Arduino to either turn the fan on or off based on its current state.
        /// </summary>
        /// <param name="sender">The source of the event (the "Laser Fan" button).</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void buttonLasFan_Click(object sender, EventArgs e)
        {
            try
            {
                // Parse the current state of the laser fan (1 = ON, 0 = OFF)
                if (int.Parse(arduinoGamma.arduinoRelayFansLaser.Split('.')[0]) == 1)
                {
                    // If the fan is currently ON, turn it off and update the button text
                    arduinoGamma.QueueCommand("LF 0");
                    buttonLasFan.Text = "Turn Fan On";
                }
                else
                {
                    // If the fan is currently OFF, turn it on and update the button text
                    arduinoGamma.QueueCommand("LF 1");
                    buttonLasFan.Text = "Turn Fan Off";
                }
            }
            catch (Exception ex)
            {
                // Log the error and inform the user if something goes wrong
                Console.WriteLine($"Error in buttonLasFan_Click: {ex.Message}");
                MessageBox.Show("An error occurred while toggling the laser fan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Toggles the pump state between ON and OFF when the button is clicked.
        /// Sends a command to the Arduino to either enable or disable the pump based on its current state.
        /// </summary>
        /// <param name="sender">The source of the event (the button responsible for toggling the pump).</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void buttonPeltFan_Click(object sender, EventArgs e)
        {
            try
            {
                // Parse the current state of the pump (1 = ON, 0 = OFF)
                if (int.Parse(arduinoGamma.arduinoPumpEnabled.Split('.')[0]) == 1)
                {
                    // If the pump is currently ON, disable it by sending "Pu 0"
                    arduinoGamma.QueueCommand("Pu 0");
                }
                else
                {
                    // If the pump is currently OFF, enable it by sending "Pu 1"
                    arduinoGamma.QueueCommand("Pu 1");
                }
            }
            catch (Exception ex)
            {
                // Log the error and inform the user if something goes wrong
                Console.WriteLine($"Error in buttonPeltFan_Click: {ex.Message}");
                MessageBox.Show("An error occurred while toggling the pump.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Handles the MouseHover event for the formsPlotArduino control.
        /// This method could be used to display additional information or update visuals when the mouse hovers over the plot.
        /// Currently, it's a placeholder for potential future functionality.
        /// </summary>
        /// <param name="sender">The source of the event (formsPlotArduino control).</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void formsPlotArduino_MouseHover(object sender, EventArgs e)
        {
            // Future functionality for mouse hover can be implemented here
            // For example, showing a tooltip or updating the plot
        }


        /// <summary>
        /// Handles the MouseLeave event for the formsPlotArduino control.
        /// This method sets the `plotArduinoLive` flag to true when the mouse leaves the plot area.
        /// </summary>
        /// <param name="sender">The source of the event (formsPlotArduino control).</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void formsPlotArduino_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                // Set the plotArduinoLive flag to true when the mouse leaves the plot area
                plotArduinoLive = true;
            }
            catch (Exception ex)
            {
                // Log the error and inform the user if something goes wrong
                Console.WriteLine($"Error in formsPlotArduino_MouseLeave: {ex.Message}");
                MessageBox.Show("An error occurred while handling the mouse leave event.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the MouseEnter event for the formsPlotArduino control.
        /// This method sets the `plotArduinoLive` flag to false when the mouse enters the plot area.
        /// </summary>
        /// <param name="sender">The source of the event (formsPlotArduino control).</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void formsPlotArduino_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                // Set the plotArduinoLive flag to false when the mouse enters the plot area
                plotArduinoLive = false;
            }
            catch (Exception ex)
            {
                // Log the error and inform the user if something goes wrong
                Console.WriteLine($"Error in formsPlotArduino_MouseEnter: {ex.Message}");
                MessageBox.Show("An error occurred while handling the mouse enter event.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Sends the "Ku" command to the Arduino with the value from the textBoxKu control.
        /// This method is invoked when the "Ku" button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event (the "Ku" button).</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void buttonKu_Click(object sender, EventArgs e)
        {
            try
            {
                // Send the "Ku" command to Arduino with the value entered in the textBoxKu
                arduinoGamma.QueueCommand("Ku " + textBoxKu.Text);
            }
            catch (Exception ex)
            {
                // Log the error and inform the user if something goes wrong
                Console.WriteLine($"Error in buttonKu_Click: {ex.Message}");
                MessageBox.Show("An error occurred while sending the Ku command to the Arduino.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Handles the Paint event for the panelLaser control.
        /// This method is currently empty, but can be used for custom drawing on the panel.
        /// </summary>
        /// <param name="sender">The source of the event (panelLaser control).</param>
        /// <param name="e">A PaintEventArgs object containing event data for custom drawing.</param>
        private void panelLaser_Paint(object sender, PaintEventArgs e)
        {
            // Currently empty - you can add custom drawing code here in the future.
            // For example: e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, 100, 100));
        }


        /// <summary>
        /// Increases the laser current setpoint value by 1 when the "Increase" button is clicked.
        /// This method updates the trackBarLaserCurrentSetpoint value accordingly.
        /// </summary>
        /// <param name="sender">The source of the event (the "Increase" button).</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void buttonLaserCurrentSetpointIncrease_Click(object sender, EventArgs e)
        {
            try
            {
                // Increment the laser current setpoint by 1
                trackBarLaserCurrentSetpoint.Value += 1;
            }
            catch (Exception ex)
            {
                // Log the error and inform the user if something goes wrong
                Console.WriteLine($"Error in buttonLaserCurrentSetpointIncrease_Click: {ex.Message}");
                MessageBox.Show("An error occurred while increasing the laser current setpoint.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Decreases the laser current setpoint value by 1 when the "Decrease" button is clicked.
        /// This method updates the trackBarLaserCurrentSetpoint value accordingly.
        /// </summary>
        /// <param name="sender">The source of the event (the "Decrease" button).</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void buttonLaserCurrentSetpointDecrease_Click(object sender, EventArgs e)
        {
            try
            {
                // Decrement the laser current setpoint by 1
                trackBarLaserCurrentSetpoint.Value -= 1;
            }
            catch (Exception ex)
            {
                // Log the error and inform the user if something goes wrong
                Console.WriteLine($"Error in buttonLaserCurrentSetpointDecrease_Click: {ex.Message}");
                MessageBox.Show("An error occurred while decreasing the laser current setpoint.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Sends the temperature setpoint to the Arduino when the button is clicked.
        /// The method sends the temperature value from the trackBarTempSetpoint to the Arduino via a command.
        /// </summary>
        /// <param name="sender">The source of the event (the "Temp Setpoint" button).</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void buttonTempSetpoint_Click(object sender, EventArgs e)
        {
            try
            {
                // Send the temperature setpoint to the Arduino using the value from the trackBarTempSetpoint
                arduinoGamma.QueueCommand("S0 " + trackBarTempSetpoint.Value);
            }
            catch (Exception ex)
            {
                // Log the error and inform the user if something goes wrong
                Console.WriteLine($"Error in buttonTempSetpoint_Click: {ex.Message}");
                MessageBox.Show("An error occurred while sending the temperature setpoint to the Arduino.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Creates a thermal image color palette based on gnuplot's PM3D palette.
        /// This palette is used for thermal imaging and generates colors that map to temperature values.
        /// The color palette is calculated for each of the 256 color values (from 0 to 255).
        /// The palette uses a combination of red, green, and blue components, with special handling for 
        /// blue values based on a sine function.
        /// </summary>
        void CreateThermalImageColorPalette()
        {
            try
            {
                // Iterate over all 256 values to generate corresponding RGB components
                for (int x = 0; x < 256; x++)
                {
                    // Calculate the red component based on the square root function
                    paletteR[x] = Convert.ToByte(255 * Math.Sqrt(x / 255.0));

                    // Calculate the green component using a cubic power function
                    paletteG[x] = Convert.ToByte(255 * Math.Pow(x / 255.0, 3));

                    // Calculate the blue component using a sine function
                    if (Math.Sin(2 * Math.PI * (x / 255.0)) >= 0)
                    {
                        // If sine is positive, use the sine value scaled to 255
                        paletteB[x] = Convert.ToByte(255 * Math.Sin(2 * Math.PI * (x / 255.0)));
                    }
                    else
                    {
                        // If sine is negative, set blue to 0
                        paletteB[x] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any error that occurs during the palette creation process
                Console.WriteLine($"Error in CreateThermalImageColorPalette: {ex.Message}");
                MessageBox.Show("An error occurred while creating the thermal image color palette.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Event handler for the FLIR thermal image update on a timer tick. 
        /// This method refreshes the thermal image, applies optional mirroring, 
        /// and updates various temperature statistics on the UI.
        /// </summary>
        private void timerFLIR_Tick(object sender, EventArgs e)
        {
            try
            {
                // Retrieve and display the updated thermal image.
                pictureBoxFLIR.Image = ThermalImagePaint();

                // Apply mirroring if the checkbox is checked.
                if (checkBoxMirror.Checked)
                {
                    pictureBoxFLIR.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }

                // Scale the picture box size based on WIDTH and SCALE constants.
                pictureBoxFLIR.Size = new Size(WIDTH * SCALE, HEIGHT * SCALE);

                // Fetch the thermal image statistics.
                ti.GetStatistics(out int[] spotmeterStatistics,
                                 out int[] temperatures,
                                 out byte resolution,
                                 out byte ffcStatus,
                                 out bool[] temperatureWarning);

                // Display the mean, maximum, and minimum temperatures, adjusting from Kelvin to Celsius.
                labelFLIRTemp.Text = $"Mean : {((spotmeterStatistics[0] / 10.0) - 273.15):0.00} C";
                labelFLIRTempMax.Text = $"Max  : {((spotmeterStatistics[1] / 10.0) - 273.15):0.00} C";
                labelFLIRTempMin.Text = $"Min  : {((spotmeterStatistics[2] / 10.0) - 273.15):0.00} C";

                // Uncomment the following lines if spotmeter configuration details are needed:
                // byte[] spotmetercfg = ti.GetSpotmeterConfig();
                // Console.WriteLine($"{spotmetercfg[0]} {spotmetercfg[1]} {spotmetercfg[2]} {spotmetercfg[3]}");
            }
            catch (Exception ex)
            {
                // Log the error if any exception occurs during the update.
                Console.WriteLine($"Error in timerFLIR_Tick: {ex.Message}");
                MessageBox.Show("An error occurred while updating the FLIR data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Event handler for the click event on the FLIR image displayed in the picture box.
        /// This method calculates the clicked point's coordinates, adjusts them for any mirroring,
        /// and defines a region of interest based on the mouse button clicked.
        /// </summary>
        private void pictureBoxFLIR_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the mouse click event arguments.
                MouseEventArgs me = (MouseEventArgs)e;
                Point coordinates = me.Location;

                // Adjust the clicked coordinates based on the scaling factor.
                int xcoord = coordinates.X / SCALE;
                int ycoord = coordinates.Y / SCALE;

                // If the mirror checkbox is checked, flip the X-coordinate.
                if (checkBoxMirror.Checked)
                {
                    xcoord = WIDTH - xcoord;
                }

                // Determine the region of interest based on the mouse button clicked.
                switch (me.Button)
                {
                    case MouseButtons.Left:
                        // For left click, define a custom region around the click location.
                        regionOfInterest[0] = (byte)(xcoord - 1);
                        regionOfInterest[1] = (byte)(ycoord - 1);
                        regionOfInterest[2] = (byte)xcoord;
                        regionOfInterest[3] = (byte)ycoord;
                        break;

                    case MouseButtons.Right:
                        // For right click, reset the region of interest to default values.
                        regionOfInterest[0] = 0;
                        regionOfInterest[1] = 0;
                        regionOfInterest[2] = 79;  // Assuming a predefined default region width.
                        regionOfInterest[3] = 59;  // Assuming a predefined default region height.
                        break;
                }

                // Define the clip limit to control the contrast enhancement level.
                int[] cliplim = new int[2];
                cliplim[0] = 4800;  // Example: max temperature for contrast enhancement.
                cliplim[1] = 512;   // Example: minimum temperature for contrast enhancement.

                // Set the spotmeter configuration based on the updated region of interest.
                ti.SetSpotmeterConfig(regionOfInterest);

                // Optional: Set high contrast configuration if needed (currently commented out).
                // ti.SetHighContrastConfig(regionOfInterest, 64, cliplim, 2);
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the click event handling.
                Console.WriteLine($"Error in pictureBoxFLIR_Click: {ex.Message}");
                MessageBox.Show("An error occurred while processing the FLIR image click.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event handler for the Save button click, which saves the current FLIR image
        /// displayed in the pictureBoxFLIR to a PNG file with a timestamped filename.
        /// </summary>
        private void buttonSaveFLIRImage_Click(object sender, EventArgs e)
        {
            try
            {
                // Define the file path with a timestamp for uniqueness.
                string filePath = @"C:\Users\Gamma_MRI\Desktop\GammaAshData\FLIR" + DateTime.Now.ToString("dd-MMMM-yyyy-HH-mm") + ".png";

                // Save the current image displayed in pictureBoxFLIR to the specified path.
                pictureBoxFLIR.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                // Optional: Provide feedback to the user that the image has been saved successfully.
                MessageBox.Show("FLIR image saved successfully!", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the saving process.
                Console.WriteLine($"Error saving FLIR image: {ex.Message}");

                // Provide feedback to the user in case of an error.
                MessageBox.Show("An error occurred while saving the FLIR image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Event handler for the ComboBox selection change. Updates the laser mode based on the selected index.
        /// </summary>
        private void comboBoxLaserMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check which item is selected in the comboBox and send the corresponding command to the laser
            switch (comboBoxLaserMode.SelectedIndex)
            {
                case 0:
                    ldx36000.QueueCommand("LAS:MODE:CW"); // Continuous Wave mode
                    break;
                case 1:
                    ldx36000.QueueCommand("LAS:MODE:HPULSE"); // High Pulse mode
                    break;
                case 2:
                    ldx36000.QueueCommand("LAS:MODE:PULSE"); // Pulse mode
                    break;
                case 3:
                    ldx36000.QueueCommand("LAS:MODE:TRIG"); // Trigger mode
                    break;
                default:
                    // Optional: Add a case for invalid selection (e.g., show a message box)
                    break;
            }
        }

        /// <summary>
        /// Event handler for the button click. Sends the DC value to the laser using the value from the textBox.
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            // Check if the input in textBoxLASDC is valid before sending the command
            if (double.TryParse(textBoxLASDC.Text, out double dcValue))
            {
                ldx36000.QueueCommand("LAS:DC " + dcValue);
            }
            else
            {
                // Handle invalid input (optional)
                MessageBox.Show("Invalid DC value. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event handler for the PWP button click. Sends the PWP value to the laser using the value from the textBox.
        /// </summary>
        private void buttonLASPWP_Click(object sender, EventArgs e)
        {
            // Check if the input in textBoxLASPWP is valid before sending the command
            if (double.TryParse(textBoxLASPWP.Text, out double pwpValue))
            {
                ldx36000.QueueCommand("LAS:PWP " + pwpValue);
            }
            else
            {
                // Handle invalid input (optional)
                MessageBox.Show("Invalid PWP value. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event handler for the LAS Frequency button click. Sends the LAS Frequency value to the laser using the value from the textBox.
        /// </summary>
        private void buttonLASF_Click(object sender, EventArgs e)
        {
            // Check if the input in textBoxLASF is valid before sending the command
            if (double.TryParse(textBoxLASF.Text, out double lasfValue))
            {
                ldx36000.QueueCommand("LAS:F " + lasfValue);
            }
            else
            {
                // Handle invalid input (optional)
                MessageBox.Show("Invalid LAS Frequency value. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event handler for the laser sequence timer tick. Handles turning the laser on/off based on the configured loop settings.
        /// </summary>
        private void timerLaserSeq_Tick(object sender, EventArgs e)
        {
            // Check if the laser output is ON
            if (ldx36000.RB_laserOut == "1")
            {
                // Set the timer interval to the OFF time specified in the text box
                timerLaserSeq.Interval = 1000 * Int32.Parse(textBoxLasOFFTime.Text);

                // Increment the loop count for the laser sequence
                laserLoopCount++;

                // If the number of loops is greater than zero, check if we've reached the maximum loop count
                if (Int32.Parse(textBoxLasONOFFLoops.Text) > 0)
                {
                    if (laserLoopCount > Int32.Parse(textBoxLasONOFFLoops.Text) - 1)
                    {
                        // Disable the timer and uncheck the sequence checkbox if we've reached the max loops
                        timerLaserSeq.Enabled = false;
                        checkBoxLasSeq.Checked = false;
                    }
                }
            }
            else
            {
                // If laser is off, set the interval to the ON time
                timerLaserSeq.Interval = 1000 * Int32.Parse(textBoxLasONTime.Text);
            }

            // Trigger the button for laser output
            buttonLaserOut.PerformClick();
        }

        /// <summary>
        /// Event handler for the PWF button click. Sends the Power Level to the laser using the value from the text box.
        /// </summary>
        private void buttonLASPWF_Click(object sender, EventArgs e)
        {
            // Send the PWF command to the laser with the specified value from the text box
            ldx36000.QueueCommand("LAS:PWF " + textBoxLASPWF.Text);
        }

        /// <summary>
        /// Event handler for the checkbox changed event. Enables/disables the laser sequence timer based on the checkbox state.
        /// </summary>
        private void checkBoxLasSeq_CheckedChanged(object sender, EventArgs e)
        {
            // If the checkbox is checked, start the laser sequence
            if (checkBoxLasSeq.Checked)
            {
                // Reset the laser loop count
                laserLoopCount = 0;

                // Check if the laser is currently on
                if (ldx36000.RB_laserOut == "1")
                {
                    // Set the timer interval to the ON time
                    timerLaserSeq.Interval = 1000 * Int32.Parse(textBoxLasONTime.Text);
                }
                else
                {
                    // Set a default interval when laser is off and switch to CW mode
                    timerLaserSeq.Interval = 5000;
                    ldx36000.QueueCommand("LAS:MODE:CW");
                    comboBoxLaserMode.SelectedIndex = 0; // Set CW mode in combo box
                }

                // Enable the laser sequence timer
                timerLaserSeq.Enabled = true;
            }
            else
            {
                // Disable the laser sequence timer if the checkbox is unchecked
                timerLaserSeq.Enabled = false;
            }
        }

        /// <summary>
        /// Event handler for the ThingSpeak timer tick. Sends data to ThingSpeak including laser parameters and temperatures.
        /// </summary>
        private void timerThingSpeak_Tick(object sender, EventArgs e)
        {
            // Set the ThingSpeak timer interval to 60 seconds (60000 ms)
            timerThingSpeak.Interval = 60000;

            // Determine if the laser is ON or OFF
            int lasON = (labelLaserOut.Text == "Laser Output ON") ? 1 : 0;

            // Send laser data to ThingSpeak (including laser temperature, current, power, etc.)
            sendDataTS(
                "field1=" + ldx36000.RB_laserTemperature +
                "&field2=" + lasON +
                "&field3=" + ldx36000.RB_laserCurrentPhotoDiode +
                "&field4=" + ldx36000.RB_laserPower +
                "&field5=" + arduinoGamma.arduinoTC0 +
                "&field6=" + tedTemp.ToString("0.##")
            );
        }


        /// <summary>
        /// Paints the thermal image based on the current image data and the defined color palette.
        /// </summary>
        /// <returns>A Bitmap object representing the thermal image with the applied palette.</returns>
        private Bitmap ThermalImagePaint()
        {
            // Create a new bitmap with the specified dimensions
            Bitmap bitmap = new Bitmap(WIDTH, HEIGHT);

            // Iterate over each pixel of the thermal image and apply the color palette
            for (int row = 0; row < WIDTH; row++)
            {
                for (int col = 0; col < HEIGHT; col++)
                {
                    // Get the color value from imageData using the current row and column
                    int color = imageData[row + col * WIDTH];

                    // Set the pixel color based on the calculated palette values
                    bitmap.SetPixel(row, col, Color.FromArgb(paletteR[color], paletteG[color], paletteB[color]));
                }
            }

            // Highlight the region of interest by drawing white pixels at the defined boundary
            bitmap.SetPixel(regionOfInterest[0], regionOfInterest[1], Color.FromArgb(255, 255, 255));
            bitmap.SetPixel(regionOfInterest[2], regionOfInterest[1], Color.FromArgb(255, 255, 255));
            bitmap.SetPixel(regionOfInterest[0], regionOfInterest[3], Color.FromArgb(255, 255, 255));
            bitmap.SetPixel(regionOfInterest[2], regionOfInterest[3], Color.FromArgb(255, 255, 255));

            // Scale the image to a larger size based on the defined SCALE constant
            bitmap = new Bitmap(bitmap, new Size(WIDTH * SCALE, HEIGHT * SCALE));

            // Return the final bitmap
            return bitmap;
        }

        /// <summary>
        /// Callback method to receive and store the high contrast image data from the thermal imaging device.
        /// </summary>
        /// <param name="sender">The sender of the callback (BrickletThermalImaging device).</param>
        /// <param name="image">The thermal image data received from the device.</param>
        void TICallback(BrickletThermalImaging sender, byte[] image)
        {
            // Store the received image data for future rendering
            imageData = image;
        }

        /// <summary>
        /// Initializes the FLIR (thermal imaging) device and sets up the required configuration.
        /// </summary>
        void FLIR_Load()
        {
            // Set the region of interest (ROI) for thermal image processing
            regionOfInterest[0] = (byte)39;
            regionOfInterest[1] = (byte)29;
            regionOfInterest[2] = (byte)40;
            regionOfInterest[3] = (byte)30;

            // Initialize the IP connection and the thermal imaging device
            ipcon = new IPConnection(); // Create IP connection
            ti = new BrickletThermalImaging(UID, ipcon); // Create device object

            // Connect to the brickd service using the specified host and port
            ipcon.Connect(HOST, PORT);

            // Ensure device is connected before interacting with it
            ti.SetImageTransferConfig(BrickletThermalImaging.IMAGE_TRANSFER_CALLBACK_HIGH_CONTRAST_IMAGE);

            // Set thermal image resolution to 0-6553 Kelvin range
            ti.SetResolution(BrickletThermalImaging.RESOLUTION_0_TO_6553_KELVIN);

            // Generate the color palette for thermal image visualization
            CreateThermalImageColorPalette();

            // Subscribe to the high contrast image callback event
            ti.HighContrastImageCallback += TICallback;

            // Set up the FLIR image update interval
            timerFLIR.Interval = 200;
            timerFLIR.Enabled = true;

            // Configure the PictureBox to stretch the image to fit its size
            pictureBoxFLIR.SizeMode = PictureBoxSizeMode.StretchImage;
        }


    }
}
