unsigned long CalcPulseDur(unsigned long pulse_duration_us, unsigned long &time_spent_us, unsigned long &base_time_us)
{  
  if (time_spent_us > 0x40000000) {
      base_time_us = micros();
      time_spent_us = 0;
  }

  time_spent_us += pulse_duration_us;
  unsigned long current_time_us = micros() - base_time_us;

  return ((long)time_spent_us - (long)current_time_us);
}

void ApplyDelay(long delay_us)
{
  if (delay_us/1000 > 0)
  {
    delay(delay_us/1000);
  }
  if (delay_us%1000 > 0)
  {
    delayMicroseconds(delay_us%1000);
  }
}

bool CheckMotorLimit(int motor, short motor_position)
{
  if (motor_position < 0 || motor_position > motorPositionLimit[motor])
    return false;

  return true;
}

uint8_t CalculatePositionLimit()
{
  // Move To Start
  digitalWrite(PIN_MOTOR_DIR[0], LOW);
  MoveToLimit(0, PIN_ORIGIN[0], HIGH, velocity);

  // Move To End and start counting
  digitalWrite(PIN_MOTOR_DIR[0], HIGH);
  measuredLimit = MoveToLimit(0, PIN_REMOTE_LIM[0], LOW, velocity);

  // Move back to start
  digitalWrite(PIN_MOTOR_DIR[0], LOW);
  MoveToLimit(0, PIN_ORIGIN[0], HIGH, velocity);

  return 0;
}

short MoveToLimit(int motor, int limit_sensor, uint8_t polarity, short target_velocity)
{
  short position_num = 0;
  
  unsigned long pulse_duration = (1e6/target_velocity);
  unsigned long base_time_us = micros();
  unsigned long pulse_dur_us = 0;

  // While the sensor has not been activated, increment motor
  while(digitalRead(limit_sensor) != polarity)
  {
    unsigned long pulse_dur_target_us = CalcPulseDur(pulse_duration, pulse_dur_us, base_time_us);
    
    digitalWrite(PIN_MOTOR_PUL[motor], HIGH);
    ApplyDelay(pulse_dur_target_us/2);
    digitalWrite(PIN_MOTOR_PUL[motor], LOW);
    ApplyDelay(pulse_dur_target_us/2);

    position_num = position_num + 1;
  }

  return position_num;
}

uint8_t HomeMotor(int motor, short target_velocity, bool center)
{
  if (center)
  {
    return MoveMotor(motor, motorPositionLimit[motor] / 2, target_velocity);
  }
  else
  {
    digitalWrite(PIN_MOTOR_DIR[motor], LOW);
    MoveToLimit(motor, PIN_ORIGIN[motor], HIGH, velocity);
    motorPosition[motor] = 0;
    return 0;
  }
}

uint8_t MoveMotor(int motor, short motor_position, short target_velocity)
{
  // Check if position, motor index and target velocity are within supported values
  if (motor > 2 ||
      !CheckMotorLimit(motor, motor_position))
  {
    return 0xff;
  }
  
  if (motor_position > motorPosition[motor])
  {
    digitalWrite(PIN_MOTOR_DIR[motor], HIGH);
  }
  else
  {
    digitalWrite(PIN_MOTOR_DIR[motor], LOW);
  }

  // Currently on the optical stage, the proximal limit switch has been giving false readings of being pushed. Using origin sensor
  // in replace of the proximal sensor for now.
  uint8_t lim_pin = (digitalRead(PIN_MOTOR_DIR[motor]) == HIGH) ? PIN_REMOTE_LIM[motor] : PIN_ORIGIN[motor];
  uint8_t polarity = (digitalRead(PIN_MOTOR_DIR[motor]) == HIGH) ? LOW : HIGH;

  // Get amount of steps to move 
  short move_steps = motorPosition[motor] - motor_position;
  unsigned long pulse_duration = (1e6/target_velocity);
  unsigned long base_time_us = micros();
  unsigned long pulse_dur_us = 0;
  
  for (int i = 0; i < abs(move_steps); i++)
  {
    if (digitalRead(lim_pin) == polarity)
      break;
    unsigned long pulse_dur_target_us = CalcPulseDur(pulse_duration, pulse_dur_us, base_time_us);
    
    digitalWrite(PIN_MOTOR_PUL[motor], HIGH);
    ApplyDelay(pulse_dur_target_us/2);
    digitalWrite(PIN_MOTOR_PUL[motor], LOW);
    ApplyDelay(pulse_dur_target_us/2);
    
    if (digitalRead(PIN_MOTOR_DIR[motor]) == HIGH)
    {
      motorPosition[motor] = motorPosition[motor] + 1;
    }
    else
    {
      motorPosition[motor] = motorPosition[motor] - 1;
    }
  }
  return 0;
}

/*uint8_t FreerunMoveMotor(short target_velocity)
{
  unsigned long pulse_duration = (1e6/target_velocity);
  
  if (analogRead(A3) < 400 && CheckMotorLimit(0, motorPosition[0]-1))
  {
    digitalWrite(PIN_MOTOR_DIR[0], LOW);
  
    digitalWrite(PIN_MOTOR_PUL[0], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[0], LOW);
    ApplyDelay(pulse_duration/2);

    motorPosition[0] = motorPosition[0] - 1;
  }
  else if (analogRead(A3) > 600 && CheckMotorLimit(0, motorPosition[0]+1))
  {
    digitalWrite(PIN_MOTOR_DIR[0], HIGH);
    
    digitalWrite(PIN_MOTOR_PUL[0], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[0], LOW);
    ApplyDelay(pulse_duration/2);

    motorPosition[0] = motorPosition[0] + 1;
  }
  else if (analogRead(A4) < 400 && CheckMotorLimit(1, motorPosition[1]-1))
  {
    
    digitalWrite(PIN_MOTOR_DIR[1], LOW);
    
    digitalWrite(PIN_MOTOR_PUL[1], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[1], LOW);
    ApplyDelay(pulse_duration/2);

    motorPosition[1] = motorPosition[1] - 1;
  }
  else if (analogRead(A4) > 600 && CheckMotorLimit(1, motorPosition[1]+1))
  {
    
    digitalWrite(PIN_MOTOR_DIR[1], HIGH);
    
    digitalWrite(PIN_MOTOR_PUL[1], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[1], LOW);
    ApplyDelay(pulse_duration/2);

    motorPosition[1] = motorPosition[1] + 1;
  }
  else if (analogRead(A5) < 400 && CheckMotorLimit(2, motorPosition[2]-1))
  {
    
    digitalWrite(PIN_MOTOR_DIR[2], LOW);
    
    digitalWrite(PIN_MOTOR_PUL[2], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[2], LOW);
    ApplyDelay(pulse_duration/2);

    motorPosition[2] = motorPosition[2] - 1;
  }
  else if (analogRead(A5) > 600  && CheckMotorLimit(2, motorPosition[2]+1))
  {
    
    digitalWrite(PIN_MOTOR_DIR[2], HIGH);
    
    digitalWrite(PIN_MOTOR_PUL[2], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[2], LOW);
    ApplyDelay(pulse_duration/2);

    motorPosition[2] = motorPosition[2] + 1;
  }

  return 0;
}*/
