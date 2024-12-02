namespace GAMMA_GUI
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timerUIReadbacks = new System.Windows.Forms.Timer(this.components);
            this.timerUILaser = new System.Windows.Forms.Timer(this.components);
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panelLaser = new System.Windows.Forms.Panel();
            this.labelLasSeqTimeRemaining = new System.Windows.Forms.Label();
            this.textBoxLasONOFFLoops = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxLasOFFTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxLasONTime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxLasSeq = new System.Windows.Forms.CheckBox();
            this.textBoxLASPWF = new System.Windows.Forms.TextBox();
            this.buttonLASPWF = new System.Windows.Forms.Button();
            this.textBoxLASF = new System.Windows.Forms.TextBox();
            this.buttonLASF = new System.Windows.Forms.Button();
            this.textBoxLASPWP = new System.Windows.Forms.TextBox();
            this.textBoxLASDC = new System.Windows.Forms.TextBox();
            this.buttonLASPWP = new System.Windows.Forms.Button();
            this.buttonLASDC = new System.Windows.Forms.Button();
            this.comboBoxLaserMode = new System.Windows.Forms.ComboBox();
            this.formsPlotLasTemp = new ScottPlot.FormsPlot();
            this.buttonLasFan = new System.Windows.Forms.Button();
            this.pictureBoxLASER = new System.Windows.Forms.PictureBox();
            this.buttonLaserCurrentSetpointDecrease = new System.Windows.Forms.Button();
            this.buttonLaserCurrentLimitDecrease = new System.Windows.Forms.Button();
            this.buttonLaserCurrentSetpointIncrease = new System.Windows.Forms.Button();
            this.buttonLaserCurrentLimitIncrease = new System.Windows.Forms.Button();
            this.checkboxRampEnable = new System.Windows.Forms.CheckBox();
            this.labelReplyFromGPIB = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelLaserPower = new System.Windows.Forms.Label();
            this.buttonClearErrors = new System.Windows.Forms.Button();
            this.labelLaserTemperature = new System.Windows.Forms.Label();
            this.buttonCONN = new System.Windows.Forms.Button();
            this.richTextBoxERRORS = new System.Windows.Forms.RichTextBox();
            this.trackBarLaserCurrentSetpoint = new System.Windows.Forms.TrackBar();
            this.labelLaserCurrentSetpoint = new System.Windows.Forms.Label();
            this.buttonLaserCurrentSetpoint = new System.Windows.Forms.Button();
            this.labelLaserCurrentPhotoDiode = new System.Windows.Forms.Label();
            this.trackBarLaserCurrentLimit = new System.Windows.Forms.TrackBar();
            this.labelLaserCurrentLimit = new System.Windows.Forms.Label();
            this.buttonCurrentLimit = new System.Windows.Forms.Button();
            this.labelLaserERR = new System.Windows.Forms.Label();
            this.labelLasFanOn = new System.Windows.Forms.Label();
            this.buttonLaserOut = new System.Windows.Forms.Button();
            this.labelLaserMode = new System.Windows.Forms.Label();
            this.labelLaserOut = new System.Windows.Forms.Label();
            this.panelArduino = new System.Windows.Forms.Panel();
            this.labelFLIRTempMax = new System.Windows.Forms.Label();
            this.labelFLIRTempMin = new System.Windows.Forms.Label();
            this.labelFLIRTemp = new System.Windows.Forms.Label();
            this.buttonSaveFLIRImage = new System.Windows.Forms.Button();
            this.checkBoxMirror = new System.Windows.Forms.CheckBox();
            this.pictureBoxFLIR = new System.Windows.Forms.PictureBox();
            this.labelKu = new System.Windows.Forms.Label();
            this.textBoxKu = new System.Windows.Forms.TextBox();
            this.buttonKu = new System.Windows.Forms.Button();
            this.formsPlotArduino = new ScottPlot.FormsPlot();
            this.labelTC1 = new System.Windows.Forms.Label();
            this.trackBarTempSetpoint = new System.Windows.Forms.TrackBar();
            this.labelFanCurrent = new System.Windows.Forms.Label();
            this.labelPeltierCurrent = new System.Windows.Forms.Label();
            this.labelPumpPower = new System.Windows.Forms.Label();
            this.buttonPeltFan = new System.Windows.Forms.Button();
            this.labelTempSetpoint = new System.Windows.Forms.Label();
            this.buttonTempSetpoint = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panelTED = new System.Windows.Forms.Panel();
            this.formsPlotTED4015 = new ScottPlot.FormsPlot();
            this.labelTedTECCurrent = new System.Windows.Forms.Label();
            this.labelTedTECVoltage = new System.Windows.Forms.Label();
            this.labelTedTempSetpoint = new System.Windows.Forms.Label();
            this.labelTedTemp = new System.Windows.Forms.Label();
            this.buttonTECOut = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.labelWarningLight = new System.Windows.Forms.Label();
            this.labelINTLK = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.timerFLIR = new System.Windows.Forms.Timer(this.components);
            this.timerLaserSeq = new System.Windows.Forms.Timer(this.components);
            this.timerThingSpeak = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panelLaser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLASER)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLaserCurrentSetpoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLaserCurrentLimit)).BeginInit();
            this.panelArduino.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFLIR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTempSetpoint)).BeginInit();
            this.panelTED.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerUIReadbacks
            // 
            this.timerUIReadbacks.Tick += new System.EventHandler(this.timerUIReadbacks_Tick);
            // 
            // timerUILaser
            // 
            this.timerUILaser.Interval = 20;
            this.timerUILaser.Tick += new System.EventHandler(this.timerUILaser_Tick);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(1048, 701);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(136, 57);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 20;
            this.pictureBox3.TabStop = false;
            // 
            // panelLaser
            // 
            this.panelLaser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLaser.Controls.Add(this.labelLasSeqTimeRemaining);
            this.panelLaser.Controls.Add(this.textBoxLasONOFFLoops);
            this.panelLaser.Controls.Add(this.label10);
            this.panelLaser.Controls.Add(this.textBoxLasOFFTime);
            this.panelLaser.Controls.Add(this.label7);
            this.panelLaser.Controls.Add(this.textBoxLasONTime);
            this.panelLaser.Controls.Add(this.label6);
            this.panelLaser.Controls.Add(this.label5);
            this.panelLaser.Controls.Add(this.checkBoxLasSeq);
            this.panelLaser.Controls.Add(this.textBoxLASPWF);
            this.panelLaser.Controls.Add(this.buttonLASPWF);
            this.panelLaser.Controls.Add(this.textBoxLASF);
            this.panelLaser.Controls.Add(this.buttonLASF);
            this.panelLaser.Controls.Add(this.textBoxLASPWP);
            this.panelLaser.Controls.Add(this.textBoxLASDC);
            this.panelLaser.Controls.Add(this.buttonLASPWP);
            this.panelLaser.Controls.Add(this.buttonLASDC);
            this.panelLaser.Controls.Add(this.comboBoxLaserMode);
            this.panelLaser.Controls.Add(this.formsPlotLasTemp);
            this.panelLaser.Controls.Add(this.buttonLasFan);
            this.panelLaser.Controls.Add(this.pictureBoxLASER);
            this.panelLaser.Controls.Add(this.buttonLaserCurrentSetpointDecrease);
            this.panelLaser.Controls.Add(this.buttonLaserCurrentLimitDecrease);
            this.panelLaser.Controls.Add(this.buttonLaserCurrentSetpointIncrease);
            this.panelLaser.Controls.Add(this.buttonLaserCurrentLimitIncrease);
            this.panelLaser.Controls.Add(this.checkboxRampEnable);
            this.panelLaser.Controls.Add(this.labelReplyFromGPIB);
            this.panelLaser.Controls.Add(this.label2);
            this.panelLaser.Controls.Add(this.labelLaserPower);
            this.panelLaser.Controls.Add(this.buttonClearErrors);
            this.panelLaser.Controls.Add(this.labelLaserTemperature);
            this.panelLaser.Controls.Add(this.buttonCONN);
            this.panelLaser.Controls.Add(this.richTextBoxERRORS);
            this.panelLaser.Controls.Add(this.trackBarLaserCurrentSetpoint);
            this.panelLaser.Controls.Add(this.labelLaserCurrentSetpoint);
            this.panelLaser.Controls.Add(this.buttonLaserCurrentSetpoint);
            this.panelLaser.Controls.Add(this.labelLaserCurrentPhotoDiode);
            this.panelLaser.Controls.Add(this.trackBarLaserCurrentLimit);
            this.panelLaser.Controls.Add(this.labelLaserCurrentLimit);
            this.panelLaser.Controls.Add(this.buttonCurrentLimit);
            this.panelLaser.Controls.Add(this.labelLaserERR);
            this.panelLaser.Controls.Add(this.labelLasFanOn);
            this.panelLaser.Controls.Add(this.buttonLaserOut);
            this.panelLaser.Controls.Add(this.labelLaserMode);
            this.panelLaser.Controls.Add(this.labelLaserOut);
            this.panelLaser.Location = new System.Drawing.Point(12, 13);
            this.panelLaser.Name = "panelLaser";
            this.panelLaser.Size = new System.Drawing.Size(546, 727);
            this.panelLaser.TabIndex = 23;
            this.panelLaser.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLaser_Paint);
            // 
            // labelLasSeqTimeRemaining
            // 
            this.labelLasSeqTimeRemaining.AutoSize = true;
            this.labelLasSeqTimeRemaining.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLasSeqTimeRemaining.Location = new System.Drawing.Point(199, 554);
            this.labelLasSeqTimeRemaining.Name = "labelLasSeqTimeRemaining";
            this.labelLasSeqTimeRemaining.Size = new System.Drawing.Size(95, 19);
            this.labelLasSeqTimeRemaining.TabIndex = 71;
            this.labelLasSeqTimeRemaining.Text = "Seq Details";
            // 
            // textBoxLasONOFFLoops
            // 
            this.textBoxLasONOFFLoops.Location = new System.Drawing.Point(384, 507);
            this.textBoxLasONOFFLoops.Name = "textBoxLasONOFFLoops";
            this.textBoxLasONOFFLoops.Size = new System.Drawing.Size(88, 20);
            this.textBoxLasONOFFLoops.TabIndex = 70;
            this.textBoxLasONOFFLoops.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(382, 490);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 14);
            this.label10.TabIndex = 69;
            this.label10.Text = "Loops (0=infinite)";
            // 
            // textBoxLasOFFTime
            // 
            this.textBoxLasOFFTime.Location = new System.Drawing.Point(290, 507);
            this.textBoxLasOFFTime.Name = "textBoxLasOFFTime";
            this.textBoxLasOFFTime.Size = new System.Drawing.Size(88, 20);
            this.textBoxLasOFFTime.TabIndex = 67;
            this.textBoxLasOFFTime.Text = "180";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(310, 490);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 14);
            this.label7.TabIndex = 66;
            this.label7.Text = "TimeOFF (s)";
            // 
            // textBoxLasONTime
            // 
            this.textBoxLasONTime.Location = new System.Drawing.Point(191, 507);
            this.textBoxLasONTime.Name = "textBoxLasONTime";
            this.textBoxLasONTime.Size = new System.Drawing.Size(88, 20);
            this.textBoxLasONTime.TabIndex = 65;
            this.textBoxLasONTime.Text = "180";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(211, 490);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 14);
            this.label6.TabIndex = 64;
            this.label6.Text = "TimeON (s)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(199, 452);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(174, 24);
            this.label5.TabIndex = 63;
            this.label5.Text = "Laser Sequence";
            // 
            // checkBoxLasSeq
            // 
            this.checkBoxLasSeq.AutoSize = true;
            this.checkBoxLasSeq.Location = new System.Drawing.Point(85, 507);
            this.checkBoxLasSeq.Name = "checkBoxLasSeq";
            this.checkBoxLasSeq.Size = new System.Drawing.Size(83, 18);
            this.checkBoxLasSeq.TabIndex = 62;
            this.checkBoxLasSeq.Text = "Sequencing";
            this.checkBoxLasSeq.UseVisualStyleBackColor = true;
            this.checkBoxLasSeq.CheckedChanged += new System.EventHandler(this.checkBoxLasSeq_CheckedChanged);
            // 
            // textBoxLASPWF
            // 
            this.textBoxLASPWF.Enabled = false;
            this.textBoxLASPWF.Location = new System.Drawing.Point(146, 234);
            this.textBoxLASPWF.Name = "textBoxLASPWF";
            this.textBoxLASPWF.Size = new System.Drawing.Size(100, 20);
            this.textBoxLASPWF.TabIndex = 61;
            this.textBoxLASPWF.Visible = false;
            // 
            // buttonLASPWF
            // 
            this.buttonLASPWF.Enabled = false;
            this.buttonLASPWF.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLASPWF.Location = new System.Drawing.Point(245, 231);
            this.buttonLASPWF.Name = "buttonLASPWF";
            this.buttonLASPWF.Size = new System.Drawing.Size(102, 25);
            this.buttonLASPWF.TabIndex = 60;
            this.buttonLASPWF.Text = "buttonLASPWF";
            this.buttonLASPWF.UseVisualStyleBackColor = true;
            this.buttonLASPWF.Visible = false;
            this.buttonLASPWF.Click += new System.EventHandler(this.buttonLASPWF_Click);
            // 
            // textBoxLASF
            // 
            this.textBoxLASF.Enabled = false;
            this.textBoxLASF.Location = new System.Drawing.Point(146, 202);
            this.textBoxLASF.Name = "textBoxLASF";
            this.textBoxLASF.Size = new System.Drawing.Size(100, 20);
            this.textBoxLASF.TabIndex = 59;
            this.textBoxLASF.Visible = false;
            // 
            // buttonLASF
            // 
            this.buttonLASF.Enabled = false;
            this.buttonLASF.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLASF.Location = new System.Drawing.Point(245, 199);
            this.buttonLASF.Name = "buttonLASF";
            this.buttonLASF.Size = new System.Drawing.Size(83, 25);
            this.buttonLASF.TabIndex = 58;
            this.buttonLASF.Text = "buttonLASF";
            this.buttonLASF.UseVisualStyleBackColor = true;
            this.buttonLASF.Visible = false;
            this.buttonLASF.Click += new System.EventHandler(this.buttonLASF_Click);
            // 
            // textBoxLASPWP
            // 
            this.textBoxLASPWP.Enabled = false;
            this.textBoxLASPWP.Location = new System.Drawing.Point(366, 236);
            this.textBoxLASPWP.Name = "textBoxLASPWP";
            this.textBoxLASPWP.Size = new System.Drawing.Size(60, 20);
            this.textBoxLASPWP.TabIndex = 57;
            this.textBoxLASPWP.Visible = false;
            // 
            // textBoxLASDC
            // 
            this.textBoxLASDC.Enabled = false;
            this.textBoxLASDC.Location = new System.Drawing.Point(326, 204);
            this.textBoxLASDC.Name = "textBoxLASDC";
            this.textBoxLASDC.Size = new System.Drawing.Size(100, 20);
            this.textBoxLASDC.TabIndex = 56;
            this.textBoxLASDC.Visible = false;
            // 
            // buttonLASPWP
            // 
            this.buttonLASPWP.Enabled = false;
            this.buttonLASPWP.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLASPWP.Location = new System.Drawing.Point(432, 234);
            this.buttonLASPWP.Name = "buttonLASPWP";
            this.buttonLASPWP.Size = new System.Drawing.Size(99, 25);
            this.buttonLASPWP.TabIndex = 55;
            this.buttonLASPWP.Text = "buttonLASPWP";
            this.buttonLASPWP.UseVisualStyleBackColor = true;
            this.buttonLASPWP.Visible = false;
            this.buttonLASPWP.Click += new System.EventHandler(this.buttonLASPWP_Click);
            // 
            // buttonLASDC
            // 
            this.buttonLASDC.Enabled = false;
            this.buttonLASDC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLASDC.Location = new System.Drawing.Point(432, 202);
            this.buttonLASDC.Name = "buttonLASDC";
            this.buttonLASDC.Size = new System.Drawing.Size(99, 25);
            this.buttonLASDC.TabIndex = 54;
            this.buttonLASDC.Text = "buttonLASDC";
            this.buttonLASDC.UseVisualStyleBackColor = true;
            this.buttonLASDC.Visible = false;
            this.buttonLASDC.Click += new System.EventHandler(this.button3_Click);
            // 
            // comboBoxLaserMode
            // 
            this.comboBoxLaserMode.FormattingEnabled = true;
            this.comboBoxLaserMode.Items.AddRange(new object[] {
            "CW",
            "Hard-Pulse",
            "QCW-Pulse",
            "QCW-ExtTrig"});
            this.comboBoxLaserMode.Location = new System.Drawing.Point(171, 67);
            this.comboBoxLaserMode.Name = "comboBoxLaserMode";
            this.comboBoxLaserMode.Size = new System.Drawing.Size(75, 22);
            this.comboBoxLaserMode.TabIndex = 53;
            this.comboBoxLaserMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxLaserMode_SelectedIndexChanged);
            // 
            // formsPlotLasTemp
            // 
            this.formsPlotLasTemp.Location = new System.Drawing.Point(-1, 253);
            this.formsPlotLasTemp.Name = "formsPlotLasTemp";
            this.formsPlotLasTemp.Size = new System.Drawing.Size(532, 209);
            this.formsPlotLasTemp.TabIndex = 52;
            // 
            // buttonLasFan
            // 
            this.buttonLasFan.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLasFan.Location = new System.Drawing.Point(171, 91);
            this.buttonLasFan.Name = "buttonLasFan";
            this.buttonLasFan.Size = new System.Drawing.Size(75, 25);
            this.buttonLasFan.TabIndex = 46;
            this.buttonLasFan.Text = "buttonLasFan";
            this.buttonLasFan.UseVisualStyleBackColor = true;
            this.buttonLasFan.Click += new System.EventHandler(this.buttonLasFan_Click);
            // 
            // pictureBoxLASER
            // 
            this.pictureBoxLASER.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLASER.Image")));
            this.pictureBoxLASER.Location = new System.Drawing.Point(408, -1);
            this.pictureBoxLASER.Name = "pictureBoxLASER";
            this.pictureBoxLASER.Size = new System.Drawing.Size(138, 117);
            this.pictureBoxLASER.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLASER.TabIndex = 25;
            this.pictureBoxLASER.TabStop = false;
            // 
            // buttonLaserCurrentSetpointDecrease
            // 
            this.buttonLaserCurrentSetpointDecrease.Location = new System.Drawing.Point(252, 155);
            this.buttonLaserCurrentSetpointDecrease.Name = "buttonLaserCurrentSetpointDecrease";
            this.buttonLaserCurrentSetpointDecrease.Size = new System.Drawing.Size(21, 23);
            this.buttonLaserCurrentSetpointDecrease.TabIndex = 45;
            this.buttonLaserCurrentSetpointDecrease.Text = "-";
            this.buttonLaserCurrentSetpointDecrease.UseVisualStyleBackColor = true;
            this.buttonLaserCurrentSetpointDecrease.Click += new System.EventHandler(this.buttonLaserCurrentSetpointDecrease_Click);
            // 
            // buttonLaserCurrentLimitDecrease
            // 
            this.buttonLaserCurrentLimitDecrease.Location = new System.Drawing.Point(252, 123);
            this.buttonLaserCurrentLimitDecrease.Name = "buttonLaserCurrentLimitDecrease";
            this.buttonLaserCurrentLimitDecrease.Size = new System.Drawing.Size(21, 23);
            this.buttonLaserCurrentLimitDecrease.TabIndex = 44;
            this.buttonLaserCurrentLimitDecrease.Text = "-";
            this.buttonLaserCurrentLimitDecrease.UseVisualStyleBackColor = true;
            this.buttonLaserCurrentLimitDecrease.Click += new System.EventHandler(this.buttonLaserCurrentLimitDecrease_Click);
            // 
            // buttonLaserCurrentSetpointIncrease
            // 
            this.buttonLaserCurrentSetpointIncrease.Location = new System.Drawing.Point(379, 155);
            this.buttonLaserCurrentSetpointIncrease.Name = "buttonLaserCurrentSetpointIncrease";
            this.buttonLaserCurrentSetpointIncrease.Size = new System.Drawing.Size(21, 23);
            this.buttonLaserCurrentSetpointIncrease.TabIndex = 43;
            this.buttonLaserCurrentSetpointIncrease.Text = "+";
            this.buttonLaserCurrentSetpointIncrease.UseVisualStyleBackColor = true;
            this.buttonLaserCurrentSetpointIncrease.Click += new System.EventHandler(this.buttonLaserCurrentSetpointIncrease_Click);
            // 
            // buttonLaserCurrentLimitIncrease
            // 
            this.buttonLaserCurrentLimitIncrease.Location = new System.Drawing.Point(379, 123);
            this.buttonLaserCurrentLimitIncrease.Name = "buttonLaserCurrentLimitIncrease";
            this.buttonLaserCurrentLimitIncrease.Size = new System.Drawing.Size(21, 23);
            this.buttonLaserCurrentLimitIncrease.TabIndex = 42;
            this.buttonLaserCurrentLimitIncrease.Text = "+";
            this.buttonLaserCurrentLimitIncrease.UseVisualStyleBackColor = true;
            this.buttonLaserCurrentLimitIncrease.Click += new System.EventHandler(this.buttonLaserCurrentLimitIncrease_Click);
            // 
            // checkboxRampEnable
            // 
            this.checkboxRampEnable.AutoSize = true;
            this.checkboxRampEnable.Location = new System.Drawing.Point(408, 160);
            this.checkboxRampEnable.Name = "checkboxRampEnable";
            this.checkboxRampEnable.Size = new System.Drawing.Size(132, 18);
            this.checkboxRampEnable.TabIndex = 41;
            this.checkboxRampEnable.Text = "checkboxRampEnable";
            this.checkboxRampEnable.UseVisualStyleBackColor = true;
            // 
            // labelReplyFromGPIB
            // 
            this.labelReplyFromGPIB.AutoSize = true;
            this.labelReplyFromGPIB.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReplyFromGPIB.Location = new System.Drawing.Point(152, 605);
            this.labelReplyFromGPIB.Name = "labelReplyFromGPIB";
            this.labelReplyFromGPIB.Size = new System.Drawing.Size(103, 14);
            this.labelReplyFromGPIB.TabIndex = 25;
            this.labelReplyFromGPIB.Text = "labelReplyFromGPIB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(167, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 24);
            this.label2.TabIndex = 40;
            this.label2.Text = "LDX36000";
            // 
            // labelLaserPower
            // 
            this.labelLaserPower.AutoSize = true;
            this.labelLaserPower.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLaserPower.Location = new System.Drawing.Point(9, 213);
            this.labelLaserPower.Name = "labelLaserPower";
            this.labelLaserPower.Size = new System.Drawing.Size(89, 14);
            this.labelLaserPower.TabIndex = 38;
            this.labelLaserPower.Text = "labelLaserPower";
            // 
            // buttonClearErrors
            // 
            this.buttonClearErrors.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClearErrors.Location = new System.Drawing.Point(456, 594);
            this.buttonClearErrors.Name = "buttonClearErrors";
            this.buttonClearErrors.Size = new System.Drawing.Size(75, 25);
            this.buttonClearErrors.TabIndex = 37;
            this.buttonClearErrors.Text = "Clear Errors";
            this.buttonClearErrors.UseVisualStyleBackColor = true;
            this.buttonClearErrors.Click += new System.EventHandler(this.buttonClearErrors_Click);
            // 
            // labelLaserTemperature
            // 
            this.labelLaserTemperature.AutoSize = true;
            this.labelLaserTemperature.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLaserTemperature.ForeColor = System.Drawing.Color.Red;
            this.labelLaserTemperature.Location = new System.Drawing.Point(9, 190);
            this.labelLaserTemperature.Name = "labelLaserTemperature";
            this.labelLaserTemperature.Size = new System.Drawing.Size(117, 14);
            this.labelLaserTemperature.TabIndex = 36;
            this.labelLaserTemperature.Text = "labelLaserTemperature";
            // 
            // buttonCONN
            // 
            this.buttonCONN.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCONN.Location = new System.Drawing.Point(12, 14);
            this.buttonCONN.Name = "buttonCONN";
            this.buttonCONN.Size = new System.Drawing.Size(75, 25);
            this.buttonCONN.TabIndex = 35;
            this.buttonCONN.Text = "buttonCONN";
            this.buttonCONN.UseVisualStyleBackColor = true;
            this.buttonCONN.Click += new System.EventHandler(this.buttonCONN_Click);
            // 
            // richTextBoxERRORS
            // 
            this.richTextBoxERRORS.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxERRORS.Location = new System.Drawing.Point(9, 625);
            this.richTextBoxERRORS.Name = "richTextBoxERRORS";
            this.richTextBoxERRORS.Size = new System.Drawing.Size(522, 87);
            this.richTextBoxERRORS.TabIndex = 34;
            this.richTextBoxERRORS.Text = "";
            // 
            // trackBarLaserCurrentSetpoint
            // 
            this.trackBarLaserCurrentSetpoint.Location = new System.Drawing.Point(274, 155);
            this.trackBarLaserCurrentSetpoint.Maximum = 40;
            this.trackBarLaserCurrentSetpoint.Name = "trackBarLaserCurrentSetpoint";
            this.trackBarLaserCurrentSetpoint.Size = new System.Drawing.Size(104, 45);
            this.trackBarLaserCurrentSetpoint.TabIndex = 33;
            // 
            // labelLaserCurrentSetpoint
            // 
            this.labelLaserCurrentSetpoint.AutoSize = true;
            this.labelLaserCurrentSetpoint.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLaserCurrentSetpoint.Location = new System.Drawing.Point(9, 157);
            this.labelLaserCurrentSetpoint.Name = "labelLaserCurrentSetpoint";
            this.labelLaserCurrentSetpoint.Size = new System.Drawing.Size(132, 14);
            this.labelLaserCurrentSetpoint.TabIndex = 32;
            this.labelLaserCurrentSetpoint.Text = "labelLaserCurrentSetpoint";
            // 
            // buttonLaserCurrentSetpoint
            // 
            this.buttonLaserCurrentSetpoint.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLaserCurrentSetpoint.Location = new System.Drawing.Point(171, 153);
            this.buttonLaserCurrentSetpoint.Name = "buttonLaserCurrentSetpoint";
            this.buttonLaserCurrentSetpoint.Size = new System.Drawing.Size(75, 25);
            this.buttonLaserCurrentSetpoint.TabIndex = 31;
            this.buttonLaserCurrentSetpoint.Text = "buttonLaserCurrentSetpoint";
            this.buttonLaserCurrentSetpoint.UseVisualStyleBackColor = true;
            this.buttonLaserCurrentSetpoint.Click += new System.EventHandler(this.buttonLaserCurrentSetpoint_Click);
            // 
            // labelLaserCurrentPhotoDiode
            // 
            this.labelLaserCurrentPhotoDiode.AutoSize = true;
            this.labelLaserCurrentPhotoDiode.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLaserCurrentPhotoDiode.Location = new System.Drawing.Point(9, 232);
            this.labelLaserCurrentPhotoDiode.Name = "labelLaserCurrentPhotoDiode";
            this.labelLaserCurrentPhotoDiode.Size = new System.Drawing.Size(147, 14);
            this.labelLaserCurrentPhotoDiode.TabIndex = 30;
            this.labelLaserCurrentPhotoDiode.Text = "labelLaserCurrentPhotoDiode";
            // 
            // trackBarLaserCurrentLimit
            // 
            this.trackBarLaserCurrentLimit.Location = new System.Drawing.Point(274, 123);
            this.trackBarLaserCurrentLimit.Maximum = 40;
            this.trackBarLaserCurrentLimit.Name = "trackBarLaserCurrentLimit";
            this.trackBarLaserCurrentLimit.Size = new System.Drawing.Size(104, 45);
            this.trackBarLaserCurrentLimit.TabIndex = 29;
            // 
            // labelLaserCurrentLimit
            // 
            this.labelLaserCurrentLimit.AutoSize = true;
            this.labelLaserCurrentLimit.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLaserCurrentLimit.Location = new System.Drawing.Point(9, 125);
            this.labelLaserCurrentLimit.Name = "labelLaserCurrentLimit";
            this.labelLaserCurrentLimit.Size = new System.Drawing.Size(114, 14);
            this.labelLaserCurrentLimit.TabIndex = 28;
            this.labelLaserCurrentLimit.Text = "labelLaserCurrentLimit";
            // 
            // buttonCurrentLimit
            // 
            this.buttonCurrentLimit.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCurrentLimit.Location = new System.Drawing.Point(171, 121);
            this.buttonCurrentLimit.Name = "buttonCurrentLimit";
            this.buttonCurrentLimit.Size = new System.Drawing.Size(75, 25);
            this.buttonCurrentLimit.TabIndex = 27;
            this.buttonCurrentLimit.Text = "buttonCurrentLimit";
            this.buttonCurrentLimit.UseVisualStyleBackColor = true;
            this.buttonCurrentLimit.Click += new System.EventHandler(this.buttonCurrentLimit_Click);
            // 
            // labelLaserERR
            // 
            this.labelLaserERR.AutoSize = true;
            this.labelLaserERR.Location = new System.Drawing.Point(10, 605);
            this.labelLaserERR.Name = "labelLaserERR";
            this.labelLaserERR.Size = new System.Drawing.Size(52, 14);
            this.labelLaserERR.TabIndex = 26;
            this.labelLaserERR.Text = "ERRORS:";
            // 
            // labelLasFanOn
            // 
            this.labelLasFanOn.AutoSize = true;
            this.labelLasFanOn.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLasFanOn.Location = new System.Drawing.Point(9, 100);
            this.labelLasFanOn.Name = "labelLasFanOn";
            this.labelLasFanOn.Size = new System.Drawing.Size(79, 14);
            this.labelLasFanOn.TabIndex = 25;
            this.labelLasFanOn.Text = "labelLasFanOn";
            // 
            // buttonLaserOut
            // 
            this.buttonLaserOut.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLaserOut.Location = new System.Drawing.Point(171, 42);
            this.buttonLaserOut.Name = "buttonLaserOut";
            this.buttonLaserOut.Size = new System.Drawing.Size(75, 25);
            this.buttonLaserOut.TabIndex = 24;
            this.buttonLaserOut.Text = "buttonLaserOut";
            this.buttonLaserOut.UseVisualStyleBackColor = true;
            this.buttonLaserOut.Click += new System.EventHandler(this.buttonLaserOut_Click);
            // 
            // labelLaserMode
            // 
            this.labelLaserMode.AutoSize = true;
            this.labelLaserMode.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLaserMode.Location = new System.Drawing.Point(9, 75);
            this.labelLaserMode.Name = "labelLaserMode";
            this.labelLaserMode.Size = new System.Drawing.Size(83, 14);
            this.labelLaserMode.TabIndex = 23;
            this.labelLaserMode.Text = "labelLaserMode";
            // 
            // labelLaserOut
            // 
            this.labelLaserOut.AutoSize = true;
            this.labelLaserOut.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLaserOut.Location = new System.Drawing.Point(9, 49);
            this.labelLaserOut.Name = "labelLaserOut";
            this.labelLaserOut.Size = new System.Drawing.Size(74, 14);
            this.labelLaserOut.TabIndex = 22;
            this.labelLaserOut.Text = "labelLaserOut";
            // 
            // panelArduino
            // 
            this.panelArduino.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelArduino.Controls.Add(this.labelFLIRTempMax);
            this.panelArduino.Controls.Add(this.labelFLIRTempMin);
            this.panelArduino.Controls.Add(this.labelFLIRTemp);
            this.panelArduino.Controls.Add(this.buttonSaveFLIRImage);
            this.panelArduino.Controls.Add(this.checkBoxMirror);
            this.panelArduino.Controls.Add(this.pictureBoxFLIR);
            this.panelArduino.Controls.Add(this.labelKu);
            this.panelArduino.Controls.Add(this.textBoxKu);
            this.panelArduino.Controls.Add(this.buttonKu);
            this.panelArduino.Controls.Add(this.formsPlotArduino);
            this.panelArduino.Controls.Add(this.labelTC1);
            this.panelArduino.Controls.Add(this.trackBarTempSetpoint);
            this.panelArduino.Controls.Add(this.labelFanCurrent);
            this.panelArduino.Controls.Add(this.labelPeltierCurrent);
            this.panelArduino.Controls.Add(this.labelPumpPower);
            this.panelArduino.Controls.Add(this.buttonPeltFan);
            this.panelArduino.Controls.Add(this.labelTempSetpoint);
            this.panelArduino.Controls.Add(this.buttonTempSetpoint);
            this.panelArduino.Controls.Add(this.label3);
            this.panelArduino.Location = new System.Drawing.Point(567, 218);
            this.panelArduino.Name = "panelArduino";
            this.panelArduino.Size = new System.Drawing.Size(653, 424);
            this.panelArduino.TabIndex = 24;
            // 
            // labelFLIRTempMax
            // 
            this.labelFLIRTempMax.AutoSize = true;
            this.labelFLIRTempMax.Location = new System.Drawing.Point(262, 107);
            this.labelFLIRTempMax.Name = "labelFLIRTempMax";
            this.labelFLIRTempMax.Size = new System.Drawing.Size(95, 14);
            this.labelFLIRTempMax.TabIndex = 64;
            this.labelFLIRTempMax.Text = "labelFLIRTempMax";
            // 
            // labelFLIRTempMin
            // 
            this.labelFLIRTempMin.AutoSize = true;
            this.labelFLIRTempMin.Location = new System.Drawing.Point(262, 85);
            this.labelFLIRTempMin.Name = "labelFLIRTempMin";
            this.labelFLIRTempMin.Size = new System.Drawing.Size(91, 14);
            this.labelFLIRTempMin.TabIndex = 63;
            this.labelFLIRTempMin.Text = "labelFLIRTempMin";
            // 
            // labelFLIRTemp
            // 
            this.labelFLIRTemp.AutoSize = true;
            this.labelFLIRTemp.Location = new System.Drawing.Point(262, 62);
            this.labelFLIRTemp.Name = "labelFLIRTemp";
            this.labelFLIRTemp.Size = new System.Drawing.Size(75, 14);
            this.labelFLIRTemp.TabIndex = 62;
            this.labelFLIRTemp.Text = "labelFLIRTemp";
            // 
            // buttonSaveFLIRImage
            // 
            this.buttonSaveFLIRImage.Location = new System.Drawing.Point(270, 36);
            this.buttonSaveFLIRImage.Name = "buttonSaveFLIRImage";
            this.buttonSaveFLIRImage.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveFLIRImage.TabIndex = 61;
            this.buttonSaveFLIRImage.Text = "Save image";
            this.buttonSaveFLIRImage.UseVisualStyleBackColor = true;
            this.buttonSaveFLIRImage.Click += new System.EventHandler(this.buttonSaveFLIRImage_Click);
            // 
            // checkBoxMirror
            // 
            this.checkBoxMirror.AutoSize = true;
            this.checkBoxMirror.Location = new System.Drawing.Point(291, 18);
            this.checkBoxMirror.Name = "checkBoxMirror";
            this.checkBoxMirror.Size = new System.Drawing.Size(54, 18);
            this.checkBoxMirror.TabIndex = 53;
            this.checkBoxMirror.Text = "Mirror";
            this.checkBoxMirror.UseVisualStyleBackColor = true;
            // 
            // pictureBoxFLIR
            // 
            this.pictureBoxFLIR.Location = new System.Drawing.Point(364, 12);
            this.pictureBoxFLIR.Name = "pictureBoxFLIR";
            this.pictureBoxFLIR.Size = new System.Drawing.Size(270, 182);
            this.pictureBoxFLIR.TabIndex = 56;
            this.pictureBoxFLIR.TabStop = false;
            this.pictureBoxFLIR.Click += new System.EventHandler(this.pictureBoxFLIR_Click);
            // 
            // labelKu
            // 
            this.labelKu.AutoSize = true;
            this.labelKu.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKu.Location = new System.Drawing.Point(140, 178);
            this.labelKu.Name = "labelKu";
            this.labelKu.Size = new System.Drawing.Size(42, 14);
            this.labelKu.TabIndex = 55;
            this.labelKu.Text = "labelKu";
            // 
            // textBoxKu
            // 
            this.textBoxKu.Location = new System.Drawing.Point(97, 174);
            this.textBoxKu.Name = "textBoxKu";
            this.textBoxKu.Size = new System.Drawing.Size(37, 20);
            this.textBoxKu.TabIndex = 54;
            // 
            // buttonKu
            // 
            this.buttonKu.Location = new System.Drawing.Point(16, 174);
            this.buttonKu.Name = "buttonKu";
            this.buttonKu.Size = new System.Drawing.Size(75, 23);
            this.buttonKu.TabIndex = 53;
            this.buttonKu.Text = "Ku";
            this.buttonKu.UseVisualStyleBackColor = true;
            this.buttonKu.Click += new System.EventHandler(this.buttonKu_Click);
            // 
            // formsPlotArduino
            // 
            this.formsPlotArduino.Location = new System.Drawing.Point(16, 203);
            this.formsPlotArduino.Name = "formsPlotArduino";
            this.formsPlotArduino.Size = new System.Drawing.Size(618, 211);
            this.formsPlotArduino.TabIndex = 52;
            this.formsPlotArduino.MouseEnter += new System.EventHandler(this.formsPlotArduino_MouseEnter);
            this.formsPlotArduino.MouseLeave += new System.EventHandler(this.formsPlotArduino_MouseLeave);
            this.formsPlotArduino.MouseHover += new System.EventHandler(this.formsPlotArduino_MouseHover);
            // 
            // labelTC1
            // 
            this.labelTC1.AutoSize = true;
            this.labelTC1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTC1.ForeColor = System.Drawing.Color.Red;
            this.labelTC1.Location = new System.Drawing.Point(112, 36);
            this.labelTC1.Name = "labelTC1";
            this.labelTC1.Size = new System.Drawing.Size(48, 14);
            this.labelTC1.TabIndex = 50;
            this.labelTC1.Text = "labelTC1";
            // 
            // trackBarTempSetpoint
            // 
            this.trackBarTempSetpoint.Location = new System.Drawing.Point(97, 122);
            this.trackBarTempSetpoint.Maximum = 60;
            this.trackBarTempSetpoint.Name = "trackBarTempSetpoint";
            this.trackBarTempSetpoint.Size = new System.Drawing.Size(61, 45);
            this.trackBarTempSetpoint.TabIndex = 46;
            this.trackBarTempSetpoint.Value = 30;
            // 
            // labelFanCurrent
            // 
            this.labelFanCurrent.AutoSize = true;
            this.labelFanCurrent.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFanCurrent.Location = new System.Drawing.Point(13, 98);
            this.labelFanCurrent.Name = "labelFanCurrent";
            this.labelFanCurrent.Size = new System.Drawing.Size(83, 14);
            this.labelFanCurrent.TabIndex = 48;
            this.labelFanCurrent.Text = "labelFanCurrent";
            // 
            // labelPeltierCurrent
            // 
            this.labelPeltierCurrent.AutoSize = true;
            this.labelPeltierCurrent.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPeltierCurrent.ForeColor = System.Drawing.Color.Orange;
            this.labelPeltierCurrent.Location = new System.Drawing.Point(13, 75);
            this.labelPeltierCurrent.Name = "labelPeltierCurrent";
            this.labelPeltierCurrent.Size = new System.Drawing.Size(94, 14);
            this.labelPeltierCurrent.TabIndex = 47;
            this.labelPeltierCurrent.Text = "labelPeltierCurrent";
            // 
            // labelPumpPower
            // 
            this.labelPumpPower.AutoSize = true;
            this.labelPumpPower.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPumpPower.Location = new System.Drawing.Point(13, 54);
            this.labelPumpPower.Name = "labelPumpPower";
            this.labelPumpPower.Size = new System.Drawing.Size(87, 14);
            this.labelPumpPower.TabIndex = 46;
            this.labelPumpPower.Text = "labelPumpPower";
            // 
            // buttonPeltFan
            // 
            this.buttonPeltFan.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPeltFan.Location = new System.Drawing.Point(16, 142);
            this.buttonPeltFan.Name = "buttonPeltFan";
            this.buttonPeltFan.Size = new System.Drawing.Size(75, 25);
            this.buttonPeltFan.TabIndex = 45;
            this.buttonPeltFan.Text = "buttonPeltFan";
            this.buttonPeltFan.UseVisualStyleBackColor = true;
            this.buttonPeltFan.Click += new System.EventHandler(this.buttonPeltFan_Click);
            // 
            // labelTempSetpoint
            // 
            this.labelTempSetpoint.AutoSize = true;
            this.labelTempSetpoint.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTempSetpoint.ForeColor = System.Drawing.Color.Blue;
            this.labelTempSetpoint.Location = new System.Drawing.Point(13, 36);
            this.labelTempSetpoint.Name = "labelTempSetpoint";
            this.labelTempSetpoint.Size = new System.Drawing.Size(93, 14);
            this.labelTempSetpoint.TabIndex = 43;
            this.labelTempSetpoint.Text = "labelTempSetpoint";
            // 
            // buttonTempSetpoint
            // 
            this.buttonTempSetpoint.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTempSetpoint.Location = new System.Drawing.Point(16, 115);
            this.buttonTempSetpoint.Name = "buttonTempSetpoint";
            this.buttonTempSetpoint.Size = new System.Drawing.Size(75, 25);
            this.buttonTempSetpoint.TabIndex = 42;
            this.buttonTempSetpoint.Text = "buttonTempSetpoint";
            this.buttonTempSetpoint.UseVisualStyleBackColor = true;
            this.buttonTempSetpoint.Click += new System.EventHandler(this.buttonTempSetpoint_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 24);
            this.label3.TabIndex = 41;
            this.label3.Text = "WaterCooling";
            // 
            // panelTED
            // 
            this.panelTED.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelTED.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTED.Controls.Add(this.formsPlotTED4015);
            this.panelTED.Controls.Add(this.labelTedTECCurrent);
            this.panelTED.Controls.Add(this.labelTedTECVoltage);
            this.panelTED.Controls.Add(this.labelTedTempSetpoint);
            this.panelTED.Controls.Add(this.labelTedTemp);
            this.panelTED.Controls.Add(this.buttonTECOut);
            this.panelTED.Controls.Add(this.label8);
            this.panelTED.Location = new System.Drawing.Point(567, 13);
            this.panelTED.Name = "panelTED";
            this.panelTED.Size = new System.Drawing.Size(662, 200);
            this.panelTED.TabIndex = 50;
            // 
            // formsPlotTED4015
            // 
            this.formsPlotTED4015.Location = new System.Drawing.Point(129, -1);
            this.formsPlotTED4015.Name = "formsPlotTED4015";
            this.formsPlotTED4015.Size = new System.Drawing.Size(523, 172);
            this.formsPlotTED4015.TabIndex = 51;
            // 
            // labelTedTECCurrent
            // 
            this.labelTedTECCurrent.AutoSize = true;
            this.labelTedTECCurrent.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTedTECCurrent.Location = new System.Drawing.Point(13, 120);
            this.labelTedTECCurrent.Name = "labelTedTECCurrent";
            this.labelTedTECCurrent.Size = new System.Drawing.Size(101, 14);
            this.labelTedTECCurrent.TabIndex = 48;
            this.labelTedTECCurrent.Text = "labelTedTECCurrent";
            // 
            // labelTedTECVoltage
            // 
            this.labelTedTECVoltage.AutoSize = true;
            this.labelTedTECVoltage.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTedTECVoltage.Location = new System.Drawing.Point(13, 140);
            this.labelTedTECVoltage.Name = "labelTedTECVoltage";
            this.labelTedTECVoltage.Size = new System.Drawing.Size(101, 14);
            this.labelTedTECVoltage.TabIndex = 47;
            this.labelTedTECVoltage.Text = "labelTedTECVoltage";
            // 
            // labelTedTempSetpoint
            // 
            this.labelTedTempSetpoint.AutoSize = true;
            this.labelTedTempSetpoint.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTedTempSetpoint.Location = new System.Drawing.Point(13, 81);
            this.labelTedTempSetpoint.Name = "labelTedTempSetpoint";
            this.labelTedTempSetpoint.Size = new System.Drawing.Size(110, 14);
            this.labelTedTempSetpoint.TabIndex = 46;
            this.labelTedTempSetpoint.Text = "labelTedTempSetpoint";
            // 
            // labelTedTemp
            // 
            this.labelTedTemp.AutoSize = true;
            this.labelTedTemp.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTedTemp.ForeColor = System.Drawing.Color.Red;
            this.labelTedTemp.Location = new System.Drawing.Point(13, 101);
            this.labelTedTemp.Name = "labelTedTemp";
            this.labelTedTemp.Size = new System.Drawing.Size(71, 14);
            this.labelTedTemp.TabIndex = 43;
            this.labelTedTemp.Text = "labelTedTemp";
            // 
            // buttonTECOut
            // 
            this.buttonTECOut.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTECOut.Location = new System.Drawing.Point(16, 44);
            this.buttonTECOut.Name = "buttonTECOut";
            this.buttonTECOut.Size = new System.Drawing.Size(98, 25);
            this.buttonTECOut.TabIndex = 42;
            this.buttonTECOut.Text = "buttonTECOut";
            this.buttonTECOut.UseVisualStyleBackColor = true;
            this.buttonTECOut.Click += new System.EventHandler(this.buttonTECOut_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 24);
            this.label8.TabIndex = 41;
            this.label8.Text = "TED4015";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.labelWarningLight);
            this.panel1.Controls.Add(this.labelINTLK);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Location = new System.Drawing.Point(567, 648);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(653, 62);
            this.panel1.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(203, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 14);
            this.label4.TabIndex = 55;
            this.label4.Text = "label4";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(97, 174);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 54;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 53;
            this.button1.Text = "Ku";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // labelWarningLight
            // 
            this.labelWarningLight.AutoSize = true;
            this.labelWarningLight.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWarningLight.Location = new System.Drawing.Point(13, 35);
            this.labelWarningLight.Name = "labelWarningLight";
            this.labelWarningLight.Size = new System.Drawing.Size(92, 14);
            this.labelWarningLight.TabIndex = 46;
            this.labelWarningLight.Text = "labelWarningLight";
            // 
            // labelINTLK
            // 
            this.labelINTLK.AutoSize = true;
            this.labelINTLK.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelINTLK.ForeColor = System.Drawing.Color.Blue;
            this.labelINTLK.Location = new System.Drawing.Point(188, 35);
            this.labelINTLK.Name = "labelINTLK";
            this.labelINTLK.Size = new System.Drawing.Size(57, 14);
            this.labelINTLK.TabIndex = 43;
            this.labelINTLK.Text = "labelINTLK";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(12, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 24);
            this.label11.TabIndex = 41;
            this.label11.Text = "Safety";
            // 
            // timerFLIR
            // 
            this.timerFLIR.Tick += new System.EventHandler(this.timerFLIR_Tick);
            // 
            // timerLaserSeq
            // 
            this.timerLaserSeq.Interval = 180000;
            this.timerLaserSeq.Tick += new System.EventHandler(this.timerLaserSeq_Tick);
            // 
            // timerThingSpeak
            // 
            this.timerThingSpeak.Interval = 15000;
            this.timerThingSpeak.Tick += new System.EventHandler(this.timerThingSpeak_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1232, 752);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTED);
            this.Controls.Add(this.panelArduino);
            this.Controls.Add(this.panelLaser);
            this.Controls.Add(this.pictureBox3);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "GAMMA-MRI-GUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panelLaser.ResumeLayout(false);
            this.panelLaser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLASER)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLaserCurrentSetpoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLaserCurrentLimit)).EndInit();
            this.panelArduino.ResumeLayout(false);
            this.panelArduino.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFLIR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTempSetpoint)).EndInit();
            this.panelTED.ResumeLayout(false);
            this.panelTED.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerUIReadbacks;
        private System.Windows.Forms.Timer timerUILaser;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel panelLaser;
        private System.Windows.Forms.Label labelLaserPower;
        private System.Windows.Forms.Button buttonClearErrors;
        private System.Windows.Forms.Label labelLaserTemperature;
        private System.Windows.Forms.Button buttonCONN;
        private System.Windows.Forms.RichTextBox richTextBoxERRORS;
        private System.Windows.Forms.TrackBar trackBarLaserCurrentSetpoint;
        private System.Windows.Forms.Label labelLaserCurrentSetpoint;
        private System.Windows.Forms.Button buttonLaserCurrentSetpoint;
        private System.Windows.Forms.Label labelLaserCurrentPhotoDiode;
        private System.Windows.Forms.TrackBar trackBarLaserCurrentLimit;
        private System.Windows.Forms.Label labelLaserCurrentLimit;
        private System.Windows.Forms.Button buttonCurrentLimit;
        private System.Windows.Forms.Label labelLaserERR;
        private System.Windows.Forms.Label labelLasFanOn;
        private System.Windows.Forms.Button buttonLaserOut;
        private System.Windows.Forms.Label labelLaserMode;
        private System.Windows.Forms.Label labelLaserOut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelArduino;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelReplyFromGPIB;
        private System.Windows.Forms.Label labelFanCurrent;
        private System.Windows.Forms.Label labelPeltierCurrent;
        private System.Windows.Forms.Label labelPumpPower;
        private System.Windows.Forms.Button buttonPeltFan;
        private System.Windows.Forms.Label labelTempSetpoint;
        private System.Windows.Forms.Button buttonTempSetpoint;
        private System.Windows.Forms.CheckBox checkboxRampEnable;
        private System.Windows.Forms.Button buttonLaserCurrentSetpointDecrease;
        private System.Windows.Forms.Button buttonLaserCurrentLimitDecrease;
        private System.Windows.Forms.Button buttonLaserCurrentSetpointIncrease;
        private System.Windows.Forms.Button buttonLaserCurrentLimitIncrease;
        private System.Windows.Forms.PictureBox pictureBoxLASER;
        private System.Windows.Forms.Panel panelTED;
        private System.Windows.Forms.Label labelTedTECCurrent;
        private System.Windows.Forms.Label labelTedTECVoltage;
        private System.Windows.Forms.Label labelTedTempSetpoint;
        private System.Windows.Forms.Label labelTedTemp;
        private System.Windows.Forms.Button buttonTECOut;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar trackBarTempSetpoint;
        private ScottPlot.FormsPlot formsPlotTED4015;
        private System.Windows.Forms.Button buttonLasFan;
        private System.Windows.Forms.Label labelTC1;
        private ScottPlot.FormsPlot formsPlotArduino;
        private ScottPlot.FormsPlot formsPlotLasTemp;
        private System.Windows.Forms.TextBox textBoxKu;
        private System.Windows.Forms.Button buttonKu;
        private System.Windows.Forms.Label labelKu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelWarningLight;
        private System.Windows.Forms.Label labelINTLK;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Timer timerFLIR;
        private System.Windows.Forms.PictureBox pictureBoxFLIR;
        private System.Windows.Forms.CheckBox checkBoxMirror;
        private System.Windows.Forms.Button buttonSaveFLIRImage;
        private System.Windows.Forms.Label labelFLIRTemp;
        private System.Windows.Forms.Label labelFLIRTempMax;
        private System.Windows.Forms.Label labelFLIRTempMin;
        private System.Windows.Forms.ComboBox comboBoxLaserMode;
        private System.Windows.Forms.TextBox textBoxLASPWP;
        private System.Windows.Forms.TextBox textBoxLASDC;
        private System.Windows.Forms.Button buttonLASPWP;
        private System.Windows.Forms.Button buttonLASDC;
        private System.Windows.Forms.TextBox textBoxLASF;
        private System.Windows.Forms.Button buttonLASF;
        private System.Windows.Forms.TextBox textBoxLASPWF;
        private System.Windows.Forms.Button buttonLASPWF;
        private System.Windows.Forms.TextBox textBoxLasONTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxLasSeq;
        private System.Windows.Forms.Timer timerLaserSeq;
        private System.Windows.Forms.TextBox textBoxLasONOFFLoops;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxLasOFFTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelLasSeqTimeRemaining;
        private System.Windows.Forms.Timer timerThingSpeak;
    }
}

