# -*- coding: utf-8 -*-
"""
Created on Fri Feb 23 13:13:34 2024

@author: Tony
"""
import JsonConfig

class MotorStatus(object):
    def __init__(self):
        self._positions = [0.00, 0.00, 0.00]
        self._move_position = 0.00
        self._velocity = 0.00
        self._position_limits = [112.50, 112.50, 112.50]
        self._homed = [False, False, False]
        self._unit = "mm"
        self._convert_flag = True
        self._steps_per_mm = 160
        self._polarity = [1, 1, 1]
        
    @property
    def Positions(self):
        if self._convert_flag:
            return [
                self.ConvertToMm(self._positions[0]),
                self.ConvertToMm(self._positions[1]),
                self.ConvertToMm(self._positions[2])]
        else:
            return self._positions
    @Positions.setter
    def Positions(self, value):
        if self._convert_flag:
            self._positions = [
                self.ConvertToSteps(value[0]),
                self.ConvertToSteps(value[1]),
                self.ConvertToSteps(value[2])]
        else:
            self._positions = value
        
    @property
    def PositionLimits(self):
        if self._convert_flag:
            return [
                self.ConvertToMm(self._position_limits[0]),
                self.ConvertToMm(self._position_limits[1]),
                self.ConvertToMm(self._position_limits[2])]
        else:
            return self._position_limits
    @PositionLimits.setter
    def PositionLimits(self, value):
        if self._convert_flag:
            self._position_limits = [
                self.ConvertToSteps(value[0]),
                self.ConvertToSteps(value[1]),
                self.ConvertToSteps(value[2])]
        else:
            self._position_limits = value
        
    @property
    def MovePosition(self):
        if self._convert_flag:
            return self.ConvertToMm(self._move_position)
        else:
            return self._move_position
    @MovePosition.setter
    def MovePosition(self, value):
        if self._convert_flag:
            self._move_position = self.ConvertToSteps(value)
        else:
            self._move_position = value
        
    @property
    def Velocity(self):
        if self._convert_flag:
            return self.ConvertToMm(self._velocity)
        else:
            return self._velocity
    @Velocity.setter
    def Velocity(self, value):
        if self._convert_flag:
            self._velocity = self.ConvertToSteps(value)
        else:
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
        
    @property
    def StepsPerMm(self):
        return self._steps_per_mm
    @StepsPerMm.setter
    def StepsPerMm(self, value):
        self._steps_per_mm = value
        
    @property
    def Polarity(self):
        return self._polarity
    @Polarity.setter
    def Polarity(self, value):
        self._polarity = value
        
    @property
    def Unit(self):
        if self._convert_flag:
            return "mm"
        else:
            return "steps"
       
    def ConvertToMm(self, value):
        ret_val = value / self._steps_per_mm
        return 0 if ret_val < 0 else ret_val
    def ConvertToSteps(self, value):
        ret_val = value * self._steps_per_mm
        return 0 if ret_val < 0 else ret_val
    
    def GetValues(self):
       return {
            "Positions": self._positions,
            "Velocity": self._velocity,
            "PositionLimits": self._position_limits
            }
    def SetValues(self, values):
       self._positions = values["Positions"]
       self._position_limits = values["PositionLimits"]
       self._velocity = values["Velocity"]

class MotorControlPresenter(object):
    def __init__(self, motor_serial, view):
        self._motor_serial = motor_serial
        self._view = view
        self._config = JsonConfig.Config('config.json')
        self._motor_status = MotorStatus()
        
        self._convert_flag = False
        
        self._config.load_json()
        self.load_previous_values()
    
    def initialize(self):        
        self._view.start()
        
    def log_response(self, function:str, response:bytes, result='0'):
        hexstr = " ".join(f"{b:02x}" for b in response)
        self._view.update_response(f'{function} \nResponse: {hexstr} \nResult: {result}\n')
        
    # Device control
    def try_connect(self, portname):
        success = False
        if self._motor_serial.connect(portname):
            success = self.configure_device()
            print("Connected") 
        
        return success
        
    def try_disconnect(self):
        self._motor_serial.disconnect()
        if not self._motor_serial._isConnected:
            print("Disconnected")
            self.save_current_values()
            
        self._view.update_view()
    
    def configure_device(self):
        self.load_previous_values()
        success = True
        
        if success:
            err,response,limit_x = self._motor_serial.QueryPositionLimit(0)
            self.log_response('Query Position Limit X', response, limit_x)
            if err != 0: success = False
            
        if success:
            err,response,limit_y = self._motor_serial.QueryPositionLimit(1)
            self.log_response('Query Position Limit Y', response, limit_y)
            if err != 0: success = False
            
        if success:
            err,response,limit_z = self._motor_serial.QueryPositionLimit(2)
            self.log_response('Query Position Limit Z', response, limit_z)
            if err != 0: success = False
            
        if success: 
            err,response,velocity = self._motor_serial.QueryVelocity()
            self.log_response('Query Velocity', response, velocity)
            if err != 0: success = False
        
        if success:
            err,response = self._motor_serial.SetMotorPosition(0, int(self._motor_status._positions[0]))
            self.log_response('Set Position X', response, self._motor_status._positions[0])
            if err != 0: success = False
            
        if success:    
            err,response = self._motor_serial.SetMotorPosition(1, int(self._motor_status._positions[1]))
            self.log_response('Set Position Y', response, self._motor_status._positions[1])
            if err != 0: success = False
        
        if success:
            err,response = self._motor_serial.SetMotorPosition(2, int(self._motor_status._positions[2]))
            self.log_response('Set Position Z', response, self._motor_status._positions[2])
            if err != 0: success = False
            
        if success:
            self._motor_status._velocity = velocity
            self._motor_status._position_limits = [limit_x, limit_y, limit_z]
            
        return success
        
    def try_motor_move(self, motor: bytes, target_position: int):
        msg = ''
        success = True
        
        if target_position == self._motor_status._positions[motor]:
            return True, ''
        
        if target_position < 0.0 or target_position > self._motor_status._position_limits[motor]:
            return False, 'Out of Range'
        
        err,response,position = self._motor_serial.MoveTo(motor, int(target_position))
        self.log_response('Move To', response, position)

        if err == 0:
            self._motor_status._positions[motor] = position
            success = True
        else:
            msg = f'Failed to move motor. Error code: {err}'
            success = False
        
        self._view.update_view()
        return success, msg
        
    def try_motor_home(self, center: bool):
        success, msg = self.HomeCommand(0, center)
        if not success:
            return success, msg
        print('X finished')
        
        success, msg = self.HomeCommand(1, center)
        if not success:
            return success, msg
        print('Y finished')
        
        success, msg = self.HomeCommand(2, center)
        if not success:
            return success, msg
        print('Z finished')
        
        return success, msg
        
    def HomeCommand(self, motor: int, center: bool):
        success = False
        msg = ''
        err,response,position = self._motor_serial.HomeMotor(motor, center)
        
        self.log_response('HomeMotor', response, position)
        
        if err == 0:
            self._motor_status._positions[motor] = position
            success = True
        else:
            success = False
            msg = f'Failed to home motor {motor}. Error code: {err}'
            
        self._view.update_view()
        return success, msg
    
    def try_calc_limit(self):
        success = False
        msg = ''
        err,response,limit = self._motor_serial.CalculateLimit()
        
        self.log_response('CalculateLimit', response, limit)
        
        if err == 0:
            self._motor_status._position_limits = [limit, limit, limit]
            success = True
        else:
            success = False
            msg = f'Faild to calculate limit. Error code: {err}'
            
        self._view.update_view()
        return success, msg
    
    # Load/Save motor parameters to json for next start
    def load_previous_values(self):
        dict_values = self._config.load_values()
        
        self._motor_status.SetValues({
            "Positions": dict_values["MotorPositions"],
            "PositionLimits": dict_values["MotorLimits"],
            "Velocity": dict_values["MotorVelocity"]
            })
        
    def save_current_values(self):
        current_vals = self._motor_status.GetValues()
        
        dict_values = self._config.load_values()
        dict_values["MotorPositions"] = current_vals["Positions"]
        dict_values["MotorVelocity"] = current_vals["Velocity"]
        dict_values["MotorLimits"] = current_vals["PositionLimits"]
        
        self._config.save_json(dict_values)
        
    #Properties
    @property
    def MotorStatus(self):
        return self._motor_status
    
    @property
    def IsConnected(self):
        return self._motor_serial._isConnected