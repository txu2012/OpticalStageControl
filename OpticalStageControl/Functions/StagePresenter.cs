using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
        double[] MotorPosition { get; }
        short[] MotorLimit { get; }
        short MotorVelocity { get; }

        bool Movement { get; }
        Task HomeMotor(bool center);
        Task MoveMotor(int motor_index, short position);
        void SetMode(int mode);

        void SaveMotorPositions();

        string CommandDisplay { get; }
    }

    public class StagePresenter : IStagePresenter
    {
        public string ReleaseVersionControl
        {
            get { return $"{softwareVersion}.0.0"; }
        }
        private int softwareVersion = 1;
        private int firmwareVersion = 1;

        public StagePresenter()
        {
            SerialCom = null;
            Conversion = 160;
            ConvertFlag = true;
            LoadMotorPositions();
            Movement = false;
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
            byte[] board = SerialCom.QueryBoardName();
            string boardString = Encoding.UTF8.GetString(board, 1, board.Length - 1).Replace("\0","");

            if (firmwareVersion == fw && boardName == boardString)
            {
                if (SerialCom.PortConnected)
                {
                    serialConnected = SerialCom.PortConnected;
                    Motor = new MotorCommand(SerialCom);

                    CommandDisplay = AppendResponse(CommandDisplay, "Connection successful.");
                    SetMotorPositions();
                    GetMotorLimits();
                    GetMotorVelocity();
                    retryConnection = 0;
                }
            }
            else
            {
                SerialCom.ClosePort();
                
                if (fw != firmwareVersion)
                {
                    // If timed out, try a second time. Plugging in the nano may cause a timeout when first connecting.
                    if (fw == 0xfe)
                    {
                        if (retryConnection == 0)
                        {
                            CommandDisplay = AppendResponse(CommandDisplay, "Connection timed out. Retrying connection.");

                            retryConnection++;
                            OpenSerialCom(port_name);
                        }
                    }
                    else
                        CommandDisplay = AppendResponse(CommandDisplay, "Connection timed out. Firmware version do not match");
                }
                else if (boardString != boardName)
                    CommandDisplay = AppendResponse(CommandDisplay, "Connection timed out. Board Names do not match");
                else
                    CommandDisplay = AppendResponse(CommandDisplay, "Connection failed.");
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
        public double[] MotorPosition 
        { 
            get 
            {
                if (ConvertFlag)
                    return new double[] {
                        (this.motor_position[0] <= 0) ? 0 : (double)this.motor_position[0] / Conversion,
                        (this.motor_position[1] <= 0) ? 0 : (double)this.motor_position[1] / Conversion,
                        (this.motor_position[2] <= 0) ? 0 : (double)this.motor_position[2] / Conversion
                    };
                else
                    return new double[] {
                        this.motor_position[0],
                        this.motor_position[1],
                        this.motor_position[2]
                    }; 
            } 
        } // { X, Y, Z }
        public double MoveDistance 
        { 
            get
            {
                if (ConvertFlag)
                    return (this.move_distance <= 0) ? 0 : this.move_distance / Conversion;
                else
                    return this.move_distance;
            }
            set
            {
                if (ConvertFlag)
                    this.move_distance = (value > distance_limit / Conversion) ? distance_limit : value * Conversion;
                else
                    this.move_distance = (value > distance_limit) ? distance_limit : value;
            }
        }
        public short[] MotorLimit { get { return this.motor_limit; } }
        public short MotorVelocity { get { return this.motor_velocity; } }
        public short Conversion { get; set; }
        public bool ConvertFlag { get; set; }

        public short[] motor_position = { 0, 0, 0 };
        private short[] motor_limit = { 0, 0, 0 };
        private short motor_velocity = 200;
        private bool serialConnected = false;
        private int retryConnection = 0;
        private short distance_limit = 20000;
        public double move_distance = 0;

        private string boardName = "NanoStage";
        public string CommandDisplay { get; private set; }
        public bool Movement { get; private set; }
        #endregion

        #region Motor Command
        public MotorCommand Motor { get; private set; }

        public async Task HomeMotor(bool center)
        {
            Movement = true;
            UpdateViewDisplays();

            await Task.Run(() =>
            {
                HomeCommand(0, center);
                HomeCommand(1, center);
                HomeCommand(2, center);
            });

            Movement = false;
            UpdateViewDisplays();
        }
        private void HomeCommand(int motor_index, bool center)
        {
            double timeout_ms = ((20400 / 250) * 1e3) + 1000;
            List<byte[]> ret = Motor.HomeMotor(motor_index, center, timeout_ms);

            Movement = false;
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            if (ret[1][0] == 0x00 && ret[1].Length == 3)
                motor_position[motor_index] = BitConverter.ToInt16(ret[1], 1);
            else
                CommandDisplay = AppendResponse(CommandDisplay, "Error Communication");
        }
        public async Task MoveMotor(int motor_index, short position)
        {
            if (position < 0 || position > MotorLimit[motor_index])
            {
                CommandDisplay = AppendResponse(CommandDisplay, "Error: Limit will be exceeded.");
                return;
            };

            Movement = true;
            UpdateViewDisplays();

            double timeout_ms = ((Math.Abs(position - MotorPosition[motor_index]) / 250) * 1e3) + 1000;

            //List<byte[]> ret = Motor.MoveMotor(motor_index, position, timeout_ms);
            var ret = await Task.Run(() => Motor.MoveMotor(motor_index, position, timeout_ms));

            Movement = false;
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            if (ret[1][0] == 0x00 && ret[1].Length == 3)
                motor_position[motor_index] = BitConverter.ToInt16(ret[1], 1);
            else
                CommandDisplay = AppendResponse(CommandDisplay, "Error Communication");

            UpdateViewDisplays();
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

        public void ResetPositions()
        {
            List<byte[]> ret = Motor.SetPosition(0, 0);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            ret = Motor.SetPosition(1, 0);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            ret = Motor.SetPosition(2, 0);
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));

            motor_position[0] = 0;
            motor_position[1] = 0;
            motor_position[2] = 0;

            UpdateViewDisplays();
        }

        public void MeasureLimit()
        {
            List<byte[]> ret = Motor.MeasureLimit();
            CommandDisplay = AppendResponse(CommandDisplay, "Command : " + BitConverter.ToString(ret[0]), "Response: " + BitConverter.ToString(ret[1]));
            CommandDisplay = AppendResponse(CommandDisplay, "Limit : " + BitConverter.ToInt16(ret[1], 1).ToString());

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
