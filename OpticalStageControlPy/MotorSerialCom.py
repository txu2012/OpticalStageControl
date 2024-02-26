import re
import serial
import struct
import time
import string
from typing import List, Tuple


class Nano:
    def __init__(self):
        self._isConnected = False
        self._version = 1
        self._name = "NanoStage"
    
    def __enter__(self):
        return self

    def __exit__(self, type, value, tb):
        self._ser.close()

    def _to_hexstr(self, buf: bytes):
        return " ".join(f"{b:02x}" for b in buf)

    def connect(self, portname: str):
        # To prevent from rebooting the board, set RTS and DTR control to False before
        # opening the port. To do so, don't give the portname to serial.Serial ctor (
        # it would then immediately open the port with DTR = True).
        self._ser = serial.Serial(baudrate=115200)
        self._ser.port = portname
        self._ser.rts = False
        self._ser.dtr = False
        self._ser.timeout = 1.0
        self._ser.open()

        time.sleep(0.1)
        buffer = self._ser.read(10)
        while len(buffer) > 0:
            print(f"Ignoring initial bytes: ({self._to_hexstr(buffer)})")
            buffer = self._ser.read(10)

        self._ser.timeout = 0.2
        
        if not self.check_board():
            self._isConnected = False
            self.disconnect()
        else:
            print(f'Connected to: {portname}')
            self._isConnected = True
            return True
        
    def disconnect(self):
        if self._ser.isOpen():
            self._ser.close()
            
        self._isConnected = False

    def check_board(self):
        error, name = self.QueryBoardName()
        print(f'error: {error}, name: {name}')
        if error != 0 or name.rstrip() != self._name:
            print('Board name does not match.')
            return False
        
        error, version = self.QueryBoardVersion()
        if error != 0 or version != self._version:
            print('Board version does not match.')
            return False
        
        return True

    def write_read(self, cmd: bytes, expected_size: int, timeout: float = 200) -> Tuple[int, bytes]:        
        print(f'({self._to_hexstr(cmd)}', end='')
        original_timeout = self._ser.timeout
        self._ser.timeout = timeout
        
        self._ser.write(bytes([len(cmd)]) + cmd)
        
        try:
            response = self._ser.read(expected_size+1)
            payload_size = response[0]
        except Exception as err:
            print('Reading from serial timed out.')
            return -1, 0xFF
        
        self._ser.timeout = original_timeout
        
        if payload_size <= 0:
            print(f'Error: payload_size={payload_size}')
            return -1, 0xFF
        
        print(f' -> {self._to_hexstr(response)})')
        
        err = response[1]
        return err, response[1:]
          
    def QueryBoardVersion(self) -> Tuple[int, int]:
        print('\nQueryBoardName')
        
        err,response = self.write_read(b'\x02', 2)    
        print(f'error code: {err}')   
        print(f"Response: {response}")
        print(f'Version: {response[1]}') 
        
        return err, response[1]
            
    def QueryBoardName(self) -> Tuple[int, str]:
        print('\nQueryBoardName')
        
        err,response = self.write_read(b'\x03', 17)
        
        print(f'err: {err}')
        print(f"Response: {response}")
        
        if err == 0:
            response = response[1:].decode().rstrip('\x00')
        
        return err, response
        
    def HomeMotor(self, motor: bytes, center: bool) -> Tuple[int, int]:
        print('\nHomeMotor')
        
        if center:
            center_byte = 0x01
        else:
            center_byte = 0x00
        
        cmd = b'\x20' + bytes([motor]) + bytes([center_byte])
        err,response = self.write_read(cmd)
        print(f"Response: {response}")
        
        pos = 0
        if len(response) > 1:
            pos = struct.unpack("<h", response[1:1+2])[0]
            print(f"Position: {pos}")
            
        
        return err, pos
        
    def MoveTo(self, motor: bytes, position: int) -> Tuple[int, int]:
        print('\nMoveTo')
        
        cmd = b'\x21' + bytes([motor]) + struct.pack('<H', position)
        err,response = self.write_read(cmd, 3, 60.0)
        print(f"Response: {response}")
        
        pos = 0
        if len(response) > 1:
            pos = struct.unpack("<h", response[1:1+2])[0]
            print(f"Position: {pos}")
            
        return err, pos
      
    def QueryPosition(self, motor: bytes) -> Tuple[int, int]:
        print('\nQueryPosition')
        
        cmd = b'\x22' + bytes([motor])
        err,response = self.write_read(cmd)
        print(f"Response: {response}")
        
        pos = 0
        if len(response) > 1:
            pos = struct.unpack("<h", response[1:1+2])[0]
            print(f"Position: {pos}")  
        
        return err, pos
        
    def QueryPositionLimit(self, motor: bytes) -> Tuple[int, int]:
        print('\nQueryPositionLimit')
        
        cmd = b'\x23' + bytes([motor])
        err,response = self.write_read(cmd)
        print(f"Response: {response}")
        
        pos = 0
        if len(response) > 1:
            pos = struct.unpack("<h", response[1:1+2])[0]
            print(f"Limit: {pos}")
            
        return err, pos

    def SetMotorPosition(self, motor: bytes, position: int) -> int:
        print('\nSetMotorPosition')
        
        cmd = b'\x24' + bytes([motor]) + struct.pack('<H', position)
        err,response = self.write_read(cmd)
        print(f"Response: {response}")
        
        return err
        
    def GetVelocity(self) -> Tuple[int, int]:
        print('\nGetVelocity')
        
        cmd = b'\x25'
        err,response = self.write_read(cmd)
        print(f"Response: {response}")
        
        velocity = 0
        if len(response) > 1:
            velocity = struct.unpack("<h", response[1:1+2])[0]
            print(f"Velocity: {velocity}")
            
        return err, velocity
            
    def CalculateLimit(self):
        print('\nCalculateLimit')
        
        cmd = b'\x26'
        err,response = self.write_read(cmd, 3, 180.0)
        
        print(f"Response: {response}")
        
        limit = 0
        if len(response) > 1:
            limit = struct.unpack("<h", response[1:1+2])[0]
            print(f"Calculated Limit: {limit}")
            
        return err, limit
            
    def ChangeMode(self, mode: bytes) -> int:
        print('\nChangeMode')
        
        cmd = b'\x30' + bytes([mode])
        err,response = self.write_read(cmd)
        print(f"Response: {response}")
        
        return err