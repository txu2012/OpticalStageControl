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

bool HomeMotor(int motor, short target_velocity)
{
  //motorHomeStatus[motor] = 0x01;
  
  return true;
}

uint8_t MoveMotor(int motor, int motor_direction, short motor_position, short target_velocity)
{
  // Check if position, motor index and target velocity are within supported values
  if (motor > 2)
  {
    return 0xff;
  }

  digitalWrite(PIN_MOTOR_DIR[motor], motor_direction);
  
  // Get amount of steps to move 
  short move_steps = motorPosition[motor] - motor_position;
  unsigned long pulse_duration = (1e6/target_velocity);
  unsigned long base_time_us = micros();
  unsigned long pulse_dur_us = 0;
  
  for (short i = 0; i < abs(move_steps); i++)
  {
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

uint8_t FreerunMoveMotor(short target_velocity)
{
  unsigned long pulse_duration = (1e6/target_velocity);
  
  if (analogRead(A0) < 400)
  {
    digitalWrite(PIN_MOTOR_DIR[0], HIGH);
    
    digitalWrite(PIN_MOTOR_PUL[0], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[0], LOW);
    ApplyDelay(pulse_duration/2);
  }
  else if (analogRead(A0) > 600)
  {
    digitalWrite(PIN_MOTOR_DIR[0], LOW);
    
    digitalWrite(PIN_MOTOR_PUL[0], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[0], LOW);
    ApplyDelay(pulse_duration/2);
  }
  else if (analogRead(A1) < 400)
  {
    digitalWrite(PIN_MOTOR_DIR[1], HIGH);
    
    digitalWrite(PIN_MOTOR_PUL[1], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[1], LOW);
    ApplyDelay(pulse_duration/2);
  }
  else if (analogRead(A1) > 600)
  {
    digitalWrite(PIN_MOTOR_DIR[1], LOW);
    
    digitalWrite(PIN_MOTOR_PUL[1], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[1], LOW);
    ApplyDelay(pulse_duration/2);
  }
  else if (analogRead(A2) < 400)
  {
    digitalWrite(PIN_MOTOR_DIR[2], HIGH);
    
    digitalWrite(PIN_MOTOR_PUL[2], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[2], LOW);
    ApplyDelay(pulse_duration/2);
  }
  else if (analogRead(A2) > 600)
  {
    digitalWrite(PIN_MOTOR_DIR[2], LOW);
    
    digitalWrite(PIN_MOTOR_PUL[2], HIGH);
    ApplyDelay(pulse_duration/2);
    digitalWrite(PIN_MOTOR_PUL[2], LOW);
    ApplyDelay(pulse_duration/2);
  }
}
