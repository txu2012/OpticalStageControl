
namespace OpticalStageControl
{
    partial class OpticalStageControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpticalStageControl));
            this.cbSerial = new System.Windows.Forms.ComboBox();
            this.tbSerial = new System.Windows.Forms.TextBox();
            this.btConnect = new System.Windows.Forms.Button();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.btRefresh = new System.Windows.Forms.Button();
            this.pnDevice = new System.Windows.Forms.Panel();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.gbBtnCtrl = new System.Windows.Forms.GroupBox();
            this.tbXAxis = new System.Windows.Forms.TextBox();
            this.tbZAxis = new System.Windows.Forms.TextBox();
            this.btZLeft = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btYDown = new System.Windows.Forms.Button();
            this.tbYAxis = new System.Windows.Forms.TextBox();
            this.btXLeft = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btXRight = new System.Windows.Forms.Button();
            this.nudDistance = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btZRight = new System.Windows.Forms.Button();
            this.nudVelocity = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btYUp = new System.Windows.Forms.Button();
            this.gbMode = new System.Windows.Forms.GroupBox();
            this.rbJoystick = new System.Windows.Forms.RadioButton();
            this.rbBtnCtrl = new System.Windows.Forms.RadioButton();
            this.btHomeEdge = new System.Windows.Forms.Button();
            this.btHomeCenter = new System.Windows.Forms.Button();
            this.btTest = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serialTesterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnDevice.SuspendLayout();
            this.gbBtnCtrl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVelocity)).BeginInit();
            this.gbMode.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbSerial
            // 
            this.cbSerial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSerial.FormattingEnabled = true;
            this.cbSerial.Location = new System.Drawing.Point(325, 37);
            this.cbSerial.Name = "cbSerial";
            this.cbSerial.Size = new System.Drawing.Size(87, 24);
            this.cbSerial.TabIndex = 0;
            // 
            // tbSerial
            // 
            this.tbSerial.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tbSerial.Enabled = false;
            this.tbSerial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSerial.Location = new System.Drawing.Point(12, 38);
            this.tbSerial.Name = "tbSerial";
            this.tbSerial.Size = new System.Drawing.Size(308, 23);
            this.tbSerial.TabIndex = 1;
            // 
            // btConnect
            // 
            this.btConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btConnect.Location = new System.Drawing.Point(511, 33);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(83, 32);
            this.btConnect.TabIndex = 2;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // btDisconnect
            // 
            this.btDisconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btDisconnect.Location = new System.Drawing.Point(600, 33);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(87, 32);
            this.btDisconnect.TabIndex = 3;
            this.btDisconnect.Text = "Disconnect";
            this.btDisconnect.UseVisualStyleBackColor = true;
            this.btDisconnect.Click += new System.EventHandler(this.btDisconnect_Click);
            // 
            // btRefresh
            // 
            this.btRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRefresh.Location = new System.Drawing.Point(418, 33);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(87, 32);
            this.btRefresh.TabIndex = 4;
            this.btRefresh.Text = "Refresh";
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // pnDevice
            // 
            this.pnDevice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnDevice.Controls.Add(this.tbResponse);
            this.pnDevice.Controls.Add(this.gbBtnCtrl);
            this.pnDevice.Controls.Add(this.gbMode);
            this.pnDevice.Controls.Add(this.btHomeEdge);
            this.pnDevice.Controls.Add(this.btHomeCenter);
            this.pnDevice.Controls.Add(this.btTest);
            this.pnDevice.Location = new System.Drawing.Point(3, 75);
            this.pnDevice.Name = "pnDevice";
            this.pnDevice.Size = new System.Drawing.Size(968, 405);
            this.pnDevice.TabIndex = 5;
            // 
            // tbResponse
            // 
            this.tbResponse.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tbResponse.Location = new System.Drawing.Point(690, 4);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.ReadOnly = true;
            this.tbResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResponse.Size = new System.Drawing.Size(271, 394);
            this.tbResponse.TabIndex = 24;
            // 
            // gbBtnCtrl
            // 
            this.gbBtnCtrl.Controls.Add(this.tbXAxis);
            this.gbBtnCtrl.Controls.Add(this.tbZAxis);
            this.gbBtnCtrl.Controls.Add(this.btZLeft);
            this.gbBtnCtrl.Controls.Add(this.label5);
            this.gbBtnCtrl.Controls.Add(this.btYDown);
            this.gbBtnCtrl.Controls.Add(this.tbYAxis);
            this.gbBtnCtrl.Controls.Add(this.btXLeft);
            this.gbBtnCtrl.Controls.Add(this.label4);
            this.gbBtnCtrl.Controls.Add(this.btXRight);
            this.gbBtnCtrl.Controls.Add(this.nudDistance);
            this.gbBtnCtrl.Controls.Add(this.label2);
            this.gbBtnCtrl.Controls.Add(this.label3);
            this.gbBtnCtrl.Controls.Add(this.btZRight);
            this.gbBtnCtrl.Controls.Add(this.nudVelocity);
            this.gbBtnCtrl.Controls.Add(this.label1);
            this.gbBtnCtrl.Controls.Add(this.btYUp);
            this.gbBtnCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbBtnCtrl.Location = new System.Drawing.Point(3, 72);
            this.gbBtnCtrl.Name = "gbBtnCtrl";
            this.gbBtnCtrl.Size = new System.Drawing.Size(685, 328);
            this.gbBtnCtrl.TabIndex = 6;
            this.gbBtnCtrl.TabStop = false;
            this.gbBtnCtrl.Text = "Button Control";
            // 
            // tbXAxis
            // 
            this.tbXAxis.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tbXAxis.Enabled = false;
            this.tbXAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbXAxis.Location = new System.Drawing.Point(122, 104);
            this.tbXAxis.Name = "tbXAxis";
            this.tbXAxis.Size = new System.Drawing.Size(283, 29);
            this.tbXAxis.TabIndex = 6;
            // 
            // tbZAxis
            // 
            this.tbZAxis.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tbZAxis.Enabled = false;
            this.tbZAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbZAxis.Location = new System.Drawing.Point(122, 184);
            this.tbZAxis.Name = "tbZAxis";
            this.tbZAxis.Size = new System.Drawing.Size(283, 29);
            this.tbZAxis.TabIndex = 7;
            // 
            // btZLeft
            // 
            this.btZLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btZLeft.Location = new System.Drawing.Point(439, 176);
            this.btZLeft.Name = "btZLeft";
            this.btZLeft.Size = new System.Drawing.Size(81, 50);
            this.btZLeft.TabIndex = 13;
            this.btZLeft.Text = "<<";
            this.btZLeft.UseVisualStyleBackColor = true;
            this.btZLeft.Click += new System.EventHandler(this.btZLeft_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 24);
            this.label5.TabIndex = 20;
            this.label5.Text = "Z Axis";
            // 
            // btYDown
            // 
            this.btYDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btYDown.Location = new System.Drawing.Point(439, 256);
            this.btYDown.Name = "btYDown";
            this.btYDown.Size = new System.Drawing.Size(81, 50);
            this.btYDown.TabIndex = 14;
            this.btYDown.Text = "Dn";
            this.btYDown.UseVisualStyleBackColor = true;
            this.btYDown.Click += new System.EventHandler(this.btYDown_Click);
            // 
            // tbYAxis
            // 
            this.tbYAxis.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tbYAxis.Enabled = false;
            this.tbYAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbYAxis.Location = new System.Drawing.Point(122, 264);
            this.tbYAxis.Name = "tbYAxis";
            this.tbYAxis.Size = new System.Drawing.Size(283, 29);
            this.tbYAxis.TabIndex = 8;
            // 
            // btXLeft
            // 
            this.btXLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btXLeft.Location = new System.Drawing.Point(439, 96);
            this.btXLeft.Name = "btXLeft";
            this.btXLeft.Size = new System.Drawing.Size(81, 50);
            this.btXLeft.TabIndex = 6;
            this.btXLeft.Text = "<<";
            this.btXLeft.UseVisualStyleBackColor = true;
            this.btXLeft.Click += new System.EventHandler(this.btXLeft_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 267);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 24);
            this.label4.TabIndex = 19;
            this.label4.Text = "Y Axis";
            // 
            // btXRight
            // 
            this.btXRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btXRight.Location = new System.Drawing.Point(567, 96);
            this.btXRight.Name = "btXRight";
            this.btXRight.Size = new System.Drawing.Size(81, 50);
            this.btXRight.TabIndex = 15;
            this.btXRight.Text = ">>";
            this.btXRight.UseVisualStyleBackColor = true;
            this.btXRight.Click += new System.EventHandler(this.btXRight_Click);
            // 
            // nudDistance
            // 
            this.nudDistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDistance.Location = new System.Drawing.Point(557, 37);
            this.nudDistance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudDistance.Name = "nudDistance";
            this.nudDistance.Size = new System.Drawing.Size(91, 29);
            this.nudDistance.TabIndex = 9;
            this.nudDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(96, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 24);
            this.label2.TabIndex = 12;
            this.label2.Text = "Steps/s (Velocity)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 24);
            this.label3.TabIndex = 18;
            this.label3.Text = "X Axis";
            // 
            // btZRight
            // 
            this.btZRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btZRight.Location = new System.Drawing.Point(567, 176);
            this.btZRight.Name = "btZRight";
            this.btZRight.Size = new System.Drawing.Size(81, 50);
            this.btZRight.TabIndex = 16;
            this.btZRight.Text = ">>";
            this.btZRight.UseVisualStyleBackColor = true;
            this.btZRight.Click += new System.EventHandler(this.btZRight_Click);
            // 
            // nudVelocity
            // 
            this.nudVelocity.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.nudVelocity.Enabled = false;
            this.nudVelocity.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudVelocity.Location = new System.Drawing.Point(256, 37);
            this.nudVelocity.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudVelocity.Name = "nudVelocity";
            this.nudVelocity.Size = new System.Drawing.Size(91, 29);
            this.nudVelocity.TabIndex = 10;
            this.nudVelocity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudVelocity.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(407, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 24);
            this.label1.TabIndex = 11;
            this.label1.Text = "Steps (Distance)";
            // 
            // btYUp
            // 
            this.btYUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btYUp.Location = new System.Drawing.Point(567, 256);
            this.btYUp.Name = "btYUp";
            this.btYUp.Size = new System.Drawing.Size(81, 50);
            this.btYUp.TabIndex = 17;
            this.btYUp.Text = "Up";
            this.btYUp.UseVisualStyleBackColor = true;
            this.btYUp.Click += new System.EventHandler(this.btYUp_Click);
            // 
            // gbMode
            // 
            this.gbMode.Controls.Add(this.rbJoystick);
            this.gbMode.Controls.Add(this.rbBtnCtrl);
            this.gbMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbMode.Location = new System.Drawing.Point(356, 16);
            this.gbMode.Name = "gbMode";
            this.gbMode.Size = new System.Drawing.Size(244, 50);
            this.gbMode.TabIndex = 23;
            this.gbMode.TabStop = false;
            this.gbMode.Text = "Control Mode";
            // 
            // rbJoystick
            // 
            this.rbJoystick.AutoSize = true;
            this.rbJoystick.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbJoystick.Location = new System.Drawing.Point(139, 16);
            this.rbJoystick.Name = "rbJoystick";
            this.rbJoystick.Size = new System.Drawing.Size(93, 28);
            this.rbJoystick.TabIndex = 24;
            this.rbJoystick.TabStop = true;
            this.rbJoystick.Text = "Joystick";
            this.rbJoystick.UseVisualStyleBackColor = true;
            // 
            // rbBtnCtrl
            // 
            this.rbBtnCtrl.AutoSize = true;
            this.rbBtnCtrl.Checked = true;
            this.rbBtnCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBtnCtrl.Location = new System.Drawing.Point(17, 16);
            this.rbBtnCtrl.Name = "rbBtnCtrl";
            this.rbBtnCtrl.Size = new System.Drawing.Size(113, 28);
            this.rbBtnCtrl.TabIndex = 23;
            this.rbBtnCtrl.TabStop = true;
            this.rbBtnCtrl.Text = "Button Ctrl";
            this.rbBtnCtrl.UseVisualStyleBackColor = true;
            this.rbBtnCtrl.CheckedChanged += new System.EventHandler(this.rbBtnCtrl_CheckedChanged);
            // 
            // btHomeEdge
            // 
            this.btHomeEdge.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btHomeEdge.Location = new System.Drawing.Point(141, 16);
            this.btHomeEdge.Name = "btHomeEdge";
            this.btHomeEdge.Size = new System.Drawing.Size(131, 50);
            this.btHomeEdge.TabIndex = 22;
            this.btHomeEdge.Text = "Home Edge";
            this.btHomeEdge.UseVisualStyleBackColor = true;
            this.btHomeEdge.Click += new System.EventHandler(this.btHomeEdge_Click);
            // 
            // btHomeCenter
            // 
            this.btHomeCenter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btHomeCenter.Location = new System.Drawing.Point(2, 16);
            this.btHomeCenter.Name = "btHomeCenter";
            this.btHomeCenter.Size = new System.Drawing.Size(131, 50);
            this.btHomeCenter.TabIndex = 21;
            this.btHomeCenter.Text = "Home Center";
            this.btHomeCenter.UseVisualStyleBackColor = true;
            this.btHomeCenter.Click += new System.EventHandler(this.btHomeCenter_Click);
            // 
            // btTest
            // 
            this.btTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btTest.Location = new System.Drawing.Point(631, 36);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(52, 24);
            this.btTest.TabIndex = 6;
            this.btTest.Text = "Test";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.otherToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(977, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serialTesterToolStripMenuItem});
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.otherToolStripMenuItem.Text = "Other";
            // 
            // serialTesterToolStripMenuItem
            // 
            this.serialTesterToolStripMenuItem.Name = "serialTesterToolStripMenuItem";
            this.serialTesterToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.serialTesterToolStripMenuItem.Text = "Serial Tester";
            this.serialTesterToolStripMenuItem.Click += new System.EventHandler(this.serialTesterToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(921, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 44);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // OpticalStageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 485);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pnDevice);
            this.Controls.Add(this.btRefresh);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.Controls.Add(this.tbSerial);
            this.Controls.Add(this.cbSerial);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "OpticalStageControl";
            this.Text = "Optical Stage Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnDevice.ResumeLayout(false);
            this.pnDevice.PerformLayout();
            this.gbBtnCtrl.ResumeLayout(false);
            this.gbBtnCtrl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVelocity)).EndInit();
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbSerial;
        private System.Windows.Forms.TextBox tbSerial;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.Button btRefresh;
        private System.Windows.Forms.Panel pnDevice;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btYUp;
        private System.Windows.Forms.Button btZRight;
        private System.Windows.Forms.Button btXRight;
        private System.Windows.Forms.Button btYDown;
        private System.Windows.Forms.Button btZLeft;
        private System.Windows.Forms.Button btXLeft;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudVelocity;
        private System.Windows.Forms.NumericUpDown nudDistance;
        private System.Windows.Forms.TextBox tbYAxis;
        private System.Windows.Forms.TextBox tbZAxis;
        private System.Windows.Forms.TextBox tbXAxis;
        private System.Windows.Forms.Button btTest;
        private System.Windows.Forms.RadioButton rbJoystick;
        private System.Windows.Forms.RadioButton rbBtnCtrl;
        private System.Windows.Forms.Button btHomeEdge;
        private System.Windows.Forms.Button btHomeCenter;
        private System.Windows.Forms.GroupBox gbBtnCtrl;
        private System.Windows.Forms.GroupBox gbMode;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serialTesterToolStripMenuItem;
        private System.Windows.Forms.TextBox tbResponse;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

