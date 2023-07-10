using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticalStageControl
{
    public partial class SerialTester : Form, IStageView
    {
        private StagePresenter Presenter;
        public SerialTester(StagePresenter presenter)
        {
            InitializeComponent();
            Presenter = presenter;
            Presenter.AddView(this);
        }
        
        public void UpdateDisplay()
        {
            if (!Presenter.SerialConnected)
                pnSerialTester.Enabled = false;
            else
                pnSerialTester.Enabled = true;

            tbResponse.SelectionStart = tbResponse.Text.Length;
            tbResponse.ScrollToCaret();
        }

        public void DisplayError() { }

        private void btSend_Click(object sender, EventArgs e)
        {
            SendCommand(); 
        }

        private void SendCommand()
        {
            if (tbInput.Text == "") return;

            string[] hexString = tbInput.Text.Split('-');
            byte[] command = new byte[hexString.Length];

            try
            {
                for (int i = 0; i < hexString.Length; i++)
                {
                    if (hexString[i].Length > 2)
                        throw new ArgumentException($"Input has longer than 2 values at {hexString[i]}");
                    command[i] = Convert.ToByte(hexString[i], 16);
                }

                byte[] ret = Presenter.SerialCom.PortWriteReadByte(command, command.Length);

                tbResponse.Text = Presenter.AppendResponse(tbResponse.Text, "Command : " + BitConverter.ToString(command), "Response: " + BitConverter.ToString(ret));
                tbInput.Text = "";
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Hex or byte format not supported. Check input string.\n\r{ex.Message}");
            }
        }
        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendCommand();
        }

        private void tbInput_TextChanged(object sender, EventArgs e)
        {
            if (tbInput.Text == "")
                btSend.Enabled = false;
            else
                btSend.Enabled = true;
        }
    }
}
