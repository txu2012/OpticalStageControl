# -*- coding: utf-8 -*-
"""
Created on Mon Feb 26 09:28:33 2024

@author: Tony
"""

import os.path as os
import json

class Config:
    def __init__(self, filename: str):
        self._values = ""
        self._dict_values = None
        self._filename = filename
        
        if not os.isfile(self._filename):
            self._dict_values = {
                "MotorPositions": [0,0,0],
                "MotorVelocity": 200,
                "MotorLimits": [18000, 18000, 18000]
            }
            
            file = open(self._filename, "w")
            json.dump(self._dict_values, file, indent=3)
            file.close()
        
    def load_json(self):
        # if file does not exist, create new file
        file = open(self._filename)
        self._dict_values = json.load(file)
        file.close()
        
        #print(self._dict_values)
        
        #return self._dict_values
        
    def save_json(self, dict_values):
        self._dict_values = dict_values
        
        file = open(self._filename, "w")
        json.dump(self._dict_values, file, indent=3)
        file.close()
        
    def load_values(self):
        return self._dict_values