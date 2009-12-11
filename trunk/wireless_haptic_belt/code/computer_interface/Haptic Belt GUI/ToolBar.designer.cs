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
            this.MainPanel = new System.Windows.Forms.Panel();
            this.MagnitudeEditOK = new System.Windows.Forms.Button();
            this.RhythmEditOK = new System.Windows.Forms.Button();
            this.EditMagnitudeBox = new System.Windows.Forms.ComboBox();
            this.EditRhythmBox = new System.Windows.Forms.ComboBox();
            this.AddedDelayLabel = new System.Windows.Forms.Label();
            this.DelayLabel = new System.Windows.Forms.Label();
            this.AddDelayField = new System.Windows.Forms.NumericUpDown();
            this.RenameLabel = new System.Windows.Forms.Label();
            this.ActivateMotor = new System.Windows.Forms.Button();
            this.ActivateGroup = new System.Windows.Forms.Button();
            this.DirectGroupLabel = new System.Windows.Forms.Label();
            this.AddGroup = new System.Windows.Forms.Button();
            this.DeleteGroup = new System.Windows.Forms.Button();
            this.ClearGroup = new System.Windows.Forms.Button();
            this.ClearSet = new System.Windows.Forms.Button();
            this.DeleteSet = new System.Windows.Forms.Button();
            this.AddSet = new System.Windows.Forms.Button();
            this.GroupList = new System.Windows.Forms.ListBox();
            this.RenameGroup = new System.Windows.Forms.Button();
            this.RenameField = new System.Windows.Forms.TextBox();
            this.RenameSet = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.DirectSetLabel = new System.Windows.Forms.Label();
            this.AddedCycleLabel = new System.Windows.Forms.Label();
            this.AddedMagLabel = new System.Windows.Forms.Label();
            this.AddedRhythmLabel = new System.Windows.Forms.Label();
            this.ClearMotor = new System.Windows.Forms.Button();
            this.ActivateSet = new System.Windows.Forms.Button();
            this.DeleteMotor = new System.Windows.Forms.Button();
            this.SetMotor = new System.Windows.Forms.Button();
            this.CyclesLabel = new System.Windows.Forms.Label();
            this.MagLabel = new System.Windows.Forms.Label();
            this.RhythmLabel = new System.Windows.Forms.Label();
            this.AddCyclesBox = new System.Windows.Forms.ComboBox();
            this.AddMagBox = new System.Windows.Forms.ComboBox();
            this.AddRhythmBox = new System.Windows.Forms.ComboBox();
            this.MotorLabel = new System.Windows.Forms.Label();
            this.SetList = new System.Windows.Forms.ListBox();
            this.MotorList = new System.Windows.Forms.ListBox();
            this.loadBinaryFile = new System.Windows.Forms.OpenFileDialog();
            this.saveBinaryFile = new System.Windows.Forms.SaveFileDialog();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.connectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshPortsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.COMComboBoxMenu = new System.Windows.Forms.ToolStripComboBox();
            this.loadMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.showOnlyConnectedMotorsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.motorSwapingOnAllGroupsSetsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.versionMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.guiVersionMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.firmwareVersionMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddDelayField)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.MagnitudeEditOK);
            this.MainPanel.Controls.Add(this.RhythmEditOK);
            this.MainPanel.Controls.Add(this.EditMagnitudeBox);
            this.MainPanel.Controls.Add(this.EditRhythmBox);
            this.MainPanel.Controls.Add(this.AddedDelayLabel);
            this.MainPanel.Controls.Add(this.DelayLabel);
            this.MainPanel.Controls.Add(this.AddDelayField);
            this.MainPanel.Controls.Add(this.RenameLabel);
            this.MainPanel.Controls.Add(this.ActivateMotor);
            this.MainPanel.Controls.Add(this.ActivateGroup);
            this.MainPanel.Controls.Add(this.DirectGroupLabel);
            this.MainPanel.Controls.Add(this.AddGroup);
            this.MainPanel.Controls.Add(this.DeleteGroup);
            this.MainPanel.Controls.Add(this.ClearGroup);
            this.MainPanel.Controls.Add(this.ClearSet);
            this.MainPanel.Controls.Add(this.DeleteSet);
            this.MainPanel.Controls.Add(this.AddSet);
            this.MainPanel.Controls.Add(this.GroupList);
            this.MainPanel.Controls.Add(this.RenameGroup);
            this.MainPanel.Controls.Add(this.RenameField);
            this.MainPanel.Controls.Add(this.RenameSet);
            this.MainPanel.Controls.Add(this.Stop);
            this.MainPanel.Controls.Add(this.DirectSetLabel);
            this.MainPanel.Controls.Add(this.AddedCycleLabel);
            this.MainPanel.Controls.Add(this.AddedMagLabel);
            this.MainPanel.Controls.Add(this.AddedRhythmLabel);
            this.MainPanel.Controls.Add(this.ClearMotor);
            this.MainPanel.Controls.Add(this.ActivateSet);
            this.MainPanel.Controls.Add(this.DeleteMotor);
            this.MainPanel.Controls.Add(this.SetMotor);
            this.MainPanel.Controls.Add(this.CyclesLabel);
            this.MainPanel.Controls.Add(this.MagLabel);
            this.MainPanel.Controls.Add(this.RhythmLabel);
            this.MainPanel.Controls.Add(this.AddCyclesBox);
            this.MainPanel.Controls.Add(this.AddMagBox);
            this.MainPanel.Controls.Add(this.AddRhythmBox);
            this.MainPanel.Controls.Add(this.MotorLabel);
            this.MainPanel.Controls.Add(this.SetList);
            this.MainPanel.Controls.Add(this.MotorList);
            this.MainPanel.Location = new System.Drawing.Point(12, 27);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(539, 263);
            this.MainPanel.TabIndex = 27;
            // 
            // MagnitudeEditOK
            // 
            this.MagnitudeEditOK.Location = new System.Drawing.Point(444, 166);
            this.MagnitudeEditOK.Name = "MagnitudeEditOK";
            this.MagnitudeEditOK.Size = new System.Drawing.Size(80, 20);
            this.MagnitudeEditOK.TabIndex = 47;
            this.MagnitudeEditOK.Text = "OK";
            this.MagnitudeEditOK.UseVisualStyleBackColor = true;
            this.MagnitudeEditOK.Click += new System.EventHandler(this.MagnitudeEditOK_Click);
            // 
            // RhythmEditOK
            // 
            this.RhythmEditOK.Location = new System.Drawing.Point(444, 108);
            this.RhythmEditOK.Name = "RhythmEditOK";
            this.RhythmEditOK.Size = new System.Drawing.Size(80, 20);
            this.RhythmEditOK.TabIndex = 46;
            this.RhythmEditOK.Text = "OK";
            this.RhythmEditOK.UseVisualStyleBackColor = true;
            this.RhythmEditOK.Click += new System.EventHandler(this.RhythmEditOK_Click);
            // 
            // EditMagnitudeBox
            // 
            this.EditMagnitudeBox.FormattingEnabled = true;
            this.EditMagnitudeBox.Items.AddRange(new object[] {
            "Edit Magnitudes"});
            this.EditMagnitudeBox.Location = new System.Drawing.Point(436, 139);
            this.EditMagnitudeBox.Name = "EditMagnitudeBox";
            this.EditMagnitudeBox.Size = new System.Drawing.Size(100, 21);
            this.EditMagnitudeBox.TabIndex = 45;
            this.EditMagnitudeBox.Text = "Edit Magnitudes";
            // 
            // EditRhythmBox
            // 
            this.EditRhythmBox.FormattingEnabled = true;
            this.EditRhythmBox.Items.AddRange(new object[] {
            "Edit Rhythms"});
            this.EditRhythmBox.Location = new System.Drawing.Point(436, 82);
            this.EditRhythmBox.Name = "EditRhythmBox";
            this.EditRhythmBox.Size = new System.Drawing.Size(100, 21);
            this.EditRhythmBox.TabIndex = 44;
            this.EditRhythmBox.Text = "Edit Rhythms";
            // 
            // AddedDelayLabel
            // 
            this.AddedDelayLabel.AutoSize = true;
            this.AddedDelayLabel.Location = new System.Drawing.Point(271, 43);
            this.AddedDelayLabel.Name = "AddedDelayLabel";
            this.AddedDelayLabel.Size = new System.Drawing.Size(27, 13);
            this.AddedDelayLabel.TabIndex = 42;
            this.AddedDelayLabel.Text = "N/A";
            // 
            // DelayLabel
            // 
            this.DelayLabel.AutoSize = true;
            this.DelayLabel.Location = new System.Drawing.Point(278, 3);
            this.DelayLabel.Name = "DelayLabel";
            this.DelayLabel.Size = new System.Drawing.Size(34, 13);
            this.DelayLabel.TabIndex = 41;
            this.DelayLabel.Text = "Delay";
            // 
            // AddDelayField
            // 
            this.AddDelayField.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.AddDelayField.Location = new System.Drawing.Point(270, 16);
            this.AddDelayField.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.AddDelayField.Name = "AddDelayField";
            this.AddDelayField.Size = new System.Drawing.Size(55, 20);
            this.AddDelayField.TabIndex = 40;
            this.AddDelayField.ValueChanged += new System.EventHandler(this.DirectDelayField_ValueChanged);
            // 
            // RenameLabel
            // 
            this.RenameLabel.AutoSize = true;
            this.RenameLabel.Location = new System.Drawing.Point(353, 3);
            this.RenameLabel.Name = "RenameLabel";
            this.RenameLabel.Size = new System.Drawing.Size(60, 13);
            this.RenameLabel.TabIndex = 30;
            this.RenameLabel.Text = "Name Field";
            // 
            // ActivateMotor
            // 
            this.ActivateMotor.Enabled = false;
            this.ActivateMotor.Location = new System.Drawing.Point(67, 183);
            this.ActivateMotor.Name = "ActivateMotor";
            this.ActivateMotor.Size = new System.Drawing.Size(60, 20);
            this.ActivateMotor.TabIndex = 37;
            this.ActivateMotor.Text = "Activate";
            this.ActivateMotor.UseVisualStyleBackColor = true;
            this.ActivateMotor.Click += new System.EventHandler(this.DirectActivateMotor_Click);
            // 
            // ActivateGroup
            // 
            this.ActivateGroup.Enabled = false;
            this.ActivateGroup.Location = new System.Drawing.Point(370, 183);
            this.ActivateGroup.Name = "ActivateGroup";
            this.ActivateGroup.Size = new System.Drawing.Size(60, 20);
            this.ActivateGroup.TabIndex = 36;
            this.ActivateGroup.Text = "Activate";
            this.ActivateGroup.UseVisualStyleBackColor = true;
            this.ActivateGroup.Click += new System.EventHandler(this.DirectActivateGroup_Click);
            // 
            // DirectGroupLabel
            // 
            this.DirectGroupLabel.AutoSize = true;
            this.DirectGroupLabel.Location = new System.Drawing.Point(304, 69);
            this.DirectGroupLabel.Name = "DirectGroupLabel";
            this.DirectGroupLabel.Size = new System.Drawing.Size(36, 13);
            this.DirectGroupLabel.TabIndex = 35;
            this.DirectGroupLabel.Text = "Group";
            // 
            // AddGroup
            // 
            this.AddGroup.Location = new System.Drawing.Point(370, 82);
            this.AddGroup.Name = "AddGroup";
            this.AddGroup.Size = new System.Drawing.Size(60, 20);
            this.AddGroup.TabIndex = 34;
            this.AddGroup.Text = "Add";
            this.AddGroup.UseVisualStyleBackColor = true;
            this.AddGroup.Click += new System.EventHandler(this.DirectAddGroup_Click);
            // 
            // DeleteGroup
            // 
            this.DeleteGroup.Location = new System.Drawing.Point(370, 118);
            this.DeleteGroup.Name = "DeleteGroup";
            this.DeleteGroup.Size = new System.Drawing.Size(60, 20);
            this.DeleteGroup.TabIndex = 33;
            this.DeleteGroup.Text = "Delete";
            this.DeleteGroup.UseVisualStyleBackColor = true;
            this.DeleteGroup.Click += new System.EventHandler(this.DirectDeleteGroup_Click);
            // 
            // ClearGroup
            // 
            this.ClearGroup.Location = new System.Drawing.Point(370, 139);
            this.ClearGroup.Name = "ClearGroup";
            this.ClearGroup.Size = new System.Drawing.Size(60, 20);
            this.ClearGroup.TabIndex = 32;
            this.ClearGroup.Text = "Clear";
            this.ClearGroup.UseVisualStyleBackColor = true;
            this.ClearGroup.Click += new System.EventHandler(this.DirectClearGroups_Click);
            // 
            // ClearSet
            // 
            this.ClearSet.Location = new System.Drawing.Point(218, 139);
            this.ClearSet.Name = "ClearSet";
            this.ClearSet.Size = new System.Drawing.Size(60, 20);
            this.ClearSet.TabIndex = 31;
            this.ClearSet.Text = "Clear";
            this.ClearSet.UseVisualStyleBackColor = true;
            this.ClearSet.Click += new System.EventHandler(this.DirectClearSets_Click);
            // 
            // DeleteSet
            // 
            this.DeleteSet.Location = new System.Drawing.Point(218, 118);
            this.DeleteSet.Name = "DeleteSet";
            this.DeleteSet.Size = new System.Drawing.Size(60, 20);
            this.DeleteSet.TabIndex = 30;
            this.DeleteSet.Text = "Delete";
            this.DeleteSet.UseVisualStyleBackColor = true;
            this.DeleteSet.Click += new System.EventHandler(this.DirectDeleteSet_Click);
            // 
            // AddSet
            // 
            this.AddSet.Location = new System.Drawing.Point(218, 82);
            this.AddSet.Name = "AddSet";
            this.AddSet.Size = new System.Drawing.Size(60, 20);
            this.AddSet.TabIndex = 29;
            this.AddSet.Text = "Add";
            this.AddSet.UseVisualStyleBackColor = true;
            this.AddSet.Click += new System.EventHandler(this.DirectAddSet_Click);
            // 
            // GroupList
            // 
            this.GroupList.FormattingEnabled = true;
            this.GroupList.Location = new System.Drawing.Point(285, 82);
            this.GroupList.Name = "GroupList";
            this.GroupList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.GroupList.Size = new System.Drawing.Size(79, 147);
            this.GroupList.TabIndex = 28;
            this.GroupList.SelectedIndexChanged += new System.EventHandler(this.GroupList_SelectedIndexChanged);
            // 
            // RenameGroup
            // 
            this.RenameGroup.Location = new System.Drawing.Point(341, 54);
            this.RenameGroup.Name = "RenameGroup";
            this.RenameGroup.Size = new System.Drawing.Size(89, 18);
            this.RenameGroup.TabIndex = 27;
            this.RenameGroup.Text = "Rename Group";
            this.RenameGroup.UseCompatibleTextRendering = true;
            this.RenameGroup.UseVisualStyleBackColor = true;
            this.RenameGroup.Click += new System.EventHandler(this.DirectRenameGroup_Click);
            // 
            // RenameField
            // 
            this.RenameField.Location = new System.Drawing.Point(341, 16);
            this.RenameField.MaxLength = 10;
            this.RenameField.Name = "RenameField";
            this.RenameField.Size = new System.Drawing.Size(89, 20);
            this.RenameField.TabIndex = 25;
            // 
            // RenameSet
            // 
            this.RenameSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RenameSet.Location = new System.Drawing.Point(341, 37);
            this.RenameSet.Name = "RenameSet";
            this.RenameSet.Size = new System.Drawing.Size(89, 18);
            this.RenameSet.TabIndex = 24;
            this.RenameSet.Text = "Rename Set";
            this.RenameSet.UseCompatibleTextRendering = true;
            this.RenameSet.UseVisualStyleBackColor = true;
            this.RenameSet.Click += new System.EventHandler(this.DirectRenameSet_Click);
            // 
            // Stop
            // 
            this.Stop.Enabled = false;
            this.Stop.Location = new System.Drawing.Point(449, 237);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(75, 23);
            this.Stop.TabIndex = 23;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.DirectStop_Click);
            // 
            // DirectSetLabel
            // 
            this.DirectSetLabel.AutoSize = true;
            this.DirectSetLabel.Location = new System.Drawing.Point(161, 69);
            this.DirectSetLabel.Name = "DirectSetLabel";
            this.DirectSetLabel.Size = new System.Drawing.Size(23, 13);
            this.DirectSetLabel.TabIndex = 5;
            this.DirectSetLabel.Text = "Set";
            // 
            // AddedCycleLabel
            // 
            this.AddedCycleLabel.AutoSize = true;
            this.AddedCycleLabel.Location = new System.Drawing.Point(207, 43);
            this.AddedCycleLabel.Name = "AddedCycleLabel";
            this.AddedCycleLabel.Size = new System.Drawing.Size(27, 13);
            this.AddedCycleLabel.TabIndex = 22;
            this.AddedCycleLabel.Text = "N/A";
            // 
            // AddedMagLabel
            // 
            this.AddedMagLabel.AutoSize = true;
            this.AddedMagLabel.Location = new System.Drawing.Point(144, 43);
            this.AddedMagLabel.Name = "AddedMagLabel";
            this.AddedMagLabel.Size = new System.Drawing.Size(27, 13);
            this.AddedMagLabel.TabIndex = 21;
            this.AddedMagLabel.Text = "N/A";
            // 
            // AddedRhythmLabel
            // 
            this.AddedRhythmLabel.AutoSize = true;
            this.AddedRhythmLabel.Location = new System.Drawing.Point(78, 43);
            this.AddedRhythmLabel.Name = "AddedRhythmLabel";
            this.AddedRhythmLabel.Size = new System.Drawing.Size(27, 13);
            this.AddedRhythmLabel.TabIndex = 20;
            this.AddedRhythmLabel.Text = "N/A";
            // 
            // ClearMotor
            // 
            this.ClearMotor.Location = new System.Drawing.Point(67, 139);
            this.ClearMotor.Name = "ClearMotor";
            this.ClearMotor.Size = new System.Drawing.Size(60, 20);
            this.ClearMotor.TabIndex = 19;
            this.ClearMotor.Text = "Clear";
            this.ClearMotor.UseVisualStyleBackColor = true;
            this.ClearMotor.Click += new System.EventHandler(this.DirectClearMotor_Click);
            // 
            // ActivateSet
            // 
            this.ActivateSet.Enabled = false;
            this.ActivateSet.Location = new System.Drawing.Point(218, 183);
            this.ActivateSet.Name = "ActivateSet";
            this.ActivateSet.Size = new System.Drawing.Size(60, 20);
            this.ActivateSet.TabIndex = 15;
            this.ActivateSet.Text = "Activate";
            this.ActivateSet.UseVisualStyleBackColor = true;
            // 
            // DeleteMotor
            // 
            this.DeleteMotor.Location = new System.Drawing.Point(67, 118);
            this.DeleteMotor.Name = "DeleteMotor";
            this.DeleteMotor.Size = new System.Drawing.Size(60, 20);
            this.DeleteMotor.TabIndex = 14;
            this.DeleteMotor.Text = "Delete";
            this.DeleteMotor.UseVisualStyleBackColor = true;
            this.DeleteMotor.Click += new System.EventHandler(this.DirectDeleteMotor_Click);
            // 
            // SetMotor
            // 
            this.SetMotor.Location = new System.Drawing.Point(67, 82);
            this.SetMotor.Name = "SetMotor";
            this.SetMotor.Size = new System.Drawing.Size(60, 20);
            this.SetMotor.TabIndex = 13;
            this.SetMotor.Text = "Set";
            this.SetMotor.UseVisualStyleBackColor = true;
            this.SetMotor.Click += new System.EventHandler(this.DirectSetMotor_Click);
            // 
            // CyclesLabel
            // 
            this.CyclesLabel.AutoSize = true;
            this.CyclesLabel.Location = new System.Drawing.Point(210, 3);
            this.CyclesLabel.Name = "CyclesLabel";
            this.CyclesLabel.Size = new System.Drawing.Size(38, 13);
            this.CyclesLabel.TabIndex = 12;
            this.CyclesLabel.Text = "Cycles";
            // 
            // MagLabel
            // 
            this.MagLabel.AutoSize = true;
            this.MagLabel.Location = new System.Drawing.Point(138, 3);
            this.MagLabel.Name = "MagLabel";
            this.MagLabel.Size = new System.Drawing.Size(57, 13);
            this.MagLabel.TabIndex = 11;
            this.MagLabel.Text = "Magnitude";
            // 
            // RhythmLabel
            // 
            this.RhythmLabel.AutoSize = true;
            this.RhythmLabel.Location = new System.Drawing.Point(79, 3);
            this.RhythmLabel.Name = "RhythmLabel";
            this.RhythmLabel.Size = new System.Drawing.Size(43, 13);
            this.RhythmLabel.TabIndex = 10;
            this.RhythmLabel.Text = "Rhythm";
            // 
            // AddCyclesBox
            // 
            this.AddCyclesBox.FormattingEnabled = true;
            this.AddCyclesBox.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "Inf"});
            this.AddCyclesBox.Location = new System.Drawing.Point(207, 16);
            this.AddCyclesBox.Name = "AddCyclesBox";
            this.AddCyclesBox.Size = new System.Drawing.Size(45, 21);
            this.AddCyclesBox.TabIndex = 9;
            this.AddCyclesBox.Text = "0";
            // 
            // AddMagBox
            // 
            this.AddMagBox.FormattingEnabled = true;
            this.AddMagBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D"});
            this.AddMagBox.Location = new System.Drawing.Point(144, 16);
            this.AddMagBox.Name = "AddMagBox";
            this.AddMagBox.Size = new System.Drawing.Size(45, 21);
            this.AddMagBox.TabIndex = 8;
            this.AddMagBox.Text = "A";
            // 
            // AddRhythmBox
            // 
            this.AddRhythmBox.FormattingEnabled = true;
            this.AddRhythmBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E"});
            this.AddRhythmBox.Location = new System.Drawing.Point(78, 16);
            this.AddRhythmBox.Name = "AddRhythmBox";
            this.AddRhythmBox.Size = new System.Drawing.Size(45, 21);
            this.AddRhythmBox.TabIndex = 7;
            this.AddRhythmBox.Text = "A";
            // 
            // MotorLabel
            // 
            this.MotorLabel.AutoSize = true;
            this.MotorLabel.Location = new System.Drawing.Point(17, 3);
            this.MotorLabel.Name = "MotorLabel";
            this.MotorLabel.Size = new System.Drawing.Size(44, 13);
            this.MotorLabel.TabIndex = 3;
            this.MotorLabel.Text = "Off   On";
            // 
            // SetList
            // 
            this.SetList.FormattingEnabled = true;
            this.SetList.Location = new System.Drawing.Point(133, 82);
            this.SetList.Name = "SetList";
            this.SetList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.SetList.Size = new System.Drawing.Size(79, 147);
            this.SetList.TabIndex = 2;
            this.SetList.SelectedIndexChanged += new System.EventHandler(this.SetList_SelectedIndexChanged);
            // 
            // MotorList
            // 
            this.MotorList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.MotorList.FormattingEnabled = true;
            this.MotorList.Location = new System.Drawing.Point(16, 17);
            this.MotorList.Name = "MotorList";
            this.MotorList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.MotorList.Size = new System.Drawing.Size(45, 212);
            this.MotorList.TabIndex = 1;
            this.MotorList.SelectedIndexChanged += new System.EventHandler(this.MotorList_SelectedIndexChanged);
            // 
            // loadBinaryFile
            // 
            this.loadBinaryFile.DefaultExt = "hbg";
            this.loadBinaryFile.Filter = "GUI Binary Files|*.hbg|All files|*";
            this.loadBinaryFile.FileOk += new System.ComponentModel.CancelEventHandler(this.loadBinaryFile_FileOk);
            // 
            // saveBinaryFile
            // 
            this.saveBinaryFile.DefaultExt = "hbg";
            this.saveBinaryFile.Filter = "GUI Binary Files|*.hbg|All files|*";
            this.saveBinaryFile.FileOk += new System.ComponentModel.CancelEventHandler(this.saveBinaryFile_FileOk);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.optionsMenu,
            this.versionMenu});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(564, 24);
            this.MenuStrip.TabIndex = 30;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectMenu,
            this.loadMenu,
            this.saveMenu});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            // 
            // connectMenu
            // 
            this.connectMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disconnectMenu,
            this.refreshPortsMenu,
            this.COMComboBoxMenu});
            this.connectMenu.Name = "connectMenu";
            this.connectMenu.Size = new System.Drawing.Size(119, 22);
            this.connectMenu.Text = "Connect";
            // 
            // disconnectMenu
            // 
            this.disconnectMenu.Enabled = false;
            this.disconnectMenu.Name = "disconnectMenu";
            this.disconnectMenu.Size = new System.Drawing.Size(181, 22);
            this.disconnectMenu.Text = "Disconnect";
            this.disconnectMenu.Click += new System.EventHandler(this.disconnectMenu_Click);
            // 
            // refreshPortsMenu
            // 
            this.refreshPortsMenu.Name = "refreshPortsMenu";
            this.refreshPortsMenu.Size = new System.Drawing.Size(181, 22);
            this.refreshPortsMenu.Text = "Refresh Ports";
            this.refreshPortsMenu.Click += new System.EventHandler(this.refreshPortsMenu_Click);
            // 
            // COMComboBoxMenu
            // 
            this.COMComboBoxMenu.Name = "COMComboBoxMenu";
            this.COMComboBoxMenu.Size = new System.Drawing.Size(121, 23);
            this.COMComboBoxMenu.Text = "Port List";
            this.COMComboBoxMenu.Click += new System.EventHandler(this.COMComboBoxMenu_Click);
            // 
            // loadMenu
            // 
            this.loadMenu.Name = "loadMenu";
            this.loadMenu.Size = new System.Drawing.Size(119, 22);
            this.loadMenu.Text = "Load";
            this.loadMenu.Click += new System.EventHandler(this.loadMenu_Click);
            // 
            // saveMenu
            // 
            this.saveMenu.Name = "saveMenu";
            this.saveMenu.Size = new System.Drawing.Size(119, 22);
            this.saveMenu.Text = "Save";
            this.saveMenu.Click += new System.EventHandler(this.saveMenu_Click);
            // 
            // optionsMenu
            // 
            this.optionsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOnlyConnectedMotorsMenu,
            this.motorSwapingOnAllGroupsSetsMenu});
            this.optionsMenu.Name = "optionsMenu";
            this.optionsMenu.Size = new System.Drawing.Size(61, 20);
            this.optionsMenu.Text = "Options";
            // 
            // showOnlyConnectedMotorsMenu
            // 
            this.showOnlyConnectedMotorsMenu.Checked = true;
            this.showOnlyConnectedMotorsMenu.CheckOnClick = true;
            this.showOnlyConnectedMotorsMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showOnlyConnectedMotorsMenu.Name = "showOnlyConnectedMotorsMenu";
            this.showOnlyConnectedMotorsMenu.Size = new System.Drawing.Size(258, 22);
            this.showOnlyConnectedMotorsMenu.Text = "Show Only Connected Motors";
            this.showOnlyConnectedMotorsMenu.Click += new System.EventHandler(this.showOnlyConnectedMotorsMenu_Click);
            // 
            // motorSwapingOnAllGroupsSetsMenu
            // 
            this.motorSwapingOnAllGroupsSetsMenu.Checked = true;
            this.motorSwapingOnAllGroupsSetsMenu.CheckOnClick = true;
            this.motorSwapingOnAllGroupsSetsMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.motorSwapingOnAllGroupsSetsMenu.Name = "motorSwapingOnAllGroupsSetsMenu";
            this.motorSwapingOnAllGroupsSetsMenu.Size = new System.Drawing.Size(258, 22);
            this.motorSwapingOnAllGroupsSetsMenu.Text = "Motor Swaping On All Groups/Sets";
            // 
            // versionMenu
            // 
            this.versionMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.guiVersionMenu,
            this.firmwareVersionMenu});
            this.versionMenu.Name = "versionMenu";
            this.versionMenu.Size = new System.Drawing.Size(58, 20);
            this.versionMenu.Text = "Version";
            // 
            // guiVersionMenu
            // 
            this.guiVersionMenu.Name = "guiVersionMenu";
            this.guiVersionMenu.Size = new System.Drawing.Size(151, 22);
            this.guiVersionMenu.Text = "GUI: 1.0";
            // 
            // firmwareVersionMenu
            // 
            this.firmwareVersionMenu.Name = "firmwareVersionMenu";
            this.firmwareVersionMenu.Size = new System.Drawing.Size(151, 22);
            this.firmwareVersionMenu.Text = "Firmware: N/A";
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(564, 298);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.MenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.MenuStrip;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GUI";
            this.Text = "Haptic Belt GUI";
            this.Load += new System.EventHandler(this.GUI_Load);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddDelayField)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.ListBox MotorList;
        private System.Windows.Forms.Label DirectSetLabel;
        private System.Windows.Forms.Label MotorLabel;
        private System.Windows.Forms.ListBox SetList;
        private System.Windows.Forms.ComboBox AddCyclesBox;
        private System.Windows.Forms.ComboBox AddMagBox;
        private System.Windows.Forms.ComboBox AddRhythmBox;
        private System.Windows.Forms.Label RhythmLabel;
        private System.Windows.Forms.Label CyclesLabel;
        private System.Windows.Forms.Label MagLabel;
        private System.Windows.Forms.Button DeleteMotor;
        private System.Windows.Forms.Button SetMotor;
        private System.Windows.Forms.Button ActivateSet;
        private System.Windows.Forms.Button ClearMotor;
        private System.Windows.Forms.Label AddedCycleLabel;
        private System.Windows.Forms.Label AddedMagLabel;
        private System.Windows.Forms.Label AddedRhythmLabel;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.TextBox RenameField;
        private System.Windows.Forms.Button RenameSet;
        private System.Windows.Forms.Button RenameGroup;
        private System.Windows.Forms.ListBox GroupList;
        private System.Windows.Forms.Button AddSet;
        private System.Windows.Forms.Label DirectGroupLabel;
        private System.Windows.Forms.Button AddGroup;
        private System.Windows.Forms.Button DeleteGroup;
        private System.Windows.Forms.Button ClearGroup;
        private System.Windows.Forms.Button ClearSet;
        private System.Windows.Forms.Button DeleteSet;
        private System.Windows.Forms.Button ActivateMotor;
        private System.Windows.Forms.Button ActivateGroup;
        private System.Windows.Forms.Label RenameLabel;
        private System.Windows.Forms.Label DelayLabel;
        private System.Windows.Forms.NumericUpDown AddDelayField;
        private System.Windows.Forms.Label AddedDelayLabel;
        private System.Windows.Forms.OpenFileDialog loadBinaryFile;
        private System.Windows.Forms.SaveFileDialog saveBinaryFile;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem connectMenu;
        private System.Windows.Forms.ToolStripMenuItem disconnectMenu;
        private System.Windows.Forms.ToolStripMenuItem loadMenu;
        private System.Windows.Forms.ToolStripMenuItem saveMenu;
        private System.Windows.Forms.ToolStripComboBox COMComboBoxMenu;
        private System.Windows.Forms.ToolStripMenuItem refreshPortsMenu;
        private System.Windows.Forms.ToolStripMenuItem optionsMenu;
        private System.Windows.Forms.ToolStripMenuItem showOnlyConnectedMotorsMenu;
        private System.Windows.Forms.ToolStripMenuItem motorSwapingOnAllGroupsSetsMenu;
        private System.Windows.Forms.ToolStripMenuItem versionMenu;
        private System.Windows.Forms.ToolStripMenuItem guiVersionMenu;
        private System.Windows.Forms.ToolStripMenuItem firmwareVersionMenu;
        private System.Windows.Forms.ComboBox EditMagnitudeBox;
        private System.Windows.Forms.ComboBox EditRhythmBox;
        private System.Windows.Forms.Button RhythmEditOK;
        private System.Windows.Forms.Button MagnitudeEditOK;

    }
}

