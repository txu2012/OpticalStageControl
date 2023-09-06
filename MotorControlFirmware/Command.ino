long ByteToInt32(const byte * byteArr)
{
  return static_cast<long>(byteArr[3]) << 24
    | static_cast<long>(byteArr[2]) << 16
    | static_cast<long>(byteArr[1]) << 8
    | byteArr[0];
}

short ByteToInt16(const byte * byteArr)
{
  return static_cast<short>(byteArr[1]) << 8
    | byteArr[0];
}

void WriteSerialByte(const byte* serialReturnByte, int len)
{
  Serial.write(len);
  Serial.write(serialReturnByte, len);
  Serial.flush();
}

void ReturnError() {
  ReturnError(0xff);  
}

void ReturnError(uint8_t err)
{
  // Coerce error code to the range of [0x80, 0xff).
  err |= 0x80;
  WriteSerialByte(&err, 1);
}

void ReadCommand()
{
  int sizeBuffer = Serial.read();
  serialBufferSize = Serial.readBytes(serialBuffer, sizeBuffer);
  switch(serialBuffer[0])
  {
    case 0x01: // Test Command
    {
      if (sizeBuffer == 1 && serialBufferSize == 1)
      {
        TestBlink();
        byte message[] = {0x00};
        WriteSerialByte(message, 1);
      }
      else
      {
        ReturnError();
      }
      break;
    }
    case 0x02: // Get Firmware Version
    {
      if (sizeBuffer == 1 && serialBufferSize == 1)
      {
        byte message[] = {0x00, byte(FwVersion)};
        WriteSerialByte(message, 2);
      }
      else
      {
        ReturnError();
      }
      break;
    }
    case 0x03: // Get Board Type
    {
      if (sizeBuffer == 1 && serialBufferSize == 1)
      {
        byte message[] = {0x00, 'N','a','n','o','S','t','a','g','e','\0','\0','\0','\0','\0','\0','\0'};
        WriteSerialByte(message, 17);
      }
      else
      {
        ReturnError();
      }
      break;
    }
    case 0x20: // Motor Homing 
    {
      if (sizeBuffer == 3 && serialBufferSize == 3)
      {
        const uint8_t res = HomeMotor(serialBuffer[1], velocity, serialBuffer[2]);
        if (res != 0)
        {
          ReturnError(res);
        }
        else
        {
          byte message[] = { 0x00, lowByte(motorPosition[serialBuffer[1]]), highByte(motorPosition[serialBuffer[1]]) };
          WriteSerialByte(message, 3);
        }
      }
      else
      {
        ReturnError();
      }
      break;
    }    
    case 0x21: // Motor Move-To
    {
      if (sizeBuffer == 4 && serialBufferSize == 4)
      {
        const uint8_t res = MoveMotor(serialBuffer[1], ByteToInt16(&(serialBuffer[2])), velocity);
        if(res != 0)
        {
          ReturnError(res);
        }
        else
        {
          byte message[] = { 0x00, lowByte(motorPosition[serialBuffer[1]]), highByte(motorPosition[serialBuffer[1]]) };
          WriteSerialByte(message, 3);
        }
      }
      else
      {
        ReturnError();
      }
      break;
    }
    case 0x22: // Get Motor Position
    {
      if (sizeBuffer == 2 && serialBufferSize == 2)
      {
        if (serialBuffer[1] <= 2)
        {
          byte message[] = { 0x00, lowByte(motorPosition[serialBuffer[1]]), highByte(motorPosition[serialBuffer[1]]) };
          WriteSerialByte(message, 3); 
        }
        else
        {
          ReturnError();
        }
      }
      else
      {
        ReturnError();
      }
      break;
    }
    case 0x23: // Get Motor Position Limit
    {
      if (sizeBuffer == 2 && serialBufferSize == 2)
      {
        if (serialBuffer[1] <= 2)
        {
          byte message[] = { 0x00, lowByte(motorPositionLimit[serialBuffer[1]]), highByte(motorPositionLimit[serialBuffer[1]]) };
          WriteSerialByte(message, 3); 
        }
        else
        {
          ReturnError();
        }
      }
      else
      {
        ReturnError();
      }
      break;
    }
    case 0x24: // Set Motor Position (Used for when reconnecting to get correct position if nano is disconnected)
    {
      if (sizeBuffer == 4 && serialBufferSize == 4)
      {
        if (serialBuffer[1] <= 2)
        {
          motorPosition[serialBuffer[1]] = ByteToInt16(&(serialBuffer[2]));
          
          byte message[] = { 0x00 };
          WriteSerialByte(message, 1); 
        }
        else
        {
          ReturnError();
        }
      }
      else
      {
        ReturnError();
      }
      break;
    }
    case 0x25: // Get Velocity
    {
      if (sizeBuffer == 1 && serialBufferSize == 1)
      {
        byte message[] = { 0x00, lowByte(velocity), highByte(velocity) };
        WriteSerialByte(message, 3); 
      }
      else
      {
        ReturnError();
      }
      break;
    }
    case 0x26: // Calculate limit
    {
      if (sizeBuffer == 1 && serialBufferSize == 1)
      {
        uint8_t res = CalculatePositionLimit();
        if (res == 0)
        {
          byte message[] = { 0x00, lowByte(measuredLimit), highByte(measuredLimit) };
          WriteSerialByte(message, 3); 
        }
        else
        {
          ReturnError();
        }
      }
      else
      {
        ReturnError();
      }
      break;
    }
    case 0x30: // Change Mode
    {
      if (sizeBuffer == 2 && serialBufferSize == 2)
      {
        mode = serialBuffer[1];
        byte message[] = { 0x00 };
        WriteSerialByte(message, 1);
      }
      else
      {
        ReturnError(); 
      }
      break;
    }
    default:
    {
      ReturnError();
      break;
    }
  }
}
