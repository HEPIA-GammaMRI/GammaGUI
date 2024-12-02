#include <PID_v1.h>
#include <EEPROM.h>

// ===== Summary =====
// This Arduino code controls a system with a Peltier device, fans, pumps, and temperature sensors.
// It uses a PID controller to regulate the temperature, reads commands via Serial, and outputs system status.

// === Global Variables ===
// PID Controller and Tuning Parameters
double setpointTC0;          // Target temperature (setpoint)
double arduinoPeltierValue;  // PID output for Peltier control
double consKp = 1, consKi = 3, consKd = 0.2;  // PID tuning parameters

// Temperature Sensor Variables
const int numReadings = 100;  // Moving average size
float arduinoTC0Value[numReadings];  // Circular buffer for TC0 readings
float arduinoTC1Value[numReadings];  // Circular buffer for TC1 readings
int readIndex = 0;                   // Index for current reading
float TC0total = 0, TC1total = 0;    // Totals for averaging
double TC0average = 0, TC1average = 0;  // Averaged temperatures

// Output and Input Pins
const int pinPelt = 5, pinSpare1 = 3, pinSpare2 = 9, pinSpare3 = 10, pinSpare4 = 11;
const int pinPeltFan = 2, pinPump = 7, pinLasLight = 12, pinLasFan = 8;
const int TC0Pin = A0, TC1Pin = A1, pinINTLK = 4;

// System State Variables
int arduinoPeltierFanValue = 0;
int arduinoPumpEnabled = 0;
int arduinoLasFanOn = 0;
int arduinoLasLightOn = 0;
int INTLK = 0;

// Serial Communication Variables
String input = "";          // Buffered input string
String output = "";         // Response output
const int numberOfsplitStrings = 20;
String splitStrings[numberOfsplitStrings];

// PID Instance
PID myPID(&TC0average, &arduinoPeltierValue, &setpointTC0, consKp, consKi, consKd, REVERSE);

// Helper Union for EEPROM
union {
    double dval;
    byte bval[4];
} doubleAsBytes;

// === Setup Function ===
void setup() {
    // Initialize Serial Communication
    Serial.begin(115200);

    // Initialize Pins
    setupPins();

    // Initialize PID Controller
    myPID.SetMode(AUTOMATIC);
    myPID.SetTunings(consKp, consKi, consKd);

    // Initialize EEPROM Stored Setpoint
    loadSetpointFromEEPROM();

    // Initialize Temperature Buffers
    initializeTemperatureBuffers();

    // Set Initial Outputs
    updateOutputs();
}

// === Loop Function ===
void loop() {
    // Read and Process Serial Commands
    handleSerialInput();

    // Update Sensor Readings and PID Computation
    updateSensorReadings();
    myPID.Compute();

    // Control Outputs Based on System State
    controlOutputs();

    // Check Interlock State
    INTLK = digitalRead(pinINTLK);
}

// === Helper Functions ===

// Pin Initialization
void setupPins() {
    pinMode(pinPelt, OUTPUT);
    pinMode(pinSpare1, OUTPUT);
    pinMode(pinSpare2, OUTPUT);
    pinMode(pinSpare3, OUTPUT);
    pinMode(pinSpare4, OUTPUT);
    pinMode(pinPeltFan, OUTPUT);
    pinMode(pinPump, OUTPUT);
    pinMode(pinLasLight, OUTPUT);
    pinMode(pinLasFan, OUTPUT);
    pinMode(pinINTLK, INPUT);
}

// Load Setpoint from EEPROM
void loadSetpointFromEEPROM() {
    for (int i = 0; i < 4; i++) {
        doubleAsBytes.bval[i] = EEPROM.read(i);
    }
    setpointTC0 = doubleAsBytes.dval;
}

// Initialize Temperature Buffers
void initializeTemperatureBuffers() {
    for (int i = 0; i < numReadings; i++) {
        arduinoTC0Value[i] = 0;
        arduinoTC1Value[i] = 0;
    }
}

// Update Sensor Readings and Averages
void updateSensorReadings() {
    TC0total -= arduinoTC0Value[readIndex];
    TC1total -= arduinoTC1Value[readIndex];

    arduinoTC0Value[readIndex] = readTemperature(TC0Pin);
    arduinoTC1Value[readIndex] = readTemperature(TC1Pin);

    TC0total += arduinoTC0Value[readIndex];
    TC1total += arduinoTC1Value[readIndex];

    readIndex = (readIndex + 1) % numReadings;  // Circular buffer index

    TC0average = TC0total / numReadings;
    TC1average = TC1total / numReadings;
}

// Read Temperature from Sensor
float readTemperature(int pin) {
    return ((analogRead(pin) * (5.0 / 1023.0)) - 1.25) / 0.005;
}

// Handle Serial Input
void handleSerialInput() {
    while (Serial.available() > 0) {
        char ch = Serial.read();
        if (ch == '\n') {
            processSerialCommand();
            input = "";
        } else {
            input += ch;
        }
    }
}

// Process Serial Command
void processSerialCommand() {
    splitString(input, ';', splitStrings, numberOfsplitStrings);
    output = "";

    for (int i = 0; i < numberOfsplitStrings; i++) {
        handleCommand(splitStrings[i]);
    }

    if (!output.isEmpty()) {
        Serial.println(output);
    }
}

// Handle Individual Commands
void handleCommand(String command) {
    if (command.startsWith("FWRev?")) output += "FWRev arduinoGAMMA1.0;";
    else if (command.startsWith("Pe?")) output += "Pe " + String(arduinoPeltierValue) + ";";
    // Additional commands handled similarly...
}

// Split String Helper
void splitString(String data, char delimiter, String output[], int maxParts) {
    int index = 0, start = 0, end = 0;
    while (index < maxParts && (end = data.indexOf(delimiter, start)) != -1) {
        output[index++] = data.substring(start, end);
        start = end + 1;
    }
    if (index < maxParts) output[index++] = data.substring(start);
}

// Control Outputs
void controlOutputs() {
    analogWrite(pinPelt, arduinoPeltierValue);
    analogWrite(pinPeltFan, arduinoPeltierFanValue);
    digitalWrite(pinPump, arduinoPumpEnabled);
    digitalWrite(pinLasFan, arduinoLasFanOn);
    digitalWrite(pinLasLight, arduinoLasLightOn);
}

// Update Outputs (Initial)
void updateOutputs() {
    arduinoPeltierValue = 0;
    arduinoPeltierFanValue = 0;
    arduinoPumpEnabled = 0;
    arduinoLasFanOn = 0;
    arduinoLasLightOn = 0;
    controlOutputs();
}
