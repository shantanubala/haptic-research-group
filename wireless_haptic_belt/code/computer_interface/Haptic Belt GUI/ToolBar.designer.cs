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
            this.AddEvent = new System.Windows.Forms.Button();
            this.AddMotorBox = new System.Windows.Forms.ComboBox();
            this.RepetitionsLabel = new System.Windows.Forms.Label();
            this.RepetitionsField = new System.Windows.Forms.NumericUpDown();
            this.RenameGroup = new System.Windows.Forms.Button();
            this.ActivateEvent = new System.Windows.Forms.Button();
            this.EventLabel = new System.Windows.Forms.Label();
            this.DeleteEvent = new System.Windows.Forms.Button();
            this.ClearEvent = new System.Windows.Forms.Button();
            this.EventList = new System.Windows.Forms.ListBox();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.ActivationList = new System.Windows.Forms.ListBox();
            this.Initialize = new System.Windows.Forms.Button();
            this.MagnitudeEditOK = new System.Windows.Forms.Button();
            this.RhythmEditOK = new System.Windows.Forms.Button();
            this.EditBox = new System.Windows.Forms.ComboBox();
            this.DelayField = new System.Windows.Forms.NumericUpDown();
            this.ActivateActivation = new System.Windows.Forms.Button();
            this.ActivateGroup = new System.Windows.Forms.Button();
            this.GroupLabel = new System.Windows.Forms.Label();
            this.AddGroup = new System.Windows.Forms.Button();
            this.DeleteGroup = new System.Windows.Forms.Button();
            this.ClearGroup = new System.Windows.Forms.Button();
            this.GroupList = new System.Windows.Forms.ListBox();
            this.RenameField = new System.Windows.Forms.TextBox();
            this.Stop = new System.Windows.Forms.Button();
            this.ClearActivation = new System.Windows.Forms.Button();
            this.DeleteActivation = new System.Windows.Forms.Button();
            this.SetActivation = new System.Windows.Forms.Button();
            this.AddCyclesBox = new System.Windows.Forms.ComboBox();
            this.AddMagBox = new System.Windows.Forms.ComboBox();
            this.AddRhythmBox = new System.Windows.Forms.ComboBox();
            this.loadBinaryFile = new System.Windows.Forms.OpenFileDialog();
            this.saveBinaryFile = new System.Windows.Forms.SaveFileDialog();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.connectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.connect = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshPorts = new System.Windows.Forms.ToolStripMenuItem();
            this.outgoingCOMComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.incomingCOMComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.load = new System.Windows.Forms.ToolStripMenuItem();
            this.save = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.showOnlyConnectedMotorsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.motorSwapingOnAllGroupsSetsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.realTimeDelayMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.realTimeDelayValueMenu = new System.Windows.Forms.ToolStripTextBox();
            this.versionMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.guiVersionMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.firmwareVersionMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RepetitionsField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayField)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.AddEvent);
            this.MainPanel.Controls.Add(this.AddMotorBox);
            this.MainPanel.Controls.Add(this.RepetitionsLabel);
            this.MainPanel.Controls.Add(this.RepetitionsField);
            this.MainPanel.Controls.Add(this.RenameGroup);
            this.MainPanel.Controls.Add(this.ActivateEvent);
            this.MainPanel.Controls.Add(this.EventLabel);
            this.MainPanel.Controls.Add(this.DeleteEvent);
            this.MainPanel.Controls.Add(this.ClearEvent);
            this.MainPanel.Controls.Add(this.EventList);
            this.MainPanel.Controls.Add(this.HeaderLabel);
            this.MainPanel.Controls.Add(this.ActivationList);
            this.MainPanel.Controls.Add(this.Initialize);
            this.MainPanel.Controls.Add(this.MagnitudeEditOK);
            this.MainPanel.Controls.Add(this.RhythmEditOK);
            this.MainPanel.Controls.Add(this.EditBox);
            this.MainPanel.Controls.Add(this.DelayField);
            this.MainPanel.Controls.Add(this.ActivateActivation);
            this.MainPanel.Controls.Add(this.ActivateGroup);
            this.MainPanel.Controls.Add(this.GroupLabel);
            this.MainPanel.Controls.Add(this.AddGroup);
            this.MainPanel.Controls.Add(this.DeleteGroup);
            this.MainPanel.Controls.Add(this.ClearGroup);
            this.MainPanel.Controls.Add(this.GroupList);
            this.MainPanel.Controls.Add(this.RenameField);
            this.MainPanel.Controls.Add(this.Stop);
            this.MainPanel.Controls.Add(this.ClearActivation);
            this.MainPanel.Controls.Add(this.DeleteActivation);
            this.MainPanel.Controls.Add(this.SetActivation);
            this.MainPanel.Controls.Add(this.AddCyclesBox);
            this.MainPanel.Controls.Add(this.AddMagBox);
            this.MainPanel.Controls.Add(this.AddRhythmBox);
            this.MainPanel.Location = new System.Drawing.Point(12, 27);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(607, 263);
            this.MainPanel.TabIndex = 27;
            // 
            // AddEvent
            // 
            this.AddEvent.Location = new System.Drawing.Point(259, 56);
            this.AddEvent.Name = "AddEvent";
            this.AddEvent.Size = new System.Drawing.Size(60, 20);
            this.AddEvent.TabIndex = 61;
            this.AddEvent.Text = "Add";
            this.AddEvent.UseVisualStyleBackColor = true;
            this.AddEvent.Click += new System.EventHandler(this.AddEvent_Click);
            // 
            // AddMotorBox
            // 
            this.AddMotorBox.FormattingEnabled = true;
            this.AddMotorBox.Location = new System.Drawing.Point(124, 17);
            this.AddMotorBox.Name = "AddMotorBox";
            this.AddMotorBox.Size = new System.Drawing.Size(45, 21);
            this.AddMotorBox.TabIndex = 60;
            // 
            // RepetitionsLabel
            // 
            this.RepetitionsLabel.AutoSize = true;
            this.RepetitionsLabel.Location = new System.Drawing.Point(411, 169);
            this.RepetitionsLabel.Name = "RepetitionsLabel";
            this.RepetitionsLabel.Size = new System.Drawing.Size(60, 13);
            this.RepetitionsLabel.TabIndex = 59;
            this.RepetitionsLabel.Text = "Repetitions";
            // 
            // RepetitionsField
            // 
            this.RepetitionsField.Location = new System.Drawing.Point(421, 183);
            this.RepetitionsField.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.RepetitionsField.Name = "RepetitionsField";
            this.RepetitionsField.Size = new System.Drawing.Size(40, 20);
            this.RepetitionsField.TabIndex = 58;
            this.RepetitionsField.Tag = "";
            this.RepetitionsField.ThousandsSeparator = true;
            this.RepetitionsField.ValueChanged += new System.EventHandler(this.GroupRepeatField_ValueChanged);
            // 
            // RenameGroup
            // 
            this.RenameGroup.Location = new System.Drawing.Point(410, 82);
            this.RenameGroup.Name = "RenameGroup";
            this.RenameGroup.Size = new System.Drawing.Size(60, 20);
            this.RenameGroup.TabIndex = 57;
            this.RenameGroup.Text = "Rename";
            this.RenameGroup.UseVisualStyleBackColor = true;
            this.RenameGroup.Click += new System.EventHandler(this.RenameGroup_Click);
            // 
            // ActivateEvent
            // 
            this.ActivateEvent.Enabled = false;
            this.ActivateEvent.Location = new System.Drawing.Point(259, 209);
            this.ActivateEvent.Name = "ActivateEvent";
            this.ActivateEvent.Size = new System.Drawing.Size(60, 20);
            this.ActivateEvent.TabIndex = 56;
            this.ActivateEvent.Text = "Activate";
            this.ActivateEvent.UseVisualStyleBackColor = true;
            // 
            // EventLabel
            // 
            this.EventLabel.AutoSize = true;
            this.EventLabel.Location = new System.Drawing.Point(194, 41);
            this.EventLabel.Name = "EventLabel";
            this.EventLabel.Size = new System.Drawing.Size(40, 13);
            this.EventLabel.TabIndex = 55;
            this.EventLabel.Text = "Events";
            // 
            // DeleteEvent
            // 
            this.DeleteEvent.Location = new System.Drawing.Point(259, 118);
            this.DeleteEvent.Name = "DeleteEvent";
            this.DeleteEvent.Size = new System.Drawing.Size(60, 20);
            this.DeleteEvent.TabIndex = 53;
            this.DeleteEvent.Text = "Delete";
            this.DeleteEvent.UseVisualStyleBackColor = true;
            this.DeleteEvent.Click += new System.EventHandler(this.DeleteEvent_Click);
            // 
            // ClearEvent
            // 
            this.ClearEvent.Location = new System.Drawing.Point(259, 144);
            this.ClearEvent.Name = "ClearEvent";
            this.ClearEvent.Size = new System.Drawing.Size(60, 20);
            this.ClearEvent.TabIndex = 52;
            this.ClearEvent.Text = "Clear";
            this.ClearEvent.UseVisualStyleBackColor = true;
            // 
            // EventList
            // 
            this.EventList.Cursor = System.Windows.Forms.Cursors.Default;
            this.EventList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EventList.FormatString = "N0";
            this.EventList.FormattingEnabled = true;
            this.EventList.Location = new System.Drawing.Point(180, 56);
            this.EventList.Name = "EventList";
            this.EventList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.EventList.Size = new System.Drawing.Size(73, 173);
            this.EventList.TabIndex = 51;
            this.EventList.SelectedIndexChanged += new System.EventHandler(this.EventList_SelectedIndexChanged);
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HeaderLabel.Location = new System.Drawing.Point(35, 1);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(414, 13);
            this.HeaderLabel.TabIndex = 50;
            this.HeaderLabel.Text = "Activations            Motor #           Rhythm         Magnitude          Cycles" +
                "                Delay";
            // 
            // ActivationList
            // 
            this.ActivationList.FormattingEnabled = true;
            this.ActivationList.Location = new System.Drawing.Point(20, 17);
            this.ActivationList.Name = "ActivationList";
            this.ActivationList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ActivationList.Size = new System.Drawing.Size(88, 212);
            this.ActivationList.TabIndex = 49;
            // 
            // Initialize
            // 
            this.Initialize.Enabled = false;
            this.Initialize.Location = new System.Drawing.Point(20, 235);
            this.Initialize.Name = "Initialize";
            this.Initialize.Size = new System.Drawing.Size(75, 23);
            this.Initialize.TabIndex = 48;
            this.Initialize.Text = "Initialize";
            this.Initialize.UseVisualStyleBackColor = true;
            this.Initialize.Click += new System.EventHandler(this.Initialize_Click);
            // 
            // MagnitudeEditOK
            // 
            this.MagnitudeEditOK.Location = new System.Drawing.Point(510, 149);
            this.MagnitudeEditOK.Name = "MagnitudeEditOK";
            this.MagnitudeEditOK.Size = new System.Drawing.Size(80, 20);
            this.MagnitudeEditOK.TabIndex = 47;
            this.MagnitudeEditOK.Text = "Magnitude";
            this.MagnitudeEditOK.UseVisualStyleBackColor = true;
            this.MagnitudeEditOK.Click += new System.EventHandler(this.MagnitudeEditOK_Click);
            // 
            // RhythmEditOK
            // 
            this.RhythmEditOK.Location = new System.Drawing.Point(510, 123);
            this.RhythmEditOK.Name = "RhythmEditOK";
            this.RhythmEditOK.Size = new System.Drawing.Size(80, 20);
            this.RhythmEditOK.TabIndex = 46;
            this.RhythmEditOK.Text = "Rhythm";
            this.RhythmEditOK.UseVisualStyleBackColor = true;
            this.RhythmEditOK.Click += new System.EventHandler(this.RhythmEditOK_Click);
            // 
            // EditBox
            // 
            this.EditBox.FormattingEnabled = true;
            this.EditBox.Items.AddRange(new object[] {
            "Edit"});
            this.EditBox.Location = new System.Drawing.Point(490, 83);
            this.EditBox.Name = "EditBox";
            this.EditBox.Size = new System.Drawing.Size(100, 21);
            this.EditBox.TabIndex = 44;
            this.EditBox.Text = "Edit";
            // 
            // DelayField
            // 
            this.DelayField.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.DelayField.Location = new System.Drawing.Point(400, 16);
            this.DelayField.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.DelayField.Name = "DelayField";
            this.DelayField.Size = new System.Drawing.Size(73, 20);
            this.DelayField.TabIndex = 40;
            this.DelayField.Tag = "";
            this.DelayField.ThousandsSeparator = true;
            this.DelayField.ValueChanged += new System.EventHandler(this.DirectDelayField_ValueChanged);
            // 
            // ActivateActivation
            // 
            this.ActivateActivation.Enabled = false;
            this.ActivateActivation.Location = new System.Drawing.Point(114, 209);
            this.ActivateActivation.Name = "ActivateActivation";
            this.ActivateActivation.Size = new System.Drawing.Size(60, 20);
            this.ActivateActivation.TabIndex = 37;
            this.ActivateActivation.Text = "Activate";
            this.ActivateActivation.UseVisualStyleBackColor = true;
            // 
            // ActivateGroup
            // 
            this.ActivateGroup.Enabled = false;
            this.ActivateGroup.Location = new System.Drawing.Point(410, 209);
            this.ActivateGroup.Name = "ActivateGroup";
            this.ActivateGroup.Size = new System.Drawing.Size(60, 20);
            this.ActivateGroup.TabIndex = 36;
            this.ActivateGroup.Text = "Activate";
            this.ActivateGroup.UseVisualStyleBackColor = true;
            this.ActivateGroup.Click += new System.EventHandler(this.DirectActivateGroup_Click);
            // 
            // GroupLabel
            // 
            this.GroupLabel.AutoSize = true;
            this.GroupLabel.Location = new System.Drawing.Point(342, 41);
            this.GroupLabel.Name = "GroupLabel";
            this.GroupLabel.Size = new System.Drawing.Size(41, 13);
            this.GroupLabel.TabIndex = 35;
            this.GroupLabel.Text = "Groups";
            // 
            // AddGroup
            // 
            this.AddGroup.Location = new System.Drawing.Point(410, 56);
            this.AddGroup.Name = "AddGroup";
            this.AddGroup.Size = new System.Drawing.Size(60, 20);
            this.AddGroup.TabIndex = 34;
            this.AddGroup.Text = "Add";
            this.AddGroup.UseVisualStyleBackColor = true;
            this.AddGroup.Click += new System.EventHandler(this.DirectAddGroup_Click);
            // 
            // DeleteGroup
            // 
            this.DeleteGroup.Location = new System.Drawing.Point(410, 123);
            this.DeleteGroup.Name = "DeleteGroup";
            this.DeleteGroup.Size = new System.Drawing.Size(60, 20);
            this.DeleteGroup.TabIndex = 33;
            this.DeleteGroup.Text = "Delete";
            this.DeleteGroup.UseVisualStyleBackColor = true;
            this.DeleteGroup.Click += new System.EventHandler(this.DirectDeleteGroup_Click);
            // 
            // ClearGroup
            // 
            this.ClearGroup.Location = new System.Drawing.Point(410, 149);
            this.ClearGroup.Name = "ClearGroup";
            this.ClearGroup.Size = new System.Drawing.Size(60, 20);
            this.ClearGroup.TabIndex = 32;
            this.ClearGroup.Text = "Clear";
            this.ClearGroup.UseVisualStyleBackColor = true;
            this.ClearGroup.Click += new System.EventHandler(this.DirectClearGroups_Click);
            // 
            // GroupList
            // 
            this.GroupList.FormattingEnabled = true;
            this.GroupList.Location = new System.Drawing.Point(325, 82);
            this.GroupList.Name = "GroupList";
            this.GroupList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.GroupList.Size = new System.Drawing.Size(79, 147);
            this.GroupList.TabIndex = 28;
            this.GroupList.SelectedIndexChanged += new System.EventHandler(this.GroupList_SelectedIndexChanged);
            // 
            // RenameField
            // 
            this.RenameField.Location = new System.Drawing.Point(325, 56);
            this.RenameField.MaxLength = 14;
            this.RenameField.Name = "RenameField";
            this.RenameField.Size = new System.Drawing.Size(79, 20);
            this.RenameField.TabIndex = 25;
            // 
            // Stop
            // 
            this.Stop.Enabled = false;
            this.Stop.Location = new System.Drawing.Point(515, 237);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(75, 23);
            this.Stop.TabIndex = 23;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.DirectStop_Click);
            // 
            // ClearActivation
            // 
            this.ClearActivation.Location = new System.Drawing.Point(114, 144);
            this.ClearActivation.Name = "ClearActivation";
            this.ClearActivation.Size = new System.Drawing.Size(60, 20);
            this.ClearActivation.TabIndex = 19;
            this.ClearActivation.Text = "Clear";
            this.ClearActivation.UseVisualStyleBackColor = true;
            this.ClearActivation.Click += new System.EventHandler(this.ClearActivation_Click);
            // 
            // DeleteActivation
            // 
            this.DeleteActivation.Location = new System.Drawing.Point(114, 118);
            this.DeleteActivation.Name = "DeleteActivation";
            this.DeleteActivation.Size = new System.Drawing.Size(60, 20);
            this.DeleteActivation.TabIndex = 14;
            this.DeleteActivation.Text = "Delete";
            this.DeleteActivation.UseVisualStyleBackColor = true;
            this.DeleteActivation.Click += new System.EventHandler(this.DeleteActivation_Click);
            // 
            // SetActivation
            // 
            this.SetActivation.Location = new System.Drawing.Point(114, 56);
            this.SetActivation.Name = "SetActivation";
            this.SetActivation.Size = new System.Drawing.Size(60, 20);
            this.SetActivation.TabIndex = 13;
            this.SetActivation.Text = "Set";
            this.SetActivation.UseVisualStyleBackColor = true;
            this.SetActivation.Click += new System.EventHandler(this.SetActivation_Click);
            // 
            // AddCyclesBox
            // 
            this.AddCyclesBox.FormattingEnabled = true;
            this.AddCyclesBox.Items.AddRange(new object[] {
            "Stop",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "Inf"});
            this.AddCyclesBox.Location = new System.Drawing.Point(331, 16);
            this.AddCyclesBox.Name = "AddCyclesBox";
            this.AddCyclesBox.Size = new System.Drawing.Size(45, 21);
            this.AddCyclesBox.TabIndex = 9;
            this.AddCyclesBox.Text = "Stop";
            // 
            // AddMagBox
            // 
            this.AddMagBox.FormattingEnabled = true;
            this.AddMagBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D"});
            this.AddMagBox.Location = new System.Drawing.Point(262, 16);
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
            this.AddRhythmBox.Location = new System.Drawing.Point(193, 16);
            this.AddRhythmBox.Name = "AddRhythmBox";
            this.AddRhythmBox.Size = new System.Drawing.Size(45, 21);
            this.AddRhythmBox.TabIndex = 7;
            this.AddRhythmBox.Text = "A";
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
            this.MenuStrip.Size = new System.Drawing.Size(641, 24);
            this.MenuStrip.TabIndex = 30;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectMenu,
            this.load,
            this.save});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "File";
            // 
            // connectMenu
            // 
            this.connectMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connect,
            this.disconnect,
            this.toolStripSeparator1,
            this.refreshPorts,
            this.outgoingCOMComboBox,
            this.incomingCOMComboBox});
            this.connectMenu.Name = "connectMenu";
            this.connectMenu.Size = new System.Drawing.Size(140, 22);
            this.connectMenu.Text = "Connect";
            this.connectMenu.ToolTipText = "Display connection options";
            // 
            // connect
            // 
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(181, 22);
            this.connect.Text = "Connect";
            this.connect.ToolTipText = "Attempts to establish connection to the Haptic Belt with the specified Incoming a" +
                "nd Outgoing ports";
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // disconnect
            // 
            this.disconnect.Enabled = false;
            this.disconnect.Name = "disconnect";
            this.disconnect.Size = new System.Drawing.Size(181, 22);
            this.disconnect.Text = "Disconnect";
            this.disconnect.ToolTipText = "Closes the connection with the Haptic Belt ";
            this.disconnect.Click += new System.EventHandler(this.disconnectMenu_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // refreshPorts
            // 
            this.refreshPorts.Name = "refreshPorts";
            this.refreshPorts.Size = new System.Drawing.Size(181, 22);
            this.refreshPorts.Text = "Refresh Ports";
            this.refreshPorts.ToolTipText = "Grabs a new list of ports from the system";
            this.refreshPorts.Click += new System.EventHandler(this.refreshPortsMenu_Click);
            // 
            // outgoingCOMComboBox
            // 
            this.outgoingCOMComboBox.Name = "outgoingCOMComboBox";
            this.outgoingCOMComboBox.Size = new System.Drawing.Size(121, 23);
            this.outgoingCOMComboBox.Text = "Outgoing Port";
            this.outgoingCOMComboBox.ToolTipText = "Port on which outgoing data is sent";
            // 
            // incomingCOMComboBox
            // 
            this.incomingCOMComboBox.Name = "incomingCOMComboBox";
            this.incomingCOMComboBox.Size = new System.Drawing.Size(121, 23);
            this.incomingCOMComboBox.Text = "Incoming Port";
            this.incomingCOMComboBox.ToolTipText = "Port on which incoming data is received";
            // 
            // load
            // 
            this.load.Name = "load";
            this.load.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.load.Size = new System.Drawing.Size(140, 22);
            this.load.Text = "Load";
            this.load.ToolTipText = "Loads selected file into GUI memory";
            this.load.Click += new System.EventHandler(this.loadMenu_Click);
            // 
            // save
            // 
            this.save.Name = "save";
            this.save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.save.Size = new System.Drawing.Size(140, 22);
            this.save.Text = "Save";
            this.save.ToolTipText = "Saves current GUI memory into specified file";
            this.save.Click += new System.EventHandler(this.saveMenu_Click);
            // 
            // optionsMenu
            // 
            this.optionsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOnlyConnectedMotorsMenu,
            this.motorSwapingOnAllGroupsSetsMenu,
            this.realTimeDelayMenu});
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
            this.showOnlyConnectedMotorsMenu.ToolTipText = "If checked, only motors that have connection to the Haptic Belt can be seen";
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
            this.motorSwapingOnAllGroupsSetsMenu.ToolTipText = "If checked, when a swap is performed on a motor it will be performed on all group" +
                "s and sets";
            // 
            // realTimeDelayMenu
            // 
            this.realTimeDelayMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.realTimeDelayValueMenu});
            this.realTimeDelayMenu.Name = "realTimeDelayMenu";
            this.realTimeDelayMenu.Size = new System.Drawing.Size(258, 22);
            this.realTimeDelayMenu.Text = "Realtime Delay";
            this.realTimeDelayMenu.ToolTipText = "Adjusts thread sleeping period while waiting for an event to occur.";
            // 
            // realTimeDelayValueMenu
            // 
            this.realTimeDelayValueMenu.MaxLength = 2;
            this.realTimeDelayValueMenu.Name = "realTimeDelayValueMenu";
            this.realTimeDelayValueMenu.Size = new System.Drawing.Size(100, 23);
            this.realTimeDelayValueMenu.Text = "1";
            this.realTimeDelayValueMenu.ToolTipText = "Recommended; value < 25 \r\nValues displayed in ms";
            this.realTimeDelayValueMenu.TextChanged += new System.EventHandler(this.realTimeDelayValueMenu_TextChanged);
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
            this.ClientSize = new System.Drawing.Size(641, 293);
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
            ((System.ComponentModel.ISupportInitialize)(this.RepetitionsField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayField)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.ComboBox AddCyclesBox;
        private System.Windows.Forms.ComboBox AddMagBox;
        private System.Windows.Forms.ComboBox AddRhythmBox;
        private System.Windows.Forms.Button DeleteActivation;
        private System.Windows.Forms.Button SetActivation;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.TextBox RenameField;
        private System.Windows.Forms.ListBox GroupList;
        private System.Windows.Forms.Label GroupLabel;
        private System.Windows.Forms.Button AddGroup;
        private System.Windows.Forms.Button DeleteGroup;
        private System.Windows.Forms.Button ClearGroup;
        private System.Windows.Forms.Button ActivateActivation;
        private System.Windows.Forms.Button ActivateGroup;
        private System.Windows.Forms.NumericUpDown DelayField;
        private System.Windows.Forms.OpenFileDialog loadBinaryFile;
        private System.Windows.Forms.SaveFileDialog saveBinaryFile;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem connectMenu;
        private System.Windows.Forms.ToolStripMenuItem disconnect;
        private System.Windows.Forms.ToolStripMenuItem load;
        private System.Windows.Forms.ToolStripMenuItem save;
        private System.Windows.Forms.ToolStripComboBox outgoingCOMComboBox;
        private System.Windows.Forms.ToolStripMenuItem refreshPorts;
        private System.Windows.Forms.ToolStripMenuItem versionMenu;
        private System.Windows.Forms.ToolStripMenuItem guiVersionMenu;
        private System.Windows.Forms.ToolStripMenuItem firmwareVersionMenu;
        private System.Windows.Forms.ComboBox EditBox;
        private System.Windows.Forms.Button RhythmEditOK;
        private System.Windows.Forms.Button MagnitudeEditOK;
        private System.Windows.Forms.ToolStripComboBox incomingCOMComboBox;
        private System.Windows.Forms.ToolStripMenuItem connect;
        private System.Windows.Forms.Button Initialize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem optionsMenu;
        private System.Windows.Forms.ToolStripMenuItem showOnlyConnectedMotorsMenu;
        private System.Windows.Forms.ToolStripMenuItem motorSwapingOnAllGroupsSetsMenu;
        private System.Windows.Forms.ToolStripMenuItem realTimeDelayMenu;
        private System.Windows.Forms.ToolStripTextBox realTimeDelayValueMenu;
        private System.Windows.Forms.ListBox ActivationList;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Button ActivateEvent;
        private System.Windows.Forms.Label EventLabel;
        private System.Windows.Forms.Button DeleteEvent;
        private System.Windows.Forms.Button ClearEvent;
        private System.Windows.Forms.ListBox EventList;
        private System.Windows.Forms.Button RenameGroup;
        private System.Windows.Forms.NumericUpDown RepetitionsField;
        private System.Windows.Forms.Label RepetitionsLabel;
        private System.Windows.Forms.ComboBox AddMotorBox;
        private System.Windows.Forms.Button ClearActivation;
        private System.Windows.Forms.Button AddEvent;

    }
}

