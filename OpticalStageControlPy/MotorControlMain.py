# -*- coding: utf-8 -*-
"""
Created on Thu Feb 22 14:00:13 2024

@author: Tony
"""
import MotorPresenter
import MotorSerialCom
import MotorControlGui

class OpticalStageControl():
    """Wrapper class for setting the main window"""

    def __init__(self):        
        view = MotorControlGui.Gui()
        
        model = MotorSerialCom.Nano()
        # create presenter
        self._presenter = MotorPresenter.MotorControlPresenter(model, view)
        
        view.set_presenter(self._presenter)
        self._presenter.initialize()

if __name__ == "__main__":
    app = OpticalStageControl()