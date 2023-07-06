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
        public OpticalStageControl()
        {
            InitializeComponent();
            Presenter = new StagePresenter();
            Presenter.AddView(this);
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
            tbSerial.Text = (Presenter.SerialConnected == true ? Presenter.SerialCom.PortName + ": Connected." : "Disconnected.");
            tbXAxis.Text = "Limit: " + Presenter.MotorLimit[0] + ", Position: " + Presenter.MotorPosition[0];
            tbYAxis.Text = "Limit: " + Presenter.MotorLimit[1] + ", Position: " + Presenter.MotorPosition[1];
            tbZAxis.Text = "Limit: " + Presenter.MotorLimit[2] + ", Position: " + Presenter.MotorPosition[2];

            if (Presenter.SerialConnected)
            {
                pnDevice.Enabled = true;
                btConnect.Enabled = false;
                btDisconnect.Enabled = true;
            }
            else
            {
                pnDevice.Enabled = false;
                btConnect.Enabled = true;
                btDisconnect.Enabled = false;
            }

            if (rbBtnCtrl.Checked)
                gbBtnCtrl.Enabled = true;
            else
                gbBtnCtrl.Enabled = false;

            nudVelocity.Value = Presenter.MotorVelocity;

            tbResponse.Text = Presenter.CommandDisplay;
            tbResponse.SelectionStart = tbResponse.Text.Length;
            tbResponse.ScrollToCaret();
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
                }
            }
        }

        private void btDisconnect_Click(object sender, EventArgs e)
        {
            Presenter.CloseSerialCom();
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            Presenter.TestCommand();
        }

        private void btHomeCenter_Click(object sender, EventArgs e)
        {
            Presenter.HomeMotor(true);
            UpdateDisplay();
        }

        private void btHomeEdge_Click(object sender, EventArgs e)
        {
            Presenter.HomeMotor(false);
            UpdateDisplay();
        }

        private void btXLeft_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(0, (short)(Presenter.MotorPosition[0] - nudDistance.Value));
            UpdateDisplay();
        }

        private void btXRight_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(0, (short)(Presenter.MotorPosition[0] + nudDistance.Value));
            UpdateDisplay();
        }

        private void btYDown_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(1, (short)(Presenter.MotorPosition[1] - nudDistance.Value));
            UpdateDisplay();
        }

        private void btYUp_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(1, (short)(Presenter.MotorPosition[1] + nudDistance.Value));
            UpdateDisplay();
        }

        private void btZLeft_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(2, (short)(Presenter.MotorPosition[2] - nudDistance.Value));
            UpdateDisplay();
        }

        private void btZRight_Click(object sender, EventArgs e)
        {
            Presenter.MoveMotor(2, (short)(Presenter.MotorPosition[2] + nudDistance.Value));
            UpdateDisplay();
        }

        private void rbBtnCtrl_CheckedChanged(object sender, EventArgs e)
        {
            Presenter.SetMode(rbBtnCtrl.Checked == true ? 0 : 1);
            UpdateDisplay();
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
    }
}
