/* Motor Pins */
// Motor: {X-Axis, Y-Axis, Z-Axis}
const int PIN_MOTOR_PUL[3] = { 2, 4, 6 };
const int PIN_MOTOR_DIR[3] = { 3, 5, 7 };
const int PIN_JOYSTICK_AXIS[3] = { A5, A6, A7};
const int PIN_JOYSTICK_BT = 12;

const int PIN_PROXIMAL_LIM[3] = { 8, 11, 14 };
const int PIN_REMOTE_LIM[3] = { 9, 17, 15 };
const int PIN_ORIGIN[3] = { 10, 18, 16 };

long motorPosition[3] = { 0, 0, 0 };
long motorPositionLimit[3] = { 18000, 18000, 18000 };
const short velocity = 250;
short measuredLimit = 0;
int serialBufferSize;
const int MAXLENGTH = 256; 
byte serialBuffer[MAXLENGTH];

const int FwVersion = 1;

// 0x00 -> Command Mode
// 0x01 -> Joystick/Freerun Mode
uint8_t mode = 0x00;

void setup() {
  // put your setup code here, to run once:    
  for (int i = 0; i < 3; i++)
  {
    pinMode(PIN_MOTOR_PUL[i], OUTPUT);
    pinMode(PIN_MOTOR_DIR[i], OUTPUT);

    pinMode(PIN_REMOTE_LIM[i], INPUT_PULLUP);
    pinMode(PIN_PROXIMAL_LIM[i], INPUT_PULLUP);
    pinMode(PIN_ORIGIN[i], INPUT_PULLUP);

    digitalWrite(PIN_MOTOR_DIR[i], HIGH);
  }
  pinMode(LED_BUILTIN, OUTPUT); 
  pinMode(PIN_JOYSTICK_BT, INPUT_PULLUP);

  Serial.begin(115200);
  Serial.setTimeout(200);
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
