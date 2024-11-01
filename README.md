# GammaGUI
1.	Introduction
A software package has been developed to enable precise and automated field mapping for the GAMMA-MRI prototype. This software allows users to specify the required measurements, save and load data, manually position the probe, monitor live readings, and conduct both spatial measurements and fixed-position time-based measurements.

2.	FieldMap-GUI

Magnetic field measurements are performed using a Teslameter (Model F71) and a Hall Probe (FP Series 3-axis), both produced by LakeShore Cryogenics Corp. The positioning of the Hall Probe is controlled by an XYZ translational stage, consisting of three MTS50 DC motorized translation stages managed by KCubes from ThorLabs. This configuration enables precise measurement within a cubic volume of 50 mm, achieving an accuracy of 1.6 µm along any axis, and a field measurement accuracy of ±0.2% of the reading.
The system is mounted on the GAMMA prototype using a custom 3D-printed fixture, with the probe positioned at (25 mm, 25 mm, 25 mm), corresponding to the geometric center of the magnet. This arrangement facilitates measurements at distances of 25 mm in all directions from the center.
 
The software, developed in C# utilizing the .NET framework, is distributed as a standalone binary executable (.exe) file. Special attention has been given to threading, memory management, and code optimization to ensure stable measurements are obtained in the shortest time possible. This design minimizes the risk of data loss, mitigates potential system slowdowns affecting software or PC performance, and facilitates rapid data collection to reduce the influence of external factors on the measurements.

2.1.	Teslameter

The F71 Teslameter can be interfaced with custom software via an RS232 serial interface, connected through USB to the control PC. The software employs a custom-developed C# class that transmits SCPI (Standard Commands for Programmable Instruments) command strings as text and processes the corresponding responses.
This custom class allows for control over various parameters of the Teslameter, including averaging windows, temperature compensation, filtering modes, and measurement modes. To minimize the risk of incorrect configuration, most functions are concealed within the graphical user interface (GUI). Additionally, the software provides live monitoring of field measurements and probe temperature.
2.2.	3-Axis translational stage

The three-axis stage, constructed from components provided by ThorLabs, features an interface compatible with custom software through a Driver Library supplied by the manufacturer. This software enables control of the DC servo motors, allowing users to move to specific coordinates, perform homing functions, and set maximum speeds.
Attached to the stage is an LS-FP-NS180-ZS30M Hall probe, which is mounted using a 3D-printed adapter. The low-voltage DC servos and their positioning away from the tip of the sensor ensure minimal distortion of measurements resulting from the movement of the stage itself.
3.	GUI
 
The graphical user interface (GUI) is a comprehensive software platform that facilitates the configuration of multiple modes of operation, customization of measurement parameters, and the ability to load and save settings and data. It also includes robust error handling and the option to display selectable live 3D and 2D plots. The plots were created using code available from https://www.codeproject.com/Articles/5294893/A-Csharp-3D-Surface-Plot-Control and https://scottplot.net/
All electronics automatically connect on startup requiring no selection of COM ports or any configuration of the software.
Three operational modes are available, as detailed below.
3.1.	Cube Measurement Mode

In this mode, the user can specify the dimensions of the cube to be measured and the number of steps for each axis. For example, a user may define a cube with a side length of 50 mm and set 51 steps in all three axes, resulting in a resolution of 1 mm.
 
The Teslameter allows the user to select an averaging time before initiating a measurement run. Each measurement can be recorded directly by the software or averaged over multiple averaging windows, enabling the calculation of the standard deviation for each measured point. For instance, an averaging window of 200 ms can be selected with five measurements averaged, including standard deviation. Alternatively, a 1000 ms averaging window can be chosen without averaging to obtain the field measurement without standard deviation.
A "Stop for Measurement" option is also available, which enhances measurement speed when disabled, as the probe will not pause during movement. Conversely, enabling this option allows for more accurate readings. For example, if the probe takes one second to move from one position to another while the averaging window is also one second, the measurement will not accurately reflect field measured at the desired point but rather over the path taken.
Additionally, a dwell time option is included, which allows for a brief pause before beginning measurements at each point. This feature minimizes potential noise from vibrations of the Hall probe during movement.
The standard measurement setup is configured as a 50 mm cube with a resolution of 1 mm in all directions, resulting in a grid of 51 x 51 x 51 data points and a total of 132,651 data points.
 
To facilitate data visualization, each data point is displayed in a table and represented in an interactive 3D heatmap that continuously updates in real time. During the measurement run, each Z-slice can be viewed independently, or the slices can be stacked to visualize all Z-slice data simultaneously. The plots are selectable, allowing users to view temperature, field magnitude, and the x, y, and z components, as well as standard deviations, even during the measurement run.
 
When a run is initiated, the user is prompted to choose a save file location. Data is written to this file in real time, ensuring that data is preserved in the event of a system crash or other disruptions. The data is written as a csv file.
During movement over each slice the head of the hall probe follows a zig zagging pattern in order to reduce time waiting for motor movements. For example, when moving in sequence, from (0,0,0) the x value will increment by +1 for each measurement. When the upper x limit is reached, the y value will increase by 1, and the x value, will now decrease by one each movement. The path of a cube of 2 mm would be as illustrated below:
 

Csv files!
3.2.	Time Measurement Mode

The Time Measurement mode allows the user to select a fixed point and conduct continuous measurements for a specified duration. This mode is particularly useful for assessing the temperature dependence of measurements or evaluating the stability of a magnetic field.
 
Similar to Cube mode, users can modify the averaging window and averaging count as needed. The data is displayed in a table format, with each new measurement added sequentially, accompanied by a live, interactive 2D plot for real-time visualization.
 
Upon initiating a run, the user is prompted to choose a save file location. This file is updated in real time, ensuring that data is preserved in the event of a system crash or other unforeseen disruptions. The data is written as a csv file.
3.3.	Manual Mode

A manual mode is also available, allowing users to select a specific position and move the probe to that location. Users can set the averaging window and read back the values displayed in the readback window. This mode facilitates precise measurements during setup and enables users to verify the proper functioning of the system before initiating potentially long-duration measurements.
  



