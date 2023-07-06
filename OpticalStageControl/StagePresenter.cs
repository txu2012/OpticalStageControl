using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticalStageControl
{
    public interface IStageView
    {
        void UpdateDisplay();
        void DisplayError();
    }

    public interface IStagePresenter
    {
        SerialCom SerialCom { get; }
        MotorCommand Motor { get; }
        bool SerialConnected { get; }
        void OpenSerialCom(string port_name);
        void CloseSerialCom();
        string[] PortList { get; }
        short[] MotorPosition { get; }
        short[] MotorLimit { get; }
        short MotorVelocity { get; }

        void HomeMotor(bool center);
        void MoveMotor(int motor_index, short position);
        void SetMode(int mode);

        void SaveMotorPositions();

        string CommandDisplay { get; }
    }

    public class StagePresenter : IStagePresenter
    {
        public StagePresenter()
        {
            SerialCom = null;
            LoadMotorPositions();
        }

        private readonly List<IStageView> views = new List<IStageView>();
        public void AddView(IStageView observer)
        {
            views.Add(observer);
            UpdateViewDisplays();
        }

        public void RemoveView(IStageView observer)
        {
            views.Remove(observer);
        }

        private void UpdateViewDisplays()
        {
            foreach (var view in views)
                view.UpdateDisplay();
        }

        #region Device Object Control
        public SerialCom SerialCom { get; private set; }
        public bool SerialConnected { get { return serialConnected; } }
        public void OpenSerialCom(string port_name)
        {
            CommandDisplay = AppendResponse(CommandDisplay, "Connecting to " + port_name);
            UpdateViewDisplays();

            SerialCom = new SerialCom(port_name);
            SerialCom.OpenPort();

            byte fw = SerialCom.QueryFirmware();
            if (firmwareVersion != fw)
            {
                SerialCom.ClosePort();
                if (fw == 0xfe)
                    CommandDisplay = AppendResponse(CommandDisplay, "Connection timed out. Try reconnecting again.");
                else
                    CommandDisplay = AppendResponse(CommandDisplay, "Connection failed.");
            }

            if (SerialCom.PortConnected)
            {
                serialConnected = SerialCom.PortConnected;
                Motor = new MotorCommand(SerialCom);

                CommandDisplay = AppendResponse(CommandDisplay, "Connection successful.");
                SetMotorPositions();
                GetMotorLimits();
                GetMotorVelocity();
            }
            

            UpdateViewDisplays();
        }
        public void CloseSerialCom()
        {
            SerialCom.ClosePort();
            serialConnected = false;
            SerialCom = null;
            Motor = null;

            CommandDisplay = AppendResponse(CommandDisplay, "Disconnected.");

            UpdateViewDisplays();
        }
        #endregion

        #region Parameters
        public string[] PortList { get { return SerialCom.PortList; } }
        public short[] MotorPosition { get { return this.motor_position; } } // { X, Y, Z }
        public short[] MotorLimit { get { return this.motor_limit; } }
        public short MotorVelocity { get { return this.motor_velocity; } }

        private short[] motor_position = { 0, 0, 0 };
        private short[] motor_limit = { 0, 0, 0 };
        private short motor_velocity = 31;
        private bool serialConnected = false;
        private int firmwareVersion = 1;

        public string CommandDisplay { get; private set; }
        #endregion

        #region Motor Command
        public MotorCommand Motor { get; private set; }

        public void HomeMotor(bool center)
        {
            short x_position = 0;
            short y_position = 0;
            short z_position = 0;

            if (center)
            {
                x_position = (short)(motor_limit[0] / 2);
                y_position = (short)(motor_limit[1] / 2);
                z_position = (short)(motor_limit[2] / 2);
            }

            MoveMotor(0, x_position);
            MoveMotor(2, z_position);
            MoveMotor(1, y_position);
        }
        public void MoveMotor(int motor_index, short position)
        {
            if (position < 0 || position > MotorLimit[motor_index])
            {
                CommandDisplay = AppendResponse(CommandDisplay, "Error: Limit will be exceeded.");
                return;
            };

            List<byte[]> ret = Motor.MoveMotor(motor_index, position);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            if (ret[1][0] == 0x00 && ret[1].Length == 3)
                motor_position[motor_index] = BitConverter.ToInt16(ret[1], 1);
            else
                CommandDisplay = AppendResponse(CommandDisplay, "Error");
        }

        public void SetMode(int mode)
        {
            List<byte[]> ret = Motor.SetMode(mode);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));
        }

        private void SetMotorPositions()
        {
            List<byte[]> ret = Motor.SetPosition(0, (short)MotorPosition[0]);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            ret = Motor.SetPosition(1, (short)MotorPosition[1]);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            ret = Motor.SetPosition(2, (short)MotorPosition[2]);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));
        }

        private void GetMotorLimits()
        {
            List<byte[]> ret = Motor.QueryLimit(0);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            if (ret[1][0] == 0x00 && ret[1].Length == 3)
                motor_limit[0] = BitConverter.ToInt16(ret[1], 1);
            else
                CommandDisplay = AppendResponse(CommandDisplay, "Error");

            ret = Motor.QueryLimit(1);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            if (ret[1][0] == 0x00 && ret[1].Length == 3)
                motor_limit[1] = BitConverter.ToInt16(ret[1], 1);
            else
                CommandDisplay = AppendResponse(CommandDisplay, "Error");

            ret = Motor.QueryLimit(2);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            if (ret[1][0] == 0x00 && ret[1].Length == 3)
                motor_limit[2] = BitConverter.ToInt16(ret[1], 1);
            else
                CommandDisplay = AppendResponse(CommandDisplay, "Error");
        }

        private void GetMotorVelocity()
        {
            List<byte[]> ret = Motor.QueryVelociy();
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            if (ret[1][0] == 0x00 && ret[1].Length == 3)
                motor_velocity = BitConverter.ToInt16(ret[1], 1);
            else
                CommandDisplay = AppendResponse(CommandDisplay, "Error");
        }
        #endregion

        #region Other Functions
        public string AppendResponse(string currentText, string input = null, string response = null)
        {
            StringBuilder sb = new StringBuilder(currentText);
            sb.AppendLine(input);
            sb.AppendLine(response);
            if (response != null) sb.AppendLine("");

            return sb.ToString();
        }

        public void TestCommand()
        {
            List<byte[]> ret = SerialCom.TestCommand();
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            UpdateViewDisplays();
        }

        public void SaveMotorPositions()
        {
            Properties.Settings.Default.XPosition = motor_position[0];
            Properties.Settings.Default.YPosition = motor_position[1];
            Properties.Settings.Default.ZPosition = motor_position[2];
            Properties.Settings.Default.Save();
        }

        private void LoadMotorPositions()
        {
            motor_position[0] = Properties.Settings.Default.XPosition;
            motor_position[1] = Properties.Settings.Default.YPosition;
            motor_position[2] = Properties.Settings.Default.ZPosition;
        }
        #endregion
    }
}
