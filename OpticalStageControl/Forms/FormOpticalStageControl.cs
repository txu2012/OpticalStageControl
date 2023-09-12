using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace OpticalStageControl
{
    public partial class OpticalStageControl : Form, IStageView
    {
        private StagePresenter Presenter;
        private bool updating = false;
        public OpticalStageControl()
        {
            InitializeComponent();
            Presenter = new StagePresenter();
            Presenter.AddView(this);
            
            menuStrip1.Visible = false;
            Presenter.InitializeJoystick();
        }

        // Detect USB Device plug in/removal and refresh COM list
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            int WM_DEVICECHANGE = 0x219, 
                DBT_DEVICEARRIVAL = 0x8000, 
                DBT_DEVICEREMOVECOMPLETE = 0x8004, 
                DBT_DEVTYP_PORT = 0x00000003;

            if (m.Msg == WM_DEVICECHANGE)
            {
                if ((int)m.WParam == DBT_DEVICEARRIVAL || (int)m.WParam == DBT_DEVICEREMOVECOMPLETE)
                {
                    int devType = Marshal.ReadInt32(m.LParam, 4);
                    if (devType == DBT_DEVTYP_PORT)
                    {
                        RefreshComs();
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Presenter.SaveMotorPositions();
        }

        private void RefreshComs()
        {
            cbSerial.Items.Clear();

            foreach(string port_name in Presenter.PortList)
            {
                if (port_name == "CNCA0" || port_name == "CNCB0")
                    continue;
                cbSerial.Items.Add(port_name);
            }

            if (cbSerial.Items.Count > 0)
                cbSerial.SelectedIndex = 0;

            UpdateDisplay();
        }

        public void DisplayError() {}

        public void UpdateDisplay()
        {
            updating = true;

            tbSerial.Text = (Presenter.SerialConnected == true ? Presenter.SerialCom.PortName + ": Connected." : "Disconnected.");
            tbXAxis.Text = "Limit: " + Presenter.MotorLimit[0] + ", Position: " + Math.Round(Presenter.MotorPosition[0], 2).ToString("0.00");
            tbYAxis.Text = "Limit: " + Presenter.MotorLimit[1] + ", Position: " + Math.Round(Presenter.MotorPosition[1], 2).ToString("0.00");
            tbZAxis.Text = "Limit: " + Presenter.MotorLimit[2] + ", Position: " + Math.Round(Presenter.MotorPosition[2], 2).ToString("0.00");

            if (Presenter.SerialConnected)
            {
                pnDevice.Enabled = true;
                btConnect.Enabled = false;
                btRefresh.Enabled = false;
                cbSerial.Enabled = false;
                btDisconnect.Enabled = true;
            }
            else
            {
                pnDevice.Enabled = false;
                btConnect.Enabled = true;
                btRefresh.Enabled = true;
                cbSerial.Enabled = true;
                btDisconnect.Enabled = false;
            }

            if (!Presenter.Movement && Presenter.SerialConnected)
            {
                gbBtnCtrl.Enabled = true;
                btHomeCenter.Enabled = true;
                btHomeEdge.Enabled = true;
                btDisconnect.Enabled = true;
            }
            else
            {
                gbBtnCtrl.Enabled = false;
                btHomeCenter.Enabled = false;
                btHomeEdge.Enabled = false;
                btDisconnect.Enabled = false;
            }

            nudVelocity.Value = Presenter.MotorVelocity;
            nudConversion.Value = Presenter.Conversion;
            nudDistance.Value = (decimal)Presenter.MoveDistance;
            cbConvertToMm.Checked = Presenter.ConvertFlag;

            tbResponse.Text = Presenter.CommandDisplay;
            tbResponse.SelectionStart = tbResponse.Text.Length;
            tbResponse.ScrollToCaret();

            updating = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshComs();
        }

        #region UI Interaction
        private void btRefresh_Click(object sender, EventArgs e)
        {
            RefreshComs();
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            if (cbSerial.SelectedItem != null)
            {
                if (!Presenter.SerialConnected)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    Presenter.OpenSerialCom(cbSerial.SelectedItem.ToString());
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
                    jsTimer.Enabled = true;
                }
            }
        }

        private void btDisconnect_Click(object sender, EventArgs e)
        {
            Presenter.CloseSerialCom();
            jsTimer.Enabled = false;
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            Presenter.TestCommand();
        }

        private void btHomeCenter_Click(object sender, EventArgs e)
        {
            Presenter.HomeMotor(true);
        }

        private void btHomeEdge_Click(object sender, EventArgs e)
        {
            Presenter.HomeMotor(false);
        }

        private void btXLeft_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(0, (short)(Presenter.motor_position[0] - Presenter.move_distance));
        }

        private void btXRight_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(0, (short)(Presenter.motor_position[0] + Presenter.move_distance));
        }

        private void btZDown_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(2, (short)(Presenter.motor_position[2] + Presenter.move_distance));
        }

        private void btZUp_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(2, (short)(Presenter.motor_position[2] - Presenter.move_distance));
        }

        private void btYLeft_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(1, (short)(Presenter.motor_position[1] - Presenter.move_distance));
        }

        private void btYRight_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(1, (short)(Presenter.motor_position[1] + Presenter.move_distance));
        }

        private void rbBtnCtrl_CheckedChanged(object sender, EventArgs e)
        {
            Presenter.SetMode(rbBtnCtrl.Checked == true ? 0 : 1);
            UpdateDisplay();
        }

        private void btLimit_Click(object sender, EventArgs e)
        {
            Presenter.MeasureLimit();
        }

        private void cbConvertToMm_CheckedChanged(object sender, EventArgs e)
        {
            if (updating) return;
            Presenter.ConvertFlag = cbConvertToMm.Checked;
            UpdateDisplay();
        }

        private void nudConversion_ValueChanged(object sender, EventArgs e)
        {
            if (updating) return;
            Presenter.Conversion = (short)nudConversion.Value;
            UpdateDisplay();
        }

        private void nudDistance_ValueChanged(object sender, EventArgs e)
        {
            if (updating) return;
            Presenter.MoveDistance = (double)nudDistance.Value;
            UpdateDisplay();
        }
        private void nudDistance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (updating) return;
                Presenter.MoveDistance = (double)nudDistance.Value;
                UpdateDisplay();
                
                e.Handled = true;
            }
        }
        #endregion

        private void serialTesterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Presenter.SerialConnected)
            {
                MessageBox.Show("Must be connected to a serial device.");
                return;
            }

            SerialTester serialTester = new SerialTester(Presenter);
            serialTester.Show();
        }

        private void OpticalStageControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && (e.KeyCode == Keys.T))
            {
                menuStrip1.Visible = (menuStrip1.Visible ? false : true);

                if (Presenter.SerialConnected)
                {
                    btTest.Visible = (btTest.Visible ? false : true);
                    btLimit.Visible = (btLimit.Visible ? false : true);
                    btReset.Visible = (btReset.Visible ? false : true);
                }
            }

            e.Handled = true;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout version = new FormAbout(Presenter.ReleaseVersionControl);
            version.Show();
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            if (updating) return;
            Presenter.ResetPositions();
        }

        private void jsTimer_Tick(object sender, EventArgs e)
        {
            Presenter.CheckJoystickState();
            //Presenter.GetMotorPositions(true);
            UpdateDisplay();
        }
    }
}