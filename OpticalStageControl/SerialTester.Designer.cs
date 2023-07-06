
namespace OpticalStageControl
{
    partial class SerialTester
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btSend = new System.Windows.Forms.Button();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnSerialTester = new System.Windows.Forms.Panel();
            this.pnSerialTester.SuspendLayout();
            this.SuspendLayout();
            // 
            // btSend
            // 
            this.btSend.Location = new System.Drawing.Point(282, 6);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(75, 23);
            this.btSend.TabIndex = 0;
            this.btSend.Text = "Send";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // tbInput
            // 
            this.tbInput.Location = new System.Drawing.Point(3, 8);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(273, 20);
            this.tbInput.TabIndex = 1;
            // 
            // tbResponse
            // 
            this.tbResponse.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tbResponse.Location = new System.Drawing.Point(3, 34);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.ReadOnly = true;
            this.tbResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResponse.Size = new System.Drawing.Size(273, 118);
            this.tbResponse.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 104);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tester used for \r\nsending bytes\r\nin hex format.\r\n\r\nMust Use \"-\"\r\nbetween hex.\r\n\r\n" +
    "Eg. 00-01-2B";
            // 
            // pnSerialTester
            // 
            this.pnSerialTester.Controls.Add(this.tbInput);
            this.pnSerialTester.Controls.Add(this.label1);
            this.pnSerialTester.Controls.Add(this.btSend);
            this.pnSerialTester.Controls.Add(this.tbResponse);
            this.pnSerialTester.Location = new System.Drawing.Point(5, 6);
            this.pnSerialTester.Name = "pnSerialTester";
            this.pnSerialTester.Size = new System.Drawing.Size(367, 160);
            this.pnSerialTester.TabIndex = 4;
            // 
            // SerialTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 173);
            this.Controls.Add(this.pnSerialTester);
            this.Name = "SerialTester";
            this.Text = "SerialTester";
            this.pnSerialTester.ResumeLayout(false);
            this.pnSerialTester.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.TextBox tbResponse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnSerialTester;
    }
}