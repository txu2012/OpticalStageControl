# -*- coding: utf-8 -*-
import serial.tools.list_ports
import tkinter as tk
from tkinter.ttk import *
import tkinter.scrolledtext as scrolledtext

class Gui(object):
    def __init__(self):        
        self._title = "Optical Stage Control"
        self._root_window = tk.Tk(screenName=self._title)
        self._width = 720
        self._height = 485

        self._selected_port = None
        self._responses = tk.StringVar()
        self._status = tk.StringVar()
        self._lbl_speed = tk.StringVar()
        self._lbl_distance = tk.StringVar()
        self._axis_status = [None, None, None]
        self._nud_distance = 0
        
        self._widget_toggles = []
        self._motor_frame = None
        
        self._presenter = None
        
    def __enter__(self):
        return self
        
    def __exit__(self):
        self._presenter.try_disconnect()
        
    def set_presenter(self, presenter):
        self._presenter = presenter
        
    def update_view(self):
        if (self._presenter.IsConnected):
            self._status.set("Connected")    
            self.toggle_widgets(True)
        else:
            self._status.set("Disconnected")
            self.toggle_widgets(False)
        
    def toggle_widgets(self, toggle: bool):
        for widget in self._widget_toggles:
            widget.configure(state='normal' if toggle else 'disabled')
        
    """
    GUI Creation
    """    
    def create_connection_frame(self):        
        conn_frame = tk.LabelFrame(self._root_window, text="Connection", width=710, height=55)
        conn_frame.grid(rowspan=1, columnspan=6, padx=5, pady=5)
        
        self._cb_ports = tk.ttk.Combobox(conn_frame, 
                                         state="readonly", 
                                         width=8, 
                                         textvariable=self._selected_port,
                                         values=[port[0] for port in serial.tools.list_ports.comports() if port[2] != 'n/a'], 
                                         postcommand= lambda: self._cb_ports.configure(values=[port[0] for port in serial.tools.list_ports.comports() if port[2] != 'n/a']))
        self._cb_ports.grid(row=0, column=0, padx=(5, 0))
        
        tk.Button(conn_frame, text="Connect", width=10, command=self.connect_clicked).grid(row=0, column=1, padx=(5,5))
        tk.Button(conn_frame, text="Disconnect", width=10, command=self.disconnect_clicked).grid(row=0, column=2, padx=(5,5))
        
        tk.Label(conn_frame, text="Status: ").grid(row=0, column=3, padx=(280,0))
        tk.Label(conn_frame, text="Disconnected", textvariable=self._status, width=15).grid(row=0, column=4, padx=(0, 10))
        self._status.set("Disconnected")
        
        conn_frame.grid_propagate(0)
        
    def create_motor_frame(self):
        motor_frame = tk.LabelFrame(self._root_window, text="Motor Control", width=710, height=420)
        motor_frame.place(x=5, y=60)
        self._motor_frame = motor_frame
        
        # Responses
        self._txt_response = scrolledtext.ScrolledText(motor_frame, width=34)
        self._txt_response.config(state='disabled')
        self._txt_response.place(x=410, height=390)
        
        # Homing
        button_frame = tk.Frame(motor_frame, width=300, height=50)
        button_frame.place(x=5, y=2)
        
        btn_home_center = tk.Button(button_frame, text="Home Center", width=10, height=2)
        btn_home_center.grid(row=0, column=0, padx=(0, 20))
        btn_home_edge = tk.Button(button_frame, text="Home Edge", width=10, height=2)
        btn_home_edge.grid(row=0, column=1)
        button_frame.grid_propagate(0)
        
        self._widget_toggles.append(btn_home_center)
        self._widget_toggles.append(btn_home_edge)
        
        # Motor Parameters
        parameter_frame = tk.Frame(motor_frame, width=400, height=50)
        parameter_frame.place(x=5, y=50)
        
        tk.Label(parameter_frame, textvariable=self._lbl_speed, width=15, height=2, anchor="w").grid(row=0, column=0)
        self._txt_speed = tk.Text(parameter_frame, width=8, height=1)
        self._txt_speed.grid(row=0, column=1, sticky="w", padx=(0, 5))
        tk.Label(parameter_frame, textvariable=self._lbl_distance, width=15, height=2, anchor="w").grid(row=0, column=2)
        self._nud_distance = tk.Spinbox(parameter_frame, from_=0.00, to=2000.00, width=8)
        self._nud_distance.grid(row=0, column=3, sticky="w")# make min/max updateable
        self._lbl_speed.set('Velocity(Steps/s): ')
        self._txt_speed.insert(tk.END, f'{str(0)}')
        self._lbl_distance.set('Distance(Steps):')
        self._txt_speed.config(state='disabled')
        parameter_frame.grid_propagate(0)
        
        self._widget_toggles.append(self._nud_distance)
        
        # Axis control
        axis_frame = tk.Frame(motor_frame, width=400, height=400)
        axis_frame.place(x=5, y=90)

        self.create_axis_frame(axis_frame, "X", 0, 0)
        self.create_axis_frame(axis_frame, "Y", 1, 1)
        self.create_axis_frame(axis_frame, "Z", 2, 2)
        
        motor_frame.grid_propagate(0)
    
    def create_axis_frame(self, frame, axis: str, motor: int, row):
        axis_frame = tk.LabelFrame(frame, text=axis+" Axis", width=400, height=100)
        axis_frame.grid(row=row)
        
        self._axis_status[motor] = tk.Text(axis_frame, width=48, height=1)
        self._axis_status[motor].grid(row=0, column=0, padx=(3,2))
        self._axis_status[motor].config(state='disabled')
        
        if axis == "X": 
            txt_btn1 = "Left"
            txt_btn2 = "Right"
        elif axis == "Y":
            txt_btn1 = "Fwd"
            txt_btn2 = "Back"
        else:
            txt_btn1 = "Up"
            txt_btn2 = "Down"
        
        btn_frame = tk.Frame(axis_frame, width=300, height=50)
        btn_frame.grid(row=1, pady=(10,5))
        btn1 = tk.Button(btn_frame, text=txt_btn1, width=10, height=2, command= lambda: self.btn_axis_clicked(motor,-1, self._nud_distance.get()))
        btn1.grid(row=0, column=0, padx=(0, 20))
        btn2 = tk.Button(btn_frame, text=txt_btn2, width=10, height=2, command= lambda: self.btn_axis_clicked(motor, 1, self._nud_distance.get()))
        btn2.grid(row=0, column=1)
        
        self._widget_toggles.append(btn1)
        self._widget_toggles.append(btn2)
        
        axis_frame.grid_propagate(0)
    
    """
    Button Functions
    """
    def connect_clicked(self):
        self._selected_port = self._cb_ports.get()
        
        if self._selected_port != None and not self._presenter.IsConnected:
            self._presenter.try_connect(self._selected_port)
        
    def disconnect_clicked(self):
        if self._presenter.IsConnected:
            self._presenter.try_disconnect()
            
    def btn_axis_clicked(self, motor, direction, distance):
        print(f'Button clicked: motor - {motor}, direction - {direction}, distance - {distance}')
        current_status = self._presenter.MotorStatus
        target_position = current_status.Positions[motor] + int(distance)
        
        success = self._presenter.try_motor_move(motor, target_position)
        
        if not success:
            tk.messagebox.showerror('Motor Move Error', 'Error: Failed to move motor.')
            
        
    def on_closing(self):
        print("Closing window")
        if self._presenter.IsConnected:
            self._presenter.try_disconnect()
        
        self._root_window.destroy()
        
    def add_response(self, msg: str):
        self._txt_response.insert(tk.END, msg)
        
    def start(self):
        self.create_connection_frame()
        self.create_motor_frame()
        self._root_window.protocol("WM_DELETE_WINDOW", self.on_closing)  
        
        self._root_window.geometry(f"{self._width}x{self._height}")
        self._root_window.resizable(False, False)
        self.toggle_widgets(False)
        self._root_window.mainloop()