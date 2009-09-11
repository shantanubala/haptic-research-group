namespace Haptikos
{
    partial class DemoForm
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
            this.btnDone = new System.Windows.Forms.Button();
            this.checkBoxScan = new System.Windows.Forms.CheckBox();
            this.checkBoxSweep = new System.Windows.Forms.CheckBox();
            this.checkBoxHeartbeats = new System.Windows.Forms.CheckBox();
            this.labelDemoCfg = new System.Windows.Forms.Label();
            this.labelDemoList = new System.Windows.Forms.Label();
            this.comboBoxCycles3 = new System.Windows.Forms.ComboBox();
            this.labelCyc3 = new System.Windows.Forms.Label();
            this.comboBoxMag3 = new System.Windows.Forms.ComboBox();
            this.labelMag3 = new System.Windows.Forms.Label();
            this.comboBoxRhy3 = new System.Windows.Forms.ComboBox();
            this.labelRhy3 = new System.Windows.Forms.Label();
            this.btnStopAll2 = new System.Windows.Forms.Button();
            this.btnActivateDemo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDone
            // 
            this.btnDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDone.Location = new System.Drawing.Point(181, 202);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(56, 38);
            this.btnDone.TabIndex = 6;
            this.btnDone.Text = "Done";
            // 
            // checkBoxScan
            // 
            this.checkBoxScan.Checked = true;
            this.checkBoxScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxScan.Location = new System.Drawing.Point(33, 25);
            this.checkBoxScan.Name = "checkBoxScan";
            this.checkBoxScan.Size = new System.Drawing.Size(177, 18);
            this.checkBoxScan.TabIndex = 63;
            this.checkBoxScan.Text = "Scanning";
            this.checkBoxScan.CheckStateChanged += new System.EventHandler(this.checkBoxDemo_CheckStateChanged);
            // 
            // checkBoxSweep
            // 
            this.checkBoxSweep.Location = new System.Drawing.Point(33, 49);
            this.checkBoxSweep.Name = "checkBoxSweep";
            this.checkBoxSweep.Size = new System.Drawing.Size(177, 18);
            this.checkBoxSweep.TabIndex = 84;
            this.checkBoxSweep.Text = "Sweeping";
            this.checkBoxSweep.CheckStateChanged += new System.EventHandler(this.checkBoxDemo_CheckStateChanged);
            // 
            // checkBoxHeartbeats
            // 
            this.checkBoxHeartbeats.Location = new System.Drawing.Point(33, 73);
            this.checkBoxHeartbeats.Name = "checkBoxHeartbeats";
            this.checkBoxHeartbeats.Size = new System.Drawing.Size(177, 18);
            this.checkBoxHeartbeats.TabIndex = 85;
            this.checkBoxHeartbeats.Text = "Heartbeats";
            this.checkBoxHeartbeats.CheckStateChanged += new System.EventHandler(this.checkBoxDemo_CheckStateChanged);
            // 
            // labelDemoCfg
            // 
            this.labelDemoCfg.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelDemoCfg.Location = new System.Drawing.Point(4, 163);
            this.labelDemoCfg.Name = "labelDemoCfg";
            this.labelDemoCfg.Size = new System.Drawing.Size(49, 33);
            this.labelDemoCfg.Text = "Tactor Config:";
            // 
            // labelDemoList
            // 
            this.labelDemoList.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelDemoList.Location = new System.Drawing.Point(4, 2);
            this.labelDemoList.Name = "labelDemoList";
            this.labelDemoList.Size = new System.Drawing.Size(174, 20);
            this.labelDemoList.Text = "Temporal-Spatial Demos:";
            // 
            // comboBoxCycles3
            // 
            this.comboBoxCycles3.Location = new System.Drawing.Point(184, 174);
            this.comboBoxCycles3.Name = "comboBoxCycles3";
            this.comboBoxCycles3.Size = new System.Drawing.Size(44, 22);
            this.comboBoxCycles3.TabIndex = 62;
            // 
            // labelCyc3
            // 
            this.labelCyc3.Location = new System.Drawing.Point(184, 158);
            this.labelCyc3.Name = "labelCyc3";
            this.labelCyc3.Size = new System.Drawing.Size(44, 20);
            this.labelCyc3.Text = "Cycles";
            this.labelCyc3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxMag3
            // 
            this.comboBoxMag3.Location = new System.Drawing.Point(122, 174);
            this.comboBoxMag3.Name = "comboBoxMag3";
            this.comboBoxMag3.Size = new System.Drawing.Size(56, 22);
            this.comboBoxMag3.TabIndex = 61;
            // 
            // labelMag3
            // 
            this.labelMag3.Location = new System.Drawing.Point(116, 158);
            this.labelMag3.Name = "labelMag3";
            this.labelMag3.Size = new System.Drawing.Size(68, 20);
            this.labelMag3.Text = "Magnitude";
            this.labelMag3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxRhy3
            // 
            this.comboBoxRhy3.Location = new System.Drawing.Point(63, 174);
            this.comboBoxRhy3.Name = "comboBoxRhy3";
            this.comboBoxRhy3.Size = new System.Drawing.Size(53, 22);
            this.comboBoxRhy3.TabIndex = 60;
            // 
            // labelRhy3
            // 
            this.labelRhy3.Location = new System.Drawing.Point(63, 158);
            this.labelRhy3.Name = "labelRhy3";
            this.labelRhy3.Size = new System.Drawing.Size(53, 20);
            this.labelRhy3.Text = "Rhythm";
            this.labelRhy3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnStopAll2
            // 
            this.btnStopAll2.Location = new System.Drawing.Point(116, 202);
            this.btnStopAll2.Name = "btnStopAll2";
            this.btnStopAll2.Size = new System.Drawing.Size(59, 38);
            this.btnStopAll2.TabIndex = 59;
            this.btnStopAll2.Text = "Stop All";
            // 
            // btnActivateDemo
            // 
            this.btnActivateDemo.Location = new System.Drawing.Point(4, 202);
            this.btnActivateDemo.Name = "btnActivateDemo";
            this.btnActivateDemo.Size = new System.Drawing.Size(106, 38);
            this.btnActivateDemo.TabIndex = 58;
            this.btnActivateDemo.Text = "Activate Demo";
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 245);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.checkBoxScan);
            this.Controls.Add(this.checkBoxSweep);
            this.Controls.Add(this.checkBoxHeartbeats);
            this.Controls.Add(this.labelDemoCfg);
            this.Controls.Add(this.labelDemoList);
            this.Controls.Add(this.comboBoxCycles3);
            this.Controls.Add(this.labelCyc3);
            this.Controls.Add(this.comboBoxMag3);
            this.Controls.Add(this.labelMag3);
            this.Controls.Add(this.comboBoxRhy3);
            this.Controls.Add(this.labelRhy3);
            this.Controls.Add(this.btnStopAll2);
            this.Controls.Add(this.btnActivateDemo);
            this.Menu = this.mainMenu1;
            this.Name = "DemoForm";
            this.Text = "Demo Options";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxHeartbeats;
        private System.Windows.Forms.CheckBox checkBoxSweep;
        private System.Windows.Forms.Label labelDemoCfg;
        private System.Windows.Forms.CheckBox checkBoxScan;
        private System.Windows.Forms.Label labelDemoList;
        private System.Windows.Forms.ComboBox comboBoxCycles3;
        private System.Windows.Forms.Label labelCyc3;
        private System.Windows.Forms.ComboBox comboBoxMag3;
        private System.Windows.Forms.Label labelMag3;
        private System.Windows.Forms.ComboBox comboBoxRhy3;
        private System.Windows.Forms.Label labelRhy3;
        internal System.Windows.Forms.Button btnStopAll2;
        internal System.Windows.Forms.Button btnActivateDemo;
        private System.Windows.Forms.Button btnDone;
    }
}