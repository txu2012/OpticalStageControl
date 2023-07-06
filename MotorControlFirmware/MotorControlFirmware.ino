/* Motor Pins */
// Motor: {X-Axis, Y-Axis, Z-Axis}
const int PIN_MOTOR_PUL[3] = { 2, 4, 6 };
const int PIN_MOTOR_DIR[3] = { 3, 5, 7 };

short motorPosition[3] = { 500, 50, 10 };
short motorPositionLimit[3] = { 1000, 1000, 1000 };
short velocity = 25;

int serialBufferSize;
const int MAXLENGTH = 256; 
byte serialBuffer[MAXLENGTH];

const int FwVersion = 1;

// 0x00 -> Command Mode
// 0x01 -> Joystick/Freerun Mode
uint8_t mode = 0x00;

void setup() {
  // put your setup code here, to run once:  
  Serial.begin(115200);
  Serial.setTimeout(200);
  
  for (int i = 0; i < 3; i++)
  {
    pinMode(PIN_MOTOR_PUL[i], OUTPUT);
    pinMode(PIN_MOTOR_DIR[i], OUTPUT);

    digitalWrite(PIN_MOTOR_DIR[i], HIGH);

    //motorPosition[i] = 500;
    //motorHomeStatus[i] = 0x00;
  }
  pinMode(LED_BUILTIN, OUTPUT); 
}

void loop() {
  // put your main code here, to run repeatedly:
  memset(serialBuffer, 0, sizeof(serialBuffer));
  if (Serial.available() > 0)
  {
    ReadCommand();
  }

  if (mode == 0x01)
  {
    FreerunMoveMotor(velocity);
  }
}

void TestBlink()
{
  digitalWrite(LED_BUILTIN, HIGH);  // turn the LED on (HIGH is the voltage level)
  delay(500);                      // wait for a second
  digitalWrite(LED_BUILTIN, LOW);   // turn the LED off by making the voltage LOW
  delay(500);  
}
