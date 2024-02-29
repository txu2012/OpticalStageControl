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
        self._debug_log = True
        print('Nano object created.')
    
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
        try:
            error,_, name = self.QueryBoardName()
            print(f'error: {error}, name: {name}')
            if error != 0 or name.rstrip() != self._name:
                print('Board name does not match.')
                return False
            
            error,_, version = self.QueryBoardVersion()
            if error != 0 or version != self._version:
                print('Board version does not match.')
                return False
            
            return True
        except Exception as ex:
            print(ex)
            return False

    def write_read(self, cmd: bytes, expected_size: int, timeout: float = 2.0) -> Tuple[int, bytes]:        
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
          
    def QueryBoardVersion(self) -> Tuple[int, bytes, int]:
        print('\nQueryBoardName')
        
        err,response = self.write_read(b'\x02', 2)    
        
        self.log_response(err, response, response[1])
        return err, response, response[1]
            
    def QueryBoardName(self) -> Tuple[int, bytes, str]:
        print('\nQueryBoardName')
        
        err,response = self.write_read(b'\x03', 17)
        
        if err == 0:
            response = response[1:].decode().rstrip('\x00')
        
        self.log_response(err, response)
        return err, response, response
        
    def HomeMotor(self, motor: bytes, center: bool) -> Tuple[int, bytes, int]:
        print('\nHomeMotor')
        
        if center:
            center_byte = 0x01
        else:
            center_byte = 0x00
        
        cmd = b'\x20' + bytes([motor]) + bytes([center_byte])
        err,response = self.write_read(cmd, 3, 240.0)
        
        pos = 0
        if len(response) > 1:
            pos = struct.unpack("<h", response[1:1+2])[0]
            
        self.log_response(err, response, pos)
        return err, response, pos
        
    def MoveTo(self, motor: bytes, position: int) -> Tuple[int, bytes, int]:
        print(f'\nMoveTo (motor: {motor}, position: {position})')
        
        cmd = b'\x21' + bytes([motor]) + struct.pack('<h', position)
        err,response = self.write_read(cmd, 3, 60.0)
        
        pos = 0
        if len(response) > 1:
            pos = struct.unpack("<h", response[1:1+2])[0]
            
        self.log_response(err, response)
        return err, response, pos
      
    def QueryPosition(self, motor: bytes) -> Tuple[int, bytes, int]:
        print('\nQueryPosition')
        
        cmd = b'\x22' + bytes([motor])
        err,response = self.write_read(cmd, 3)
        
        pos = 0
        if len(response) > 1:
            pos = struct.unpack("<h", response[1:1+2])[0]
        
        self.log_response(err, response, pos)
        return err, response, pos
        
    def QueryPositionLimit(self, motor: bytes) -> Tuple[int, bytes, int]:
        print('\nQueryPositionLimit')
        
        cmd = b'\x23' + bytes([motor])
        err,response = self.write_read(cmd, 3)
        
        limit = 0
        if len(response) > 1:
            limit = struct.unpack("<h", response[1:1+2])[0]
            
        self.log_response(err, response, limit)
        return err, response, limit

    def SetMotorPosition(self, motor: bytes, position: int) -> Tuple[int, bytes]:
        print('\nSetMotorPosition')
        
        cmd = b'\x24' + bytes([motor]) + struct.pack('<h', position)
        err,response = self.write_read(cmd, 1)
        
        self.log_response(err, response)
        return err, response
        
    def QueryVelocity(self) -> Tuple[int, bytes, int]:
        print('\nGetVelocity')
        
        cmd = b'\x25'
        err,response = self.write_read(cmd, 3)
        
        velocity = 0
        if len(response) > 1:
            velocity = struct.unpack("<h", response[1:1+2])[0]
            
        self.log_response(err, response, velocity)    
        return err, response, velocity
            
    def CalculateLimit(self) -> Tuple[int, bytes, int]:
        print('\nCalculateLimit')
        
        cmd = b'\x26'
        err,response = self.write_read(cmd, 3, 240.0)
        
        limit = 0
        if len(response) > 1:
            limit = struct.unpack("<h", response[1:1+2])[0]
            
        self.log_response(err, response, limit)    
        return err, response, limit
            
    def ChangeMode(self, mode: bytes) -> Tuple[int, bytes]:
        print('\nChangeMode')
        
        cmd = b'\x30' + bytes([mode])
        err,response = self.write_read(cmd)
        
        self.log_response(err, response)
        
        return err, response
    
    def log_response(self, err, response, result=''):
        if self._debug_log:            
            print(f'Error Code: {err} \nResponse: {response} \nResult: {result}')

"""    
with Nano() as nano:
    nano.connect('COM9')
    nano.GetVelocity()
    nano.QueryPosition(0)
    nano.MoveTo(0, 10)
    nano.QueryPosition(0)
    nano.MoveTo(0, 2)
    nano.QueryPosition(0)
"""