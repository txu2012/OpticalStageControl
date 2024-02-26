# -*- coding: utf-8 -*-
"""
Created on Fri Feb 23 13:13:34 2024

@author: Tony
"""
import JsonConfig

class MotorStatus(object):
    def __init__(self):
        self._positions = [0, 0, 0]
        self._move_positions = [0, 0, 0]
        self._velocity = 0
        self._position_limits = [18000, 18000, 18000]
        self._homed = [False, False, False]
        self._unit = "Steps"
        self._convert_flag = False
        
    @property
    def Positions(self):
        return self._positions
    @Positions.setter
    def Positions(self, value):
        self._positions = value
        
    @property
    def PositionLimits(self):
        return self._position_limits
    @PositionLimits.setter
    def PositionLimits(self, value):
        self._position_limits = value
        
    @property
    def Velocity(self):
        return self._velocity
    @Velocity.setter
    def Velocity(self, value):
        self._velocity = value
        
    @property
    def Homed(self):
        return self._homed
    @Homed.setter
    def Homed(self, value):
        self._homed = value
        
    @property
    def ConvertFlag(self):
        return self._convert_flag
    @ConvertFlag.setter
    def ConvertFlag(self, value):
        self._convert_flag = value

class MotorControlPresenter(object):
    def __init__(self, motor_serial, view):
        self._motor_serial = motor_serial
        self._view = view
        self._config = JsonConfig.Config('config.json')
        self._motor_status = MotorStatus()
        
        self._convert_flag = False
    
    def initialize(self):        
        self._view.start()
        self._config.load_json()
        
        self.load_previous_values()
    
    # Device control
    def try_connect(self, portname):
        if self._motor_serial.connect(portname):
            print("Connected") 
        
        self._view.update_view()
        
    def try_disconnect(self):
        self._motor_serial.disconnect()
        if not self._motor_serial._isConnected:
            print("Disconnected")
            
        self._view.update_view()
        
    def try_motor_move(self, motor: bytes, target_position: int):
        success = False
        err, position = self._motor_serial.MoveTo(motor, target_position)
                
        if err == 0:
            self._motor_status.Positions[motor] = position
            success = True
        self._motor_status.Positions[motor] = target_position
        print(f'move motor {target_position}, {self._motor_status.Positions[motor]}')
        
        self._view.update_view()
        return success
        
    # Load/Save motor parameters to json for next start
    def load_previous_values(self):
        dict_values = self._config.load_values()
        
        self._motor_status.Positions = dict_values["MotorPositions"]
        self._motor_status.Velocity = dict_values["MotorVelocity"]
        self._motor_status.PositionLimits = dict_values["MotorLimits"]
        
        print('Values loaded: ')
        print(f'Positions: {self._motor_status.Positions}')
        print(f'Velocity: {self._motor_status.Velocity}')
        print(f'Limits: {self._motor_status.PositionLimits}')
        
    def save_current_values(self):
        dict_values = self._config.load_values()
        dict_values["MotorPositions"] = self._motor_status.Positions
        dict_values["MotorVelocity"] = self._motor_status.Velocity
        dict_values["MotorLimits"] = self._motor_status.PositionLimits
        
        self._config.save_json(dict_values)
        print('Saved values to json.')
        
    #Properties
    @property
    def MotorStatus(self):
        return self._motor_status
    
    @property
    def IsConnected(self):
        return self._motor_serial._isConnected