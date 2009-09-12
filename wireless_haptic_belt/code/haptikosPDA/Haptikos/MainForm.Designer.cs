namespace Haptikos
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.mnuClose = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuSettings = new System.Windows.Forms.MenuItem();
            this.mnuConnect = new System.Windows.Forms.MenuItem();
            this.mnuDisconnect = new System.Windows.Forms.MenuItem();
            this.mnuDemo = new System.Windows.Forms.MenuItem();
            this.txtMess = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelStatusMsg = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.comboBoxCycles = new System.Windows.Forms.ComboBox();
            this.labelCyc = new System.Windows.Forms.Label();
            this.comboBoxMag = new System.Windows.Forms.ComboBox();
            this.labelMag = new System.Windows.Forms.Label();
            this.comboBoxRhy = new System.Windows.Forms.ComboBox();
            this.labelRhy = new System.Windows.Forms.Label();
            this.comboBoxMotor = new System.Windows.Forms.ComboBox();
            this.labelMtr = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnActivate = new System.Windows.Forms.Button();
            this.mnuTempSpat = new System.Windows.Forms.MenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.mnuClose);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // mnuClose
            // 
            this.mnuClose.Text = "Close";
            this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.MenuItems.Add(this.mnuSettings);
            this.menuItem2.MenuItems.Add(this.mnuConnect);
            this.menuItem2.MenuItems.Add(this.mnuDisconnect);
            this.menuItem2.MenuItems.Add(this.mnuDemo);
            this.menuItem2.MenuItems.Add(this.mnuTempSpat);
            this.menuItem2.Text = "Menu";
            // 
            // mnuSettings
            // 
            this.mnuSettings.Text = "Settings";
            this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
            // 
            // mnuConnect
            // 
            this.mnuConnect.Enabled = false;
            this.mnuConnect.Text = "Connect";
            this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // mnuDisconnect
            // 
            this.mnuDisconnect.Enabled = false;
            this.mnuDisconnect.Text = "Disconnect";
            this.mnuDisconnect.Click += new System.EventHandler(this.mnuDisconnect_Click);
            // 
            // mnuDemo
            // 
            this.mnuDemo.Enabled = false;
            this.mnuDemo.Text = "Demo";
            this.mnuDemo.Click += new System.EventHandler(this.mnuDemo_Click);
            // 
            // txtMess
            // 
            this.txtMess.Enabled = false;
            this.txtMess.Location = new System.Drawing.Point(3, 3);
            this.txtMess.Multiline = true;
            this.txtMess.Name = "txtMess";
            this.txtMess.Size = new System.Drawing.Size(159, 26);
            this.txtMess.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(165, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(72, 26);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtLog
            // 
            this.txtLog.Enabled = false;
            this.txtLog.Location = new System.Drawing.Point(3, 35);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(234, 91);
            this.txtLog.TabIndex = 3;
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(3, 214);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(45, 20);
            this.labelStatus.Text = "Status:";
            // 
            // labelStatusMsg
            // 
            this.labelStatusMsg.Location = new System.Drawing.Point(47, 214);
            this.labelStatusMsg.Name = "labelStatusMsg";
            this.labelStatusMsg.Size = new System.Drawing.Size(190, 31);
            this.labelStatusMsg.Text = "status label";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageMain);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(240, 268);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.comboBoxCycles);
            this.tabPageMain.Controls.Add(this.labelCyc);
            this.tabPageMain.Controls.Add(this.comboBoxMag);
            this.tabPageMain.Controls.Add(this.labelMag);
            this.tabPageMain.Controls.Add(this.comboBoxRhy);
            this.tabPageMain.Controls.Add(this.labelRhy);
            this.tabPageMain.Controls.Add(this.comboBoxMotor);
            this.tabPageMain.Controls.Add(this.labelMtr);
            this.tabPageMain.Controls.Add(this.btnQuery);
            this.tabPageMain.Controls.Add(this.btnStop);
            this.tabPageMain.Controls.Add(this.btnActivate);
            this.tabPageMain.Controls.Add(this.labelStatusMsg);
            this.tabPageMain.Controls.Add(this.btnSend);
            this.tabPageMain.Controls.Add(this.txtLog);
            this.tabPageMain.Controls.Add(this.txtMess);
            this.tabPageMain.Controls.Add(this.labelStatus);
            this.tabPageMain.Location = new System.Drawing.Point(0, 0);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Size = new System.Drawing.Size(240, 245);
            this.tabPageMain.Text = "Haptics Main";
            // 
            // comboBoxCycles
            // 
            this.comboBoxCycles.Location = new System.Drawing.Point(183, 145);
            this.comboBoxCycles.Name = "comboBoxCycles";
            this.comboBoxCycles.Size = new System.Drawing.Size(44, 22);
            this.comboBoxCycles.TabIndex = 16;
            // 
            // labelCyc
            // 
            this.labelCyc.Location = new System.Drawing.Point(183, 129);
            this.labelCyc.Name = "labelCyc";
            this.labelCyc.Size = new System.Drawing.Size(44, 20);
            this.labelCyc.Text = "Cycles";
            this.labelCyc.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxMag
            // 
            this.comboBoxMag.Location = new System.Drawing.Point(121, 145);
            this.comboBoxMag.Name = "comboBoxMag";
            this.comboBoxMag.Size = new System.Drawing.Size(56, 22);
            this.comboBoxMag.TabIndex = 13;
            // 
            // labelMag
            // 
            this.labelMag.Location = new System.Drawing.Point(115, 129);
            this.labelMag.Name = "labelMag";
            this.labelMag.Size = new System.Drawing.Size(68, 20);
            this.labelMag.Text = "Magnitude";
            this.labelMag.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxRhy
            // 
            this.comboBoxRhy.Location = new System.Drawing.Point(62, 145);
            this.comboBoxRhy.Name = "comboBoxRhy";
            this.comboBoxRhy.Size = new System.Drawing.Size(53, 22);
            this.comboBoxRhy.TabIndex = 10;
            // 
            // labelRhy
            // 
            this.labelRhy.Location = new System.Drawing.Point(62, 129);
            this.labelRhy.Name = "labelRhy";
            this.labelRhy.Size = new System.Drawing.Size(53, 20);
            this.labelRhy.Text = "Rhythm";
            this.labelRhy.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxMotor
            // 
            this.comboBoxMotor.Location = new System.Drawing.Point(12, 145);
            this.comboBoxMotor.Name = "comboBoxMotor";
            this.comboBoxMotor.Size = new System.Drawing.Size(44, 22);
            this.comboBoxMotor.TabIndex = 7;
            // 
            // labelMtr
            // 
            this.labelMtr.Location = new System.Drawing.Point(12, 129);
            this.labelMtr.Name = "labelMtr";
            this.labelMtr.Size = new System.Drawing.Size(44, 20);
            this.labelMtr.Text = "Motor";
            this.labelMtr.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnQuery
            // 
            this.btnQuery.Enabled = false;
            this.btnQuery.Location = new System.Drawing.Point(138, 173);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(99, 38);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query Config";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(78, 173);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(54, 38);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnActivate
            // 
            this.btnActivate.Enabled = false;
            this.btnActivate.Location = new System.Drawing.Point(3, 173);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(69, 38);
            this.btnActivate.TabIndex = 4;
            this.btnActivate.Text = "Activate";
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // mnuTempSpat
            // 
            this.mnuTempSpat.Enabled = false;
            this.mnuTempSpat.Text = "Temp-Spatial";
            this.mnuTempSpat.Click += new System.EventHandler(this.mnuTempSpat_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 285);
            this.Controls.Add(this.tabControl1);
            //this.Controls.Add(this.comboBoxCycles);
            //this.Controls.Add(this.labelCyc);
            //this.Controls.Add(this.comboBoxMag);
            //this.Controls.Add(this.labelMag);
            //this.Controls.Add(this.comboBoxRhy);
            //this.Controls.Add(this.labelRhy);
            //this.Controls.Add(this.comboBoxMotor);
            //this.Controls.Add(this.labelMtr);
            //this.Controls.Add(this.btnQuery);
            //this.Controls.Add(this.btnStop);
            //this.Controls.Add(this.btnActivate);
            //this.Controls.Add(this.labelStatusMsg);
            //this.Controls.Add(this.btnSend);
            //this.Controls.Add(this.txtLog);
            //this.Controls.Add(this.txtMess);
            //this.Controls.Add(this.labelStatus);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.Text = "Haptikos";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.tabControl1.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtMess;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelStatusMsg;
        private System.Windows.Forms.MenuItem mnuClose;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem mnuSettings;
        private System.Windows.Forms.MenuItem mnuConnect;
        private System.Windows.Forms.MenuItem mnuDisconnect;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.ComboBox comboBoxCycles;
        private System.Windows.Forms.Label labelCyc;
        private System.Windows.Forms.ComboBox comboBoxMag;
        private System.Windows.Forms.Label labelMag;
        private System.Windows.Forms.ComboBox comboBoxRhy;
        private System.Windows.Forms.Label labelRhy;
        private System.Windows.Forms.ComboBox comboBoxMotor;
        private System.Windows.Forms.Label labelMtr;
        private System.Windows.Forms.MenuItem mnuDemo;
        private System.Windows.Forms.MenuItem mnuTempSpat;
        //private System.Windows.Forms.CheckBox checkBoxHeartbeats;
        //private System.Windows.Forms.CheckBox checkBoxSweep;
        //private System.Windows.Forms.Label labelDemoCfg;
        //private System.Windows.Forms.CheckBox checkBoxScan;
        //private System.Windows.Forms.Label labelDemoList;
        //private System.Windows.Forms.ComboBox comboBoxCycles3;
        //private System.Windows.Forms.Label labelCyc3;
        //private System.Windows.Forms.ComboBox comboBoxMag3;
        //private System.Windows.Forms.Label labelMag3;
        //private System.Windows.Forms.ComboBox comboBoxRhy3;
        //private System.Windows.Forms.Label labelRhy3;
        //private System.Windows.Forms.Button btnStopAll2;
        //private System.Windows.Forms.Button btnActivateDemo;

    }
}
