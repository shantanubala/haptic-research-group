namespace HapticGUI
{
    partial class GUI
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
            this.ModeComboBox = new System.Windows.Forms.ComboBox();
            this.SelectModeLabel = new System.Windows.Forms.Label();
            this.ModeGo = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.RhythmComboBox = new System.Windows.Forms.ComboBox();
            this.RhythmLearn = new System.Windows.Forms.Button();
            this.RhythmBack = new System.Windows.Forms.Button();
            this.RhythmTest = new System.Windows.Forms.Button();
            this.RhythmPatternList = new System.Windows.Forms.ListBox();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.ClosePort = new System.Windows.Forms.Button();
            this.OpenPort = new System.Windows.Forms.Button();
            this.RefreshPorts = new System.Windows.Forms.Button();
            this.COMComboBox = new System.Windows.Forms.ComboBox();
            this.SelectPortLabel = new System.Windows.Forms.Label();
            this.MagPanel = new System.Windows.Forms.Panel();
            this.MagTestStop = new System.Windows.Forms.Button();
            this.DutyLabel = new System.Windows.Forms.Label();
            this.DutyCycle = new System.Windows.Forms.NumericUpDown();
            this.Period = new System.Windows.Forms.NumericUpDown();
            this.PeriodLabel = new System.Windows.Forms.Label();
            this.PeriodDefaultLabel = new System.Windows.Forms.Label();
            this.MagOption = new System.Windows.Forms.CheckBox();
            this.Percentage = new System.Windows.Forms.NumericUpDown();
            this.PercentLabel = new System.Windows.Forms.Label();
            this.MagTest = new System.Windows.Forms.Button();
            this.MagBack = new System.Windows.Forms.Button();
            this.MagLearn = new System.Windows.Forms.Button();
            this.MagComboBox = new System.Windows.Forms.ComboBox();
            this.MagSelLabel = new System.Windows.Forms.Label();
            this.RhythmPanel = new System.Windows.Forms.Panel();
            this.RhythmTestStop = new System.Windows.Forms.Button();
            this.RhythmTime = new System.Windows.Forms.Label();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.RhythmClear = new System.Windows.Forms.Button();
            this.RhythmPaint = new System.Windows.Forms.Panel();
            this.RhythmDelete = new System.Windows.Forms.Button();
            this.RhythmReplace = new System.Windows.Forms.Button();
            this.RhythmInsert = new System.Windows.Forms.Button();
            this.RhythmAdd = new System.Windows.Forms.Button();
            this.RhythmOffLabel = new System.Windows.Forms.Label();
            this.RhythmOnLabel = new System.Windows.Forms.Label();
            this.RhythmOff = new System.Windows.Forms.NumericUpDown();
            this.RhythmOn = new System.Windows.Forms.NumericUpDown();
            this.RhythmLabel = new System.Windows.Forms.Label();
            this.DirectPanel = new System.Windows.Forms.Panel();
            this.DirectSave = new System.Windows.Forms.Button();
            this.DirectLoad = new System.Windows.Forms.Button();
            this.DirectRenameLabel = new System.Windows.Forms.Label();
            this.DirectActivateMotor = new System.Windows.Forms.Button();
            this.DirectActivateGroup = new System.Windows.Forms.Button();
            this.DirectGroupLabel = new System.Windows.Forms.Label();
            this.DirectAddGroup = new System.Windows.Forms.Button();
            this.DirectDeleteGroup = new System.Windows.Forms.Button();
            this.DirectClearGroup = new System.Windows.Forms.Button();
            this.DirectClearSet = new System.Windows.Forms.Button();
            this.DirectDeleteSet = new System.Windows.Forms.Button();
            this.DirectAddSet = new System.Windows.Forms.Button();
            this.GroupList = new System.Windows.Forms.ListBox();
            this.DirectRenameGroup = new System.Windows.Forms.Button();
            this.DirectBack = new System.Windows.Forms.Button();
            this.DirectRenameField = new System.Windows.Forms.TextBox();
            this.DirectRenameSet = new System.Windows.Forms.Button();
            this.DirectStop = new System.Windows.Forms.Button();
            this.DirectSetLabel = new System.Windows.Forms.Label();
            this.AddedCycleLabel = new System.Windows.Forms.Label();
            this.AddedMagLabel = new System.Windows.Forms.Label();
            this.AddedRhythmLabel = new System.Windows.Forms.Label();
            this.DirectClearMotor = new System.Windows.Forms.Button();
            this.DirectActivateSet = new System.Windows.Forms.Button();
            this.DirectDeleteMotor = new System.Windows.Forms.Button();
            this.DirectAddMotor = new System.Windows.Forms.Button();
            this.DirectCyclesLabel = new System.Windows.Forms.Label();
            this.DirectMagLabel = new System.Windows.Forms.Label();
            this.DirectRhythmLabel = new System.Windows.Forms.Label();
            this.DirectCyclesBox = new System.Windows.Forms.ComboBox();
            this.DirectMagBox = new System.Windows.Forms.ComboBox();
            this.DirectRhythmBox = new System.Windows.Forms.ComboBox();
            this.DirectMotorLabel = new System.Windows.Forms.Label();
            this.SetList = new System.Windows.Forms.ListBox();
            this.AddedList = new System.Windows.Forms.ListBox();
            this.AvailableList = new System.Windows.Forms.ListBox();
            this.ErrorStatus = new System.Windows.Forms.Label();
            this.ErrorLocation = new System.Windows.Forms.Label();
            this.MainPanel.SuspendLayout();
            this.MagPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DutyCycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Period)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Percentage)).BeginInit();
            this.RhythmPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RhythmOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RhythmOn)).BeginInit();
            this.DirectPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ModeComboBox
            // 
            this.ModeComboBox.FormattingEnabled = true;
            this.ModeComboBox.Items.AddRange(new object[] {
            "Rhythm Mode",
            "Magnitude Mode",
            "Direct Operation Mode"});
            this.ModeComboBox.Location = new System.Drawing.Point(99, 16);
            this.ModeComboBox.Name = "ModeComboBox";
            this.ModeComboBox.Size = new System.Drawing.Size(156, 21);
            this.ModeComboBox.TabIndex = 1;
            this.ModeComboBox.Text = "Rhythm Mode";
            // 
            // SelectModeLabel
            // 
            this.SelectModeLabel.AutoSize = true;
            this.SelectModeLabel.BackColor = System.Drawing.SystemColors.Control;
            this.SelectModeLabel.Location = new System.Drawing.Point(13, 19);
            this.SelectModeLabel.Name = "SelectModeLabel";
            this.SelectModeLabel.Size = new System.Drawing.Size(67, 13);
            this.SelectModeLabel.TabIndex = 1;
            this.SelectModeLabel.Text = "Select Mode";
            // 
            // ModeGo
            // 
            this.ModeGo.Location = new System.Drawing.Point(180, 222);
            this.ModeGo.Name = "ModeGo";
            this.ModeGo.Size = new System.Drawing.Size(75, 23);
            this.ModeGo.TabIndex = 7;
            this.ModeGo.Text = "Go";
            this.ModeGo.UseVisualStyleBackColor = true;
            this.ModeGo.Click += new System.EventHandler(this.ModeGo_Click);
            // 
            // Exit
            // 
            this.Exit.Location = new System.Drawing.Point(16, 222);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 23);
            this.Exit.TabIndex = 6;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // RhythmComboBox
            // 
            this.RhythmComboBox.FormattingEnabled = true;
            this.RhythmComboBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E"});
            this.RhythmComboBox.Location = new System.Drawing.Point(95, 16);
            this.RhythmComboBox.Name = "RhythmComboBox";
            this.RhythmComboBox.Size = new System.Drawing.Size(78, 21);
            this.RhythmComboBox.TabIndex = 1;
            this.RhythmComboBox.Text = "A";
            // 
            // RhythmLearn
            // 
            this.RhythmLearn.Location = new System.Drawing.Point(180, 222);
            this.RhythmLearn.Name = "RhythmLearn";
            this.RhythmLearn.Size = new System.Drawing.Size(75, 23);
            this.RhythmLearn.TabIndex = 12;
            this.RhythmLearn.Text = "Learn";
            this.RhythmLearn.UseVisualStyleBackColor = true;
            this.RhythmLearn.Click += new System.EventHandler(this.RhythmLearn_Click);
            // 
            // RhythmBack
            // 
            this.RhythmBack.Location = new System.Drawing.Point(16, 222);
            this.RhythmBack.Name = "RhythmBack";
            this.RhythmBack.Size = new System.Drawing.Size(75, 23);
            this.RhythmBack.TabIndex = 10;
            this.RhythmBack.Text = "Back";
            this.RhythmBack.UseVisualStyleBackColor = true;
            this.RhythmBack.Click += new System.EventHandler(this.RhythmBack_Click);
            // 
            // RhythmTest
            // 
            this.RhythmTest.Location = new System.Drawing.Point(98, 222);
            this.RhythmTest.Name = "RhythmTest";
            this.RhythmTest.Size = new System.Drawing.Size(75, 23);
            this.RhythmTest.TabIndex = 11;
            this.RhythmTest.Text = "Test";
            this.RhythmTest.UseVisualStyleBackColor = true;
            this.RhythmTest.Click += new System.EventHandler(this.RhythmTest_Click);
            // 
            // RhythmPatternList
            // 
            this.RhythmPatternList.FormattingEnabled = true;
            this.RhythmPatternList.Location = new System.Drawing.Point(77, 74);
            this.RhythmPatternList.Name = "RhythmPatternList";
            this.RhythmPatternList.Size = new System.Drawing.Size(97, 95);
            this.RhythmPatternList.TabIndex = 4;
            this.RhythmPatternList.SelectedIndexChanged += new System.EventHandler(this.RhythmPatternList_SelectedIndexChanged);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.ClosePort);
            this.MainPanel.Controls.Add(this.OpenPort);
            this.MainPanel.Controls.Add(this.RefreshPorts);
            this.MainPanel.Controls.Add(this.COMComboBox);
            this.MainPanel.Controls.Add(this.SelectPortLabel);
            this.MainPanel.Controls.Add(this.Exit);
            this.MainPanel.Controls.Add(this.ModeGo);
            this.MainPanel.Controls.Add(this.ModeComboBox);
            this.MainPanel.Controls.Add(this.SelectModeLabel);
            this.MainPanel.Location = new System.Drawing.Point(13, 35);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(270, 248);
            this.MainPanel.TabIndex = 25;
            // 
            // ClosePort
            // 
            this.ClosePort.Location = new System.Drawing.Point(180, 102);
            this.ClosePort.Name = "ClosePort";
            this.ClosePort.Size = new System.Drawing.Size(75, 21);
            this.ClosePort.TabIndex = 5;
            this.ClosePort.Text = "Close";
            this.ClosePort.UseVisualStyleBackColor = true;
            this.ClosePort.Visible = false;
            this.ClosePort.Click += new System.EventHandler(this.ClosePort_Click);
            // 
            // OpenPort
            // 
            this.OpenPort.Location = new System.Drawing.Point(180, 74);
            this.OpenPort.Name = "OpenPort";
            this.OpenPort.Size = new System.Drawing.Size(75, 21);
            this.OpenPort.TabIndex = 4;
            this.OpenPort.Text = "Open";
            this.OpenPort.UseVisualStyleBackColor = true;
            this.OpenPort.Click += new System.EventHandler(this.OpenPort_Click);
            // 
            // RefreshPorts
            // 
            this.RefreshPorts.Location = new System.Drawing.Point(180, 47);
            this.RefreshPorts.Name = "RefreshPorts";
            this.RefreshPorts.Size = new System.Drawing.Size(75, 21);
            this.RefreshPorts.TabIndex = 3;
            this.RefreshPorts.Text = "Refresh";
            this.RefreshPorts.UseVisualStyleBackColor = true;
            this.RefreshPorts.Click += new System.EventHandler(this.RefreshPorts_Click);
            // 
            // COMComboBox
            // 
            this.COMComboBox.FormattingEnabled = true;
            this.COMComboBox.Location = new System.Drawing.Point(99, 47);
            this.COMComboBox.Name = "COMComboBox";
            this.COMComboBox.Size = new System.Drawing.Size(75, 21);
            this.COMComboBox.TabIndex = 2;
            // 
            // SelectPortLabel
            // 
            this.SelectPortLabel.AutoSize = true;
            this.SelectPortLabel.Location = new System.Drawing.Point(13, 52);
            this.SelectPortLabel.Name = "SelectPortLabel";
            this.SelectPortLabel.Size = new System.Drawing.Size(59, 13);
            this.SelectPortLabel.TabIndex = 4;
            this.SelectPortLabel.Text = "Select Port";
            // 
            // MagPanel
            // 
            this.MagPanel.Controls.Add(this.MagTestStop);
            this.MagPanel.Controls.Add(this.DutyLabel);
            this.MagPanel.Controls.Add(this.DutyCycle);
            this.MagPanel.Controls.Add(this.Period);
            this.MagPanel.Controls.Add(this.PeriodLabel);
            this.MagPanel.Controls.Add(this.PeriodDefaultLabel);
            this.MagPanel.Controls.Add(this.MagOption);
            this.MagPanel.Controls.Add(this.Percentage);
            this.MagPanel.Controls.Add(this.PercentLabel);
            this.MagPanel.Controls.Add(this.MagTest);
            this.MagPanel.Controls.Add(this.MagBack);
            this.MagPanel.Controls.Add(this.MagLearn);
            this.MagPanel.Controls.Add(this.MagComboBox);
            this.MagPanel.Controls.Add(this.MagSelLabel);
            this.MagPanel.Location = new System.Drawing.Point(13, 35);
            this.MagPanel.Name = "MagPanel";
            this.MagPanel.Size = new System.Drawing.Size(270, 248);
            this.MagPanel.TabIndex = 26;
            this.MagPanel.Visible = false;
            // 
            // MagTestStop
            // 
            this.MagTestStop.Location = new System.Drawing.Point(98, 222);
            this.MagTestStop.Name = "MagTestStop";
            this.MagTestStop.Size = new System.Drawing.Size(75, 23);
            this.MagTestStop.TabIndex = 37;
            this.MagTestStop.Text = "Stop";
            this.MagTestStop.UseVisualStyleBackColor = true;
            this.MagTestStop.Visible = false;
            this.MagTestStop.Click += new System.EventHandler(this.MagTestStop_Click);
            // 
            // DutyLabel
            // 
            this.DutyLabel.AutoSize = true;
            this.DutyLabel.Location = new System.Drawing.Point(28, 128);
            this.DutyLabel.Name = "DutyLabel";
            this.DutyLabel.Size = new System.Drawing.Size(75, 13);
            this.DutyLabel.TabIndex = 14;
            this.DutyLabel.Text = "Duty Cycle(us)";
            this.DutyLabel.Visible = false;
            // 
            // DutyCycle
            // 
            this.DutyCycle.Location = new System.Drawing.Point(108, 126);
            this.DutyCycle.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.DutyCycle.Name = "DutyCycle";
            this.DutyCycle.Size = new System.Drawing.Size(66, 20);
            this.DutyCycle.TabIndex = 13;
            this.DutyCycle.Visible = false;
            this.DutyCycle.ValueChanged += new System.EventHandler(this.DutyCycle_ValueChanged);
            // 
            // Period
            // 
            this.Period.Location = new System.Drawing.Point(108, 100);
            this.Period.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.Period.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Period.Name = "Period";
            this.Period.Size = new System.Drawing.Size(66, 20);
            this.Period.TabIndex = 12;
            this.Period.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.Period.Visible = false;
            this.Period.ValueChanged += new System.EventHandler(this.Period_ValueChanged);
            // 
            // PeriodLabel
            // 
            this.PeriodLabel.AutoSize = true;
            this.PeriodLabel.Location = new System.Drawing.Point(46, 102);
            this.PeriodLabel.Name = "PeriodLabel";
            this.PeriodLabel.Size = new System.Drawing.Size(54, 13);
            this.PeriodLabel.TabIndex = 11;
            this.PeriodLabel.Text = "Period(us)";
            this.PeriodLabel.Visible = false;
            // 
            // PeriodDefaultLabel
            // 
            this.PeriodDefaultLabel.AutoSize = true;
            this.PeriodDefaultLabel.Location = new System.Drawing.Point(180, 102);
            this.PeriodDefaultLabel.Name = "PeriodDefaultLabel";
            this.PeriodDefaultLabel.Size = new System.Drawing.Size(74, 13);
            this.PeriodDefaultLabel.TabIndex = 10;
            this.PeriodDefaultLabel.Text = "(Default 2000)";
            this.PeriodDefaultLabel.Visible = false;
            // 
            // MagOption
            // 
            this.MagOption.AutoSize = true;
            this.MagOption.Location = new System.Drawing.Point(141, 199);
            this.MagOption.Name = "MagOption";
            this.MagOption.Size = new System.Drawing.Size(114, 17);
            this.MagOption.TabIndex = 7;
            this.MagOption.Text = "Advanced Options";
            this.MagOption.UseVisualStyleBackColor = true;
            this.MagOption.CheckedChanged += new System.EventHandler(this.MagOption_CheckedChanged);
            // 
            // Percentage
            // 
            this.Percentage.Location = new System.Drawing.Point(108, 71);
            this.Percentage.Name = "Percentage";
            this.Percentage.Size = new System.Drawing.Size(65, 20);
            this.Percentage.TabIndex = 6;
            this.Percentage.ValueChanged += new System.EventHandler(this.Percentage_ValueChanged);
            // 
            // PercentLabel
            // 
            this.PercentLabel.AutoSize = true;
            this.PercentLabel.Location = new System.Drawing.Point(31, 73);
            this.PercentLabel.Name = "PercentLabel";
            this.PercentLabel.Size = new System.Drawing.Size(71, 13);
            this.PercentLabel.TabIndex = 5;
            this.PercentLabel.Text = "Magnitude(%)";
            // 
            // MagTest
            // 
            this.MagTest.Location = new System.Drawing.Point(98, 222);
            this.MagTest.Name = "MagTest";
            this.MagTest.Size = new System.Drawing.Size(75, 23);
            this.MagTest.TabIndex = 4;
            this.MagTest.Text = "Test";
            this.MagTest.UseVisualStyleBackColor = true;
            this.MagTest.Click += new System.EventHandler(this.MagTest_Click);
            // 
            // MagBack
            // 
            this.MagBack.Location = new System.Drawing.Point(16, 222);
            this.MagBack.Name = "MagBack";
            this.MagBack.Size = new System.Drawing.Size(75, 23);
            this.MagBack.TabIndex = 3;
            this.MagBack.Text = "Back";
            this.MagBack.UseVisualStyleBackColor = true;
            this.MagBack.Click += new System.EventHandler(this.MagBack_Click);
            // 
            // MagLearn
            // 
            this.MagLearn.Location = new System.Drawing.Point(180, 222);
            this.MagLearn.Name = "MagLearn";
            this.MagLearn.Size = new System.Drawing.Size(75, 23);
            this.MagLearn.TabIndex = 2;
            this.MagLearn.Text = "Learn";
            this.MagLearn.UseVisualStyleBackColor = true;
            this.MagLearn.Click += new System.EventHandler(this.MagLearn_Click);
            // 
            // MagComboBox
            // 
            this.MagComboBox.FormattingEnabled = true;
            this.MagComboBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D"});
            this.MagComboBox.Location = new System.Drawing.Point(108, 16);
            this.MagComboBox.Name = "MagComboBox";
            this.MagComboBox.Size = new System.Drawing.Size(65, 21);
            this.MagComboBox.TabIndex = 0;
            this.MagComboBox.Text = "A";
            this.MagComboBox.SelectedIndexChanged += new System.EventHandler(this.MagComboBox_SelectedIndexChanged);
            // 
            // MagSelLabel
            // 
            this.MagSelLabel.AutoSize = true;
            this.MagSelLabel.BackColor = System.Drawing.SystemColors.Control;
            this.MagSelLabel.Location = new System.Drawing.Point(12, 19);
            this.MagSelLabel.Name = "MagSelLabel";
            this.MagSelLabel.Size = new System.Drawing.Size(90, 13);
            this.MagSelLabel.TabIndex = 1;
            this.MagSelLabel.Text = "Select Magnitude";
            // 
            // RhythmPanel
            // 
            this.RhythmPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.RhythmPanel.Controls.Add(this.RhythmTestStop);
            this.RhythmPanel.Controls.Add(this.RhythmTime);
            this.RhythmPanel.Controls.Add(this.RhythmLearn);
            this.RhythmPanel.Controls.Add(this.TimeLabel);
            this.RhythmPanel.Controls.Add(this.RhythmClear);
            this.RhythmPanel.Controls.Add(this.RhythmPaint);
            this.RhythmPanel.Controls.Add(this.RhythmDelete);
            this.RhythmPanel.Controls.Add(this.RhythmReplace);
            this.RhythmPanel.Controls.Add(this.RhythmInsert);
            this.RhythmPanel.Controls.Add(this.RhythmAdd);
            this.RhythmPanel.Controls.Add(this.RhythmOffLabel);
            this.RhythmPanel.Controls.Add(this.RhythmOnLabel);
            this.RhythmPanel.Controls.Add(this.RhythmOff);
            this.RhythmPanel.Controls.Add(this.RhythmOn);
            this.RhythmPanel.Controls.Add(this.RhythmTest);
            this.RhythmPanel.Controls.Add(this.RhythmBack);
            this.RhythmPanel.Controls.Add(this.RhythmLabel);
            this.RhythmPanel.Controls.Add(this.RhythmComboBox);
            this.RhythmPanel.Controls.Add(this.RhythmPatternList);
            this.RhythmPanel.Location = new System.Drawing.Point(13, 35);
            this.RhythmPanel.Name = "RhythmPanel";
            this.RhythmPanel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RhythmPanel.Size = new System.Drawing.Size(270, 248);
            this.RhythmPanel.TabIndex = 26;
            this.RhythmPanel.Visible = false;
            // 
            // RhythmTestStop
            // 
            this.RhythmTestStop.Location = new System.Drawing.Point(98, 222);
            this.RhythmTestStop.Name = "RhythmTestStop";
            this.RhythmTestStop.Size = new System.Drawing.Size(75, 23);
            this.RhythmTestStop.TabIndex = 13;
            this.RhythmTestStop.Text = "Stop";
            this.RhythmTestStop.UseVisualStyleBackColor = true;
            this.RhythmTestStop.Visible = false;
            this.RhythmTestStop.Click += new System.EventHandler(this.RhythmTestStop_Click);
            // 
            // RhythmTime
            // 
            this.RhythmTime.Location = new System.Drawing.Point(11, 159);
            this.RhythmTime.Name = "RhythmTime";
            this.RhythmTime.Size = new System.Drawing.Size(49, 13);
            this.RhythmTime.TabIndex = 35;
            this.RhythmTime.Text = "0";
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Location = new System.Drawing.Point(4, 146);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(60, 13);
            this.TimeLabel.TabIndex = 34;
            this.TimeLabel.Text = "Total Time:";
            // 
            // RhythmClear
            // 
            this.RhythmClear.Location = new System.Drawing.Point(180, 149);
            this.RhythmClear.Name = "RhythmClear";
            this.RhythmClear.Size = new System.Drawing.Size(75, 20);
            this.RhythmClear.TabIndex = 9;
            this.RhythmClear.Text = "Clear";
            this.RhythmClear.UseVisualStyleBackColor = true;
            this.RhythmClear.Click += new System.EventHandler(this.RhythmClear_Click);
            // 
            // RhythmPaint
            // 
            this.RhythmPaint.Location = new System.Drawing.Point(7, 175);
            this.RhythmPaint.Name = "RhythmPaint";
            this.RhythmPaint.Size = new System.Drawing.Size(257, 41);
            this.RhythmPaint.TabIndex = 32;
            // 
            // RhythmDelete
            // 
            this.RhythmDelete.Location = new System.Drawing.Point(180, 129);
            this.RhythmDelete.Name = "RhythmDelete";
            this.RhythmDelete.Size = new System.Drawing.Size(75, 20);
            this.RhythmDelete.TabIndex = 8;
            this.RhythmDelete.Text = "Delete";
            this.RhythmDelete.UseVisualStyleBackColor = true;
            this.RhythmDelete.Click += new System.EventHandler(this.RhythmDelete_Click);
            // 
            // RhythmReplace
            // 
            this.RhythmReplace.Location = new System.Drawing.Point(180, 90);
            this.RhythmReplace.Name = "RhythmReplace";
            this.RhythmReplace.Size = new System.Drawing.Size(75, 20);
            this.RhythmReplace.TabIndex = 7;
            this.RhythmReplace.Text = "Replace";
            this.RhythmReplace.UseVisualStyleBackColor = true;
            this.RhythmReplace.Click += new System.EventHandler(this.RhythmReplace_Click);
            // 
            // RhythmInsert
            // 
            this.RhythmInsert.Location = new System.Drawing.Point(180, 70);
            this.RhythmInsert.Name = "RhythmInsert";
            this.RhythmInsert.Size = new System.Drawing.Size(75, 20);
            this.RhythmInsert.TabIndex = 6;
            this.RhythmInsert.Text = "Insert";
            this.RhythmInsert.UseVisualStyleBackColor = true;
            this.RhythmInsert.Click += new System.EventHandler(this.RhythmInsert_Click);
            // 
            // RhythmAdd
            // 
            this.RhythmAdd.Location = new System.Drawing.Point(180, 50);
            this.RhythmAdd.Name = "RhythmAdd";
            this.RhythmAdd.Size = new System.Drawing.Size(75, 20);
            this.RhythmAdd.TabIndex = 5;
            this.RhythmAdd.Text = "Add";
            this.RhythmAdd.UseVisualStyleBackColor = true;
            this.RhythmAdd.Click += new System.EventHandler(this.RhythmAdd_Click);
            // 
            // RhythmOffLabel
            // 
            this.RhythmOffLabel.AutoSize = true;
            this.RhythmOffLabel.BackColor = System.Drawing.SystemColors.Control;
            this.RhythmOffLabel.Location = new System.Drawing.Point(132, 37);
            this.RhythmOffLabel.Name = "RhythmOffLabel";
            this.RhythmOffLabel.Size = new System.Drawing.Size(21, 13);
            this.RhythmOffLabel.TabIndex = 27;
            this.RhythmOffLabel.Text = "Off";
            // 
            // RhythmOnLabel
            // 
            this.RhythmOnLabel.AutoSize = true;
            this.RhythmOnLabel.BackColor = System.Drawing.SystemColors.Control;
            this.RhythmOnLabel.Location = new System.Drawing.Point(84, 37);
            this.RhythmOnLabel.Name = "RhythmOnLabel";
            this.RhythmOnLabel.Size = new System.Drawing.Size(21, 13);
            this.RhythmOnLabel.TabIndex = 26;
            this.RhythmOnLabel.Text = "On";
            // 
            // RhythmOff
            // 
            this.RhythmOff.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.RhythmOff.Location = new System.Drawing.Point(126, 50);
            this.RhythmOff.Maximum = new decimal(new int[] {
            3200,
            0,
            0,
            0});
            this.RhythmOff.Name = "RhythmOff";
            this.RhythmOff.Size = new System.Drawing.Size(48, 20);
            this.RhythmOff.TabIndex = 3;
            this.RhythmOff.ValueChanged += new System.EventHandler(this.RhythmOff_ValueChanged);
            // 
            // RhythmOn
            // 
            this.RhythmOn.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.RhythmOn.Location = new System.Drawing.Point(77, 50);
            this.RhythmOn.Maximum = new decimal(new int[] {
            3200,
            0,
            0,
            0});
            this.RhythmOn.Name = "RhythmOn";
            this.RhythmOn.Size = new System.Drawing.Size(48, 20);
            this.RhythmOn.TabIndex = 2;
            this.RhythmOn.ValueChanged += new System.EventHandler(this.RhythmOn_ValueChanged);
            // 
            // RhythmLabel
            // 
            this.RhythmLabel.AutoSize = true;
            this.RhythmLabel.BackColor = System.Drawing.SystemColors.Control;
            this.RhythmLabel.Location = new System.Drawing.Point(13, 19);
            this.RhythmLabel.Name = "RhythmLabel";
            this.RhythmLabel.Size = new System.Drawing.Size(76, 13);
            this.RhythmLabel.TabIndex = 1;
            this.RhythmLabel.Text = "Select Rhythm";
            // 
            // DirectPanel
            // 
            this.DirectPanel.Controls.Add(this.DirectSave);
            this.DirectPanel.Controls.Add(this.DirectLoad);
            this.DirectPanel.Controls.Add(this.DirectRenameLabel);
            this.DirectPanel.Controls.Add(this.DirectActivateMotor);
            this.DirectPanel.Controls.Add(this.DirectActivateGroup);
            this.DirectPanel.Controls.Add(this.DirectGroupLabel);
            this.DirectPanel.Controls.Add(this.DirectAddGroup);
            this.DirectPanel.Controls.Add(this.DirectDeleteGroup);
            this.DirectPanel.Controls.Add(this.DirectClearGroup);
            this.DirectPanel.Controls.Add(this.DirectClearSet);
            this.DirectPanel.Controls.Add(this.DirectDeleteSet);
            this.DirectPanel.Controls.Add(this.DirectAddSet);
            this.DirectPanel.Controls.Add(this.GroupList);
            this.DirectPanel.Controls.Add(this.DirectRenameGroup);
            this.DirectPanel.Controls.Add(this.DirectBack);
            this.DirectPanel.Controls.Add(this.DirectRenameField);
            this.DirectPanel.Controls.Add(this.DirectRenameSet);
            this.DirectPanel.Controls.Add(this.DirectStop);
            this.DirectPanel.Controls.Add(this.DirectSetLabel);
            this.DirectPanel.Controls.Add(this.AddedCycleLabel);
            this.DirectPanel.Controls.Add(this.AddedMagLabel);
            this.DirectPanel.Controls.Add(this.AddedRhythmLabel);
            this.DirectPanel.Controls.Add(this.DirectClearMotor);
            this.DirectPanel.Controls.Add(this.DirectActivateSet);
            this.DirectPanel.Controls.Add(this.DirectDeleteMotor);
            this.DirectPanel.Controls.Add(this.DirectAddMotor);
            this.DirectPanel.Controls.Add(this.DirectCyclesLabel);
            this.DirectPanel.Controls.Add(this.DirectMagLabel);
            this.DirectPanel.Controls.Add(this.DirectRhythmLabel);
            this.DirectPanel.Controls.Add(this.DirectCyclesBox);
            this.DirectPanel.Controls.Add(this.DirectMagBox);
            this.DirectPanel.Controls.Add(this.DirectRhythmBox);
            this.DirectPanel.Controls.Add(this.DirectMotorLabel);
            this.DirectPanel.Controls.Add(this.SetList);
            this.DirectPanel.Controls.Add(this.AddedList);
            this.DirectPanel.Controls.Add(this.AvailableList);
            this.DirectPanel.Location = new System.Drawing.Point(13, 35);
            this.DirectPanel.Name = "DirectPanel";
            this.DirectPanel.Size = new System.Drawing.Size(561, 248);
            this.DirectPanel.TabIndex = 27;
            this.DirectPanel.Visible = false;
            // 
            // DirectSave
            // 
            this.DirectSave.Location = new System.Drawing.Point(470, 45);
            this.DirectSave.Name = "DirectSave";
            this.DirectSave.Size = new System.Drawing.Size(75, 23);
            this.DirectSave.TabIndex = 39;
            this.DirectSave.Text = "Save";
            this.DirectSave.UseVisualStyleBackColor = true;
            this.DirectSave.Click += new System.EventHandler(this.DirectSave_Click);
            // 
            // DirectLoad
            // 
            this.DirectLoad.Location = new System.Drawing.Point(470, 14);
            this.DirectLoad.Name = "DirectLoad";
            this.DirectLoad.Size = new System.Drawing.Size(75, 23);
            this.DirectLoad.TabIndex = 38;
            this.DirectLoad.Text = "Load";
            this.DirectLoad.UseVisualStyleBackColor = true;
            this.DirectLoad.Click += new System.EventHandler(this.DirectLoad_Click);
            // 
            // DirectRenameLabel
            // 
            this.DirectRenameLabel.AutoSize = true;
            this.DirectRenameLabel.Location = new System.Drawing.Point(314, 3);
            this.DirectRenameLabel.Name = "DirectRenameLabel";
            this.DirectRenameLabel.Size = new System.Drawing.Size(72, 13);
            this.DirectRenameLabel.TabIndex = 30;
            this.DirectRenameLabel.Text = "Rename Field";
            // 
            // DirectActivateMotor
            // 
            this.DirectActivateMotor.Location = new System.Drawing.Point(99, 183);
            this.DirectActivateMotor.Name = "DirectActivateMotor";
            this.DirectActivateMotor.Size = new System.Drawing.Size(60, 20);
            this.DirectActivateMotor.TabIndex = 37;
            this.DirectActivateMotor.Text = "Activate";
            this.DirectActivateMotor.UseVisualStyleBackColor = true;
            this.DirectActivateMotor.Click += new System.EventHandler(this.DirectActivateMotor_Click);
            // 
            // DirectActivateGroup
            // 
            this.DirectActivateGroup.Location = new System.Drawing.Point(402, 183);
            this.DirectActivateGroup.Name = "DirectActivateGroup";
            this.DirectActivateGroup.Size = new System.Drawing.Size(60, 20);
            this.DirectActivateGroup.TabIndex = 36;
            this.DirectActivateGroup.Text = "Activate";
            this.DirectActivateGroup.UseVisualStyleBackColor = true;
            this.DirectActivateGroup.Click += new System.EventHandler(this.DirectActivateGroup_Click);
            // 
            // DirectGroupLabel
            // 
            this.DirectGroupLabel.AutoSize = true;
            this.DirectGroupLabel.Location = new System.Drawing.Point(336, 69);
            this.DirectGroupLabel.Name = "DirectGroupLabel";
            this.DirectGroupLabel.Size = new System.Drawing.Size(36, 13);
            this.DirectGroupLabel.TabIndex = 35;
            this.DirectGroupLabel.Text = "Group";
            // 
            // DirectAddGroup
            // 
            this.DirectAddGroup.Location = new System.Drawing.Point(402, 82);
            this.DirectAddGroup.Name = "DirectAddGroup";
            this.DirectAddGroup.Size = new System.Drawing.Size(60, 20);
            this.DirectAddGroup.TabIndex = 34;
            this.DirectAddGroup.Text = "Add";
            this.DirectAddGroup.UseVisualStyleBackColor = true;
            this.DirectAddGroup.Click += new System.EventHandler(this.DirectAddGroup_Click);
            // 
            // DirectDeleteGroup
            // 
            this.DirectDeleteGroup.Location = new System.Drawing.Point(402, 118);
            this.DirectDeleteGroup.Name = "DirectDeleteGroup";
            this.DirectDeleteGroup.Size = new System.Drawing.Size(60, 20);
            this.DirectDeleteGroup.TabIndex = 33;
            this.DirectDeleteGroup.Text = "Delete";
            this.DirectDeleteGroup.UseVisualStyleBackColor = true;
            this.DirectDeleteGroup.Click += new System.EventHandler(this.DirectDeleteGroup_Click);
            // 
            // DirectClearGroup
            // 
            this.DirectClearGroup.Location = new System.Drawing.Point(402, 139);
            this.DirectClearGroup.Name = "DirectClearGroup";
            this.DirectClearGroup.Size = new System.Drawing.Size(60, 20);
            this.DirectClearGroup.TabIndex = 32;
            this.DirectClearGroup.Text = "Clear";
            this.DirectClearGroup.UseVisualStyleBackColor = true;
            this.DirectClearGroup.Click += new System.EventHandler(this.DirectClearGroup_Click);
            // 
            // DirectClearSet
            // 
            this.DirectClearSet.Location = new System.Drawing.Point(250, 139);
            this.DirectClearSet.Name = "DirectClearSet";
            this.DirectClearSet.Size = new System.Drawing.Size(60, 20);
            this.DirectClearSet.TabIndex = 31;
            this.DirectClearSet.Text = "Clear";
            this.DirectClearSet.UseVisualStyleBackColor = true;
            this.DirectClearSet.Click += new System.EventHandler(this.DirectClearSet_Click);
            // 
            // DirectDeleteSet
            // 
            this.DirectDeleteSet.Location = new System.Drawing.Point(250, 118);
            this.DirectDeleteSet.Name = "DirectDeleteSet";
            this.DirectDeleteSet.Size = new System.Drawing.Size(60, 20);
            this.DirectDeleteSet.TabIndex = 30;
            this.DirectDeleteSet.Text = "Delete";
            this.DirectDeleteSet.UseVisualStyleBackColor = true;
            this.DirectDeleteSet.Click += new System.EventHandler(this.DirectDeleteSet_Click);
            // 
            // DirectAddSet
            // 
            this.DirectAddSet.Location = new System.Drawing.Point(250, 82);
            this.DirectAddSet.Name = "DirectAddSet";
            this.DirectAddSet.Size = new System.Drawing.Size(60, 20);
            this.DirectAddSet.TabIndex = 29;
            this.DirectAddSet.Text = "Add";
            this.DirectAddSet.UseVisualStyleBackColor = true;
            this.DirectAddSet.Click += new System.EventHandler(this.DirectAddSet_Click);
            // 
            // GroupList
            // 
            this.GroupList.FormattingEnabled = true;
            this.GroupList.Location = new System.Drawing.Point(317, 82);
            this.GroupList.Name = "GroupList";
            this.GroupList.Size = new System.Drawing.Size(79, 134);
            this.GroupList.TabIndex = 28;
            this.GroupList.SelectedIndexChanged += new System.EventHandler(this.GroupList_SelectedIndexChanged);
            // 
            // DirectRenameGroup
            // 
            this.DirectRenameGroup.Location = new System.Drawing.Point(307, 52);
            this.DirectRenameGroup.Name = "DirectRenameGroup";
            this.DirectRenameGroup.Size = new System.Drawing.Size(89, 18);
            this.DirectRenameGroup.TabIndex = 27;
            this.DirectRenameGroup.Text = "Rename Group";
            this.DirectRenameGroup.UseCompatibleTextRendering = true;
            this.DirectRenameGroup.UseVisualStyleBackColor = true;
            this.DirectRenameGroup.Click += new System.EventHandler(this.DirectRenameGroup_Click);
            // 
            // DirectBack
            // 
            this.DirectBack.Location = new System.Drawing.Point(16, 222);
            this.DirectBack.Name = "DirectBack";
            this.DirectBack.Size = new System.Drawing.Size(75, 23);
            this.DirectBack.TabIndex = 26;
            this.DirectBack.Text = "Back";
            this.DirectBack.UseVisualStyleBackColor = true;
            this.DirectBack.Click += new System.EventHandler(this.DirectBack_Click);
            // 
            // DirectRenameField
            // 
            this.DirectRenameField.Location = new System.Drawing.Point(307, 16);
            this.DirectRenameField.MaxLength = 10;
            this.DirectRenameField.Name = "DirectRenameField";
            this.DirectRenameField.Size = new System.Drawing.Size(89, 20);
            this.DirectRenameField.TabIndex = 25;
            // 
            // DirectRenameSet
            // 
            this.DirectRenameSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DirectRenameSet.Location = new System.Drawing.Point(307, 35);
            this.DirectRenameSet.Name = "DirectRenameSet";
            this.DirectRenameSet.Size = new System.Drawing.Size(89, 18);
            this.DirectRenameSet.TabIndex = 24;
            this.DirectRenameSet.Text = "Rename Set";
            this.DirectRenameSet.UseCompatibleTextRendering = true;
            this.DirectRenameSet.UseVisualStyleBackColor = true;
            this.DirectRenameSet.Click += new System.EventHandler(this.DirectRenameSet_Click);
            // 
            // DirectStop
            // 
            this.DirectStop.Location = new System.Drawing.Point(470, 222);
            this.DirectStop.Name = "DirectStop";
            this.DirectStop.Size = new System.Drawing.Size(75, 23);
            this.DirectStop.TabIndex = 23;
            this.DirectStop.Text = "Stop";
            this.DirectStop.UseVisualStyleBackColor = true;
            this.DirectStop.Click += new System.EventHandler(this.DirectStop_Click);
            // 
            // DirectSetLabel
            // 
            this.DirectSetLabel.AutoSize = true;
            this.DirectSetLabel.Location = new System.Drawing.Point(193, 69);
            this.DirectSetLabel.Name = "DirectSetLabel";
            this.DirectSetLabel.Size = new System.Drawing.Size(23, 13);
            this.DirectSetLabel.TabIndex = 5;
            this.DirectSetLabel.Text = "Set";
            // 
            // AddedCycleLabel
            // 
            this.AddedCycleLabel.AutoSize = true;
            this.AddedCycleLabel.Location = new System.Drawing.Point(246, 43);
            this.AddedCycleLabel.Name = "AddedCycleLabel";
            this.AddedCycleLabel.Size = new System.Drawing.Size(27, 13);
            this.AddedCycleLabel.TabIndex = 22;
            this.AddedCycleLabel.Text = "N/A";
            // 
            // AddedMagLabel
            // 
            this.AddedMagLabel.AutoSize = true;
            this.AddedMagLabel.Location = new System.Drawing.Point(183, 43);
            this.AddedMagLabel.Name = "AddedMagLabel";
            this.AddedMagLabel.Size = new System.Drawing.Size(27, 13);
            this.AddedMagLabel.TabIndex = 21;
            this.AddedMagLabel.Text = "N/A";
            // 
            // AddedRhythmLabel
            // 
            this.AddedRhythmLabel.AutoSize = true;
            this.AddedRhythmLabel.Location = new System.Drawing.Point(116, 43);
            this.AddedRhythmLabel.Name = "AddedRhythmLabel";
            this.AddedRhythmLabel.Size = new System.Drawing.Size(27, 13);
            this.AddedRhythmLabel.TabIndex = 20;
            this.AddedRhythmLabel.Text = "N/A";
            // 
            // DirectClearMotor
            // 
            this.DirectClearMotor.Location = new System.Drawing.Point(99, 139);
            this.DirectClearMotor.Name = "DirectClearMotor";
            this.DirectClearMotor.Size = new System.Drawing.Size(60, 20);
            this.DirectClearMotor.TabIndex = 19;
            this.DirectClearMotor.Text = "Clear";
            this.DirectClearMotor.UseVisualStyleBackColor = true;
            this.DirectClearMotor.Click += new System.EventHandler(this.DirectClearMotor_Click);
            // 
            // DirectActivateSet
            // 
            this.DirectActivateSet.Location = new System.Drawing.Point(250, 183);
            this.DirectActivateSet.Name = "DirectActivateSet";
            this.DirectActivateSet.Size = new System.Drawing.Size(60, 20);
            this.DirectActivateSet.TabIndex = 15;
            this.DirectActivateSet.Text = "Activate";
            this.DirectActivateSet.UseVisualStyleBackColor = true;
            // 
            // DirectDeleteMotor
            // 
            this.DirectDeleteMotor.Location = new System.Drawing.Point(99, 118);
            this.DirectDeleteMotor.Name = "DirectDeleteMotor";
            this.DirectDeleteMotor.Size = new System.Drawing.Size(60, 20);
            this.DirectDeleteMotor.TabIndex = 14;
            this.DirectDeleteMotor.Text = "Delete";
            this.DirectDeleteMotor.UseVisualStyleBackColor = true;
            this.DirectDeleteMotor.Click += new System.EventHandler(this.DirectDeleteMotor_Click);
            // 
            // DirectAddMotor
            // 
            this.DirectAddMotor.Location = new System.Drawing.Point(99, 82);
            this.DirectAddMotor.Name = "DirectAddMotor";
            this.DirectAddMotor.Size = new System.Drawing.Size(60, 20);
            this.DirectAddMotor.TabIndex = 13;
            this.DirectAddMotor.Text = "Add";
            this.DirectAddMotor.UseVisualStyleBackColor = true;
            this.DirectAddMotor.Click += new System.EventHandler(this.DirectAddMotor_Click);
            // 
            // DirectCyclesLabel
            // 
            this.DirectCyclesLabel.AutoSize = true;
            this.DirectCyclesLabel.Location = new System.Drawing.Point(242, 3);
            this.DirectCyclesLabel.Name = "DirectCyclesLabel";
            this.DirectCyclesLabel.Size = new System.Drawing.Size(38, 13);
            this.DirectCyclesLabel.TabIndex = 12;
            this.DirectCyclesLabel.Text = "Cycles";
            // 
            // DirectMagLabel
            // 
            this.DirectMagLabel.AutoSize = true;
            this.DirectMagLabel.Location = new System.Drawing.Point(170, 3);
            this.DirectMagLabel.Name = "DirectMagLabel";
            this.DirectMagLabel.Size = new System.Drawing.Size(57, 13);
            this.DirectMagLabel.TabIndex = 11;
            this.DirectMagLabel.Text = "Magnitude";
            // 
            // DirectRhythmLabel
            // 
            this.DirectRhythmLabel.AutoSize = true;
            this.DirectRhythmLabel.Location = new System.Drawing.Point(111, 3);
            this.DirectRhythmLabel.Name = "DirectRhythmLabel";
            this.DirectRhythmLabel.Size = new System.Drawing.Size(43, 13);
            this.DirectRhythmLabel.TabIndex = 10;
            this.DirectRhythmLabel.Text = "Rhythm";
            // 
            // DirectCyclesBox
            // 
            this.DirectCyclesBox.FormattingEnabled = true;
            this.DirectCyclesBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "Inf"});
            this.DirectCyclesBox.Location = new System.Drawing.Point(239, 16);
            this.DirectCyclesBox.Name = "DirectCyclesBox";
            this.DirectCyclesBox.Size = new System.Drawing.Size(45, 21);
            this.DirectCyclesBox.TabIndex = 9;
            this.DirectCyclesBox.Text = "1";
            // 
            // DirectMagBox
            // 
            this.DirectMagBox.FormattingEnabled = true;
            this.DirectMagBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D"});
            this.DirectMagBox.Location = new System.Drawing.Point(176, 16);
            this.DirectMagBox.Name = "DirectMagBox";
            this.DirectMagBox.Size = new System.Drawing.Size(45, 21);
            this.DirectMagBox.TabIndex = 8;
            this.DirectMagBox.Text = "A";
            // 
            // DirectRhythmBox
            // 
            this.DirectRhythmBox.FormattingEnabled = true;
            this.DirectRhythmBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E"});
            this.DirectRhythmBox.Location = new System.Drawing.Point(110, 16);
            this.DirectRhythmBox.Name = "DirectRhythmBox";
            this.DirectRhythmBox.Size = new System.Drawing.Size(45, 21);
            this.DirectRhythmBox.TabIndex = 7;
            this.DirectRhythmBox.Text = "A";
            // 
            // DirectMotorLabel
            // 
            this.DirectMotorLabel.AutoSize = true;
            this.DirectMotorLabel.Location = new System.Drawing.Point(7, 3);
            this.DirectMotorLabel.Name = "DirectMotorLabel";
            this.DirectMotorLabel.Size = new System.Drawing.Size(87, 13);
            this.DirectMotorLabel.TabIndex = 3;
            this.DirectMotorLabel.Text = "Available  Added";
            // 
            // SetList
            // 
            this.SetList.FormattingEnabled = true;
            this.SetList.Location = new System.Drawing.Point(165, 82);
            this.SetList.Name = "SetList";
            this.SetList.Size = new System.Drawing.Size(79, 134);
            this.SetList.TabIndex = 2;
            this.SetList.SelectedIndexChanged += new System.EventHandler(this.SetList_SelectedIndexChanged);
            // 
            // AddedList
            // 
            this.AddedList.FormattingEnabled = true;
            this.AddedList.Location = new System.Drawing.Point(53, 17);
            this.AddedList.Name = "AddedList";
            this.AddedList.Size = new System.Drawing.Size(40, 199);
            this.AddedList.TabIndex = 1;
            this.AddedList.SelectedIndexChanged += new System.EventHandler(this.AddedList_SelectedIndexChanged);
            // 
            // AvailableList
            // 
            this.AvailableList.FormattingEnabled = true;
            this.AvailableList.Location = new System.Drawing.Point(13, 17);
            this.AvailableList.Name = "AvailableList";
            this.AvailableList.Size = new System.Drawing.Size(39, 199);
            this.AvailableList.TabIndex = 0;
            // 
            // ErrorStatus
            // 
            this.ErrorStatus.AutoSize = true;
            this.ErrorStatus.Location = new System.Drawing.Point(12, 19);
            this.ErrorStatus.Name = "ErrorStatus";
            this.ErrorStatus.Size = new System.Drawing.Size(65, 13);
            this.ErrorStatus.TabIndex = 28;
            this.ErrorStatus.Text = "Error Status:";
            // 
            // ErrorLocation
            // 
            this.ErrorLocation.AutoSize = true;
            this.ErrorLocation.Location = new System.Drawing.Point(12, 4);
            this.ErrorLocation.Name = "ErrorLocation";
            this.ErrorLocation.Size = new System.Drawing.Size(76, 13);
            this.ErrorLocation.TabIndex = 29;
            this.ErrorLocation.Text = "Error Location:";
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(585, 289);
            this.Controls.Add(this.ErrorLocation);
            this.Controls.Add(this.ErrorStatus);
            this.Controls.Add(this.DirectPanel);
            this.Controls.Add(this.RhythmPanel);
            this.Controls.Add(this.MagPanel);
            this.Controls.Add(this.MainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GUI";
            this.Text = "Haptic Belt GUI";
            this.Load += new System.EventHandler(this.GUI_Load);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.MagPanel.ResumeLayout(false);
            this.MagPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DutyCycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Period)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Percentage)).EndInit();
            this.RhythmPanel.ResumeLayout(false);
            this.RhythmPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RhythmOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RhythmOn)).EndInit();
            this.DirectPanel.ResumeLayout(false);
            this.DirectPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ModeComboBox;
        private System.Windows.Forms.Label SelectModeLabel;
        private System.Windows.Forms.Button ModeGo;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.ComboBox RhythmComboBox;
        private System.Windows.Forms.Button RhythmLearn;
        private System.Windows.Forms.Button RhythmBack;
        private System.Windows.Forms.Button RhythmTest;
        private System.Windows.Forms.ListBox RhythmPatternList;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Panel MagPanel;
        private System.Windows.Forms.Button MagBack;
        private System.Windows.Forms.Button MagLearn;
        private System.Windows.Forms.ComboBox MagComboBox;
        private System.Windows.Forms.Label MagSelLabel;
        private System.Windows.Forms.Panel RhythmPanel;
        private System.Windows.Forms.Label RhythmLabel;
        private System.Windows.Forms.NumericUpDown RhythmOff;
        private System.Windows.Forms.NumericUpDown RhythmOn;
        private System.Windows.Forms.Label RhythmOnLabel;
        private System.Windows.Forms.Label RhythmOffLabel;
        private System.Windows.Forms.Button RhythmAdd;
        private System.Windows.Forms.Button RhythmDelete;
        private System.Windows.Forms.Button RhythmReplace;
        private System.Windows.Forms.Button RhythmInsert;
        private System.Windows.Forms.Panel RhythmPaint;
        private System.Windows.Forms.Button RhythmClear;
        private System.Windows.Forms.Button MagTest;
        private System.Windows.Forms.Label PercentLabel;
        private System.Windows.Forms.CheckBox MagOption;
        private System.Windows.Forms.NumericUpDown Percentage;
        private System.Windows.Forms.NumericUpDown DutyCycle;
        private System.Windows.Forms.NumericUpDown Period;
        private System.Windows.Forms.Label PeriodLabel;
        private System.Windows.Forms.Label PeriodDefaultLabel;
        private System.Windows.Forms.Label DutyLabel;
        private System.Windows.Forms.Panel DirectPanel;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Label RhythmTime;
        private System.Windows.Forms.ListBox AddedList;
        private System.Windows.Forms.ListBox AvailableList;
        private System.Windows.Forms.Label DirectSetLabel;
        private System.Windows.Forms.Label DirectMotorLabel;
        private System.Windows.Forms.ListBox SetList;
        private System.Windows.Forms.ComboBox DirectCyclesBox;
        private System.Windows.Forms.ComboBox DirectMagBox;
        private System.Windows.Forms.ComboBox DirectRhythmBox;
        private System.Windows.Forms.Label DirectRhythmLabel;
        private System.Windows.Forms.Label DirectCyclesLabel;
        private System.Windows.Forms.Label DirectMagLabel;
        private System.Windows.Forms.Button DirectDeleteMotor;
        private System.Windows.Forms.Button DirectAddMotor;
        private System.Windows.Forms.Button DirectActivateSet;
        private System.Windows.Forms.Button DirectClearMotor;
        private System.Windows.Forms.Label AddedCycleLabel;
        private System.Windows.Forms.Label AddedMagLabel;
        private System.Windows.Forms.Label AddedRhythmLabel;
        private System.Windows.Forms.Button DirectStop;
        private System.Windows.Forms.ComboBox COMComboBox;
        private System.Windows.Forms.Label SelectPortLabel;
        private System.Windows.Forms.Button RefreshPorts;
        private System.Windows.Forms.TextBox DirectRenameField;
        private System.Windows.Forms.Button DirectRenameSet;
        private System.Windows.Forms.Button ClosePort;
        private System.Windows.Forms.Button OpenPort;
        private System.Windows.Forms.Label ErrorStatus;
        private System.Windows.Forms.Label ErrorLocation;
        private System.Windows.Forms.Button DirectBack;
        private System.Windows.Forms.Button RhythmTestStop;
        private System.Windows.Forms.Button MagTestStop;
        private System.Windows.Forms.Button DirectRenameGroup;
        private System.Windows.Forms.ListBox GroupList;
        private System.Windows.Forms.Button DirectAddSet;
        private System.Windows.Forms.Label DirectGroupLabel;
        private System.Windows.Forms.Button DirectAddGroup;
        private System.Windows.Forms.Button DirectDeleteGroup;
        private System.Windows.Forms.Button DirectClearGroup;
        private System.Windows.Forms.Button DirectClearSet;
        private System.Windows.Forms.Button DirectDeleteSet;
        private System.Windows.Forms.Button DirectActivateMotor;
        private System.Windows.Forms.Button DirectActivateGroup;
        private System.Windows.Forms.Label DirectRenameLabel;
        private System.Windows.Forms.Button DirectSave;
        private System.Windows.Forms.Button DirectLoad;

    }
}

