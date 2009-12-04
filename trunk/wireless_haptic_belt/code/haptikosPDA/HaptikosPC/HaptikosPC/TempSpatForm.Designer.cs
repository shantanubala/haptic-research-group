namespace Haptikos
{
    /// <summary>
    /// 
    /// </summary>
    partial class TempSpatForm
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
            this.btnTmpSpatStart = new System.Windows.Forms.Button();
            this.btnTmpSpatStop = new System.Windows.Forms.Button();
            this.labelPatternName = new System.Windows.Forms.Label();
            this.labelComment = new System.Windows.Forms.Label();
            this.labelPatternExist = new System.Windows.Forms.Label();
            this.labelWaitTime = new System.Windows.Forms.Label();
            this.textBoxPatternExist = new System.Windows.Forms.TextBox();
            this.textBoxWaitTime = new System.Windows.Forms.TextBox();
            this.textBoxComment = new System.Windows.Forms.TextBox();
            this.comboBoxCycles = new System.Windows.Forms.ComboBox();
            this.labelCyc = new System.Windows.Forms.Label();
            this.comboBoxMag = new System.Windows.Forms.ComboBox();
            this.labelMag = new System.Windows.Forms.Label();
            this.comboBoxRhy = new System.Windows.Forms.ComboBox();
            this.labelRhy = new System.Windows.Forms.Label();
            this.comboBoxMotor = new System.Windows.Forms.ComboBox();
            this.labelMtr = new System.Windows.Forms.Label();
            this.groupBoxCmd = new System.Windows.Forms.GroupBox();
            this.radioBtnPattern = new System.Windows.Forms.RadioButton();
            this.radioBtnComment = new System.Windows.Forms.RadioButton();
            this.radioBtnStopAll = new System.Windows.Forms.RadioButton();
            this.radioBtnStop = new System.Windows.Forms.RadioButton();
            this.radioBtnWait = new System.Windows.Forms.RadioButton();
            this.radioBtnVibrate = new System.Windows.Forms.RadioButton();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnPatternExist = new System.Windows.Forms.Button();
            this.textBoxPatternName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.comboBoxTempo = new System.Windows.Forms.ComboBox();
            this.labelTempo = new System.Windows.Forms.Label();
            this.btnPatternLoad = new System.Windows.Forms.Button();
            this.patternDesign = new System.Windows.Forms.TextBox();
            this.groupBoxCmd.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTmpSpatStart
            // 
            this.btnTmpSpatStart.BackColor = System.Drawing.Color.LightGreen;
            this.btnTmpSpatStart.Location = new System.Drawing.Point(1, 258);
            this.btnTmpSpatStart.Name = "btnTmpSpatStart";
            this.btnTmpSpatStart.Size = new System.Drawing.Size(42, 38);
            this.btnTmpSpatStart.TabIndex = 21;
            this.btnTmpSpatStart.Text = "Start";
            this.btnTmpSpatStart.UseVisualStyleBackColor = false;
            this.btnTmpSpatStart.Click += new System.EventHandler(this.btnTmpSpatStart_Click);
            // 
            // btnTmpSpatStop
            // 
            this.btnTmpSpatStop.BackColor = System.Drawing.Color.LightCoral;
            this.btnTmpSpatStop.Location = new System.Drawing.Point(48, 259);
            this.btnTmpSpatStop.Name = "btnTmpSpatStop";
            this.btnTmpSpatStop.Size = new System.Drawing.Size(45, 38);
            this.btnTmpSpatStop.TabIndex = 22;
            this.btnTmpSpatStop.Text = "Stop";
            this.btnTmpSpatStop.UseVisualStyleBackColor = false;
            this.btnTmpSpatStop.Click += new System.EventHandler(this.btnTmpSpatStop_Click);
            // 
            // labelPatternName
            // 
            this.labelPatternName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelPatternName.Location = new System.Drawing.Point(3, 4);
            this.labelPatternName.Name = "labelPatternName";
            this.labelPatternName.Size = new System.Drawing.Size(124, 20);
            this.labelPatternName.TabIndex = 63;
            this.labelPatternName.Text = "Pattern Name:";
            // 
            // labelComment
            // 
            this.labelComment.Location = new System.Drawing.Point(88, 80);
            this.labelComment.Name = "labelComment";
            this.labelComment.Size = new System.Drawing.Size(54, 13);
            this.labelComment.TabIndex = 62;
            this.labelComment.Text = "Comment:";
            this.labelComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPatternExist
            // 
            this.labelPatternExist.Location = new System.Drawing.Point(88, 159);
            this.labelPatternExist.Name = "labelPatternExist";
            this.labelPatternExist.Size = new System.Drawing.Size(135, 20);
            this.labelPatternExist.TabIndex = 61;
            this.labelPatternExist.Text = "Existing Pattern File:";
            this.labelPatternExist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelWaitTime
            // 
            this.labelWaitTime.Location = new System.Drawing.Point(88, 61);
            this.labelWaitTime.Name = "labelWaitTime";
            this.labelWaitTime.Size = new System.Drawing.Size(90, 18);
            this.labelWaitTime.TabIndex = 60;
            this.labelWaitTime.Text = "Wait Time (ms):";
            this.labelWaitTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxPatternExist
            // 
            this.textBoxPatternExist.Location = new System.Drawing.Point(88, 178);
            this.textBoxPatternExist.Name = "textBoxPatternExist";
            this.textBoxPatternExist.Size = new System.Drawing.Size(115, 20);
            this.textBoxPatternExist.TabIndex = 44;
            // 
            // textBoxWaitTime
            // 
            this.textBoxWaitTime.Location = new System.Drawing.Point(174, 61);
            this.textBoxWaitTime.Name = "textBoxWaitTime";
            this.textBoxWaitTime.Size = new System.Drawing.Size(54, 20);
            this.textBoxWaitTime.TabIndex = 45;
            // 
            // textBoxComment
            // 
            this.textBoxComment.Location = new System.Drawing.Point(88, 96);
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(140, 20);
            this.textBoxComment.TabIndex = 48;
            // 
            // comboBoxCycles
            // 
            this.comboBoxCycles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCycles.Location = new System.Drawing.Point(184, 36);
            this.comboBoxCycles.Name = "comboBoxCycles";
            this.comboBoxCycles.Size = new System.Drawing.Size(44, 21);
            this.comboBoxCycles.TabIndex = 67;
            // 
            // labelCyc
            // 
            this.labelCyc.Location = new System.Drawing.Point(184, 20);
            this.labelCyc.Name = "labelCyc";
            this.labelCyc.Size = new System.Drawing.Size(44, 20);
            this.labelCyc.TabIndex = 68;
            this.labelCyc.Text = "Cycles";
            this.labelCyc.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxMag
            // 
            this.comboBoxMag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMag.Location = new System.Drawing.Point(122, 36);
            this.comboBoxMag.Name = "comboBoxMag";
            this.comboBoxMag.Size = new System.Drawing.Size(56, 21);
            this.comboBoxMag.TabIndex = 66;
            // 
            // labelMag
            // 
            this.labelMag.Location = new System.Drawing.Point(116, 20);
            this.labelMag.Name = "labelMag";
            this.labelMag.Size = new System.Drawing.Size(68, 20);
            this.labelMag.TabIndex = 69;
            this.labelMag.Text = "Magnitude";
            this.labelMag.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxRhy
            // 
            this.comboBoxRhy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRhy.Location = new System.Drawing.Point(63, 36);
            this.comboBoxRhy.Name = "comboBoxRhy";
            this.comboBoxRhy.Size = new System.Drawing.Size(53, 21);
            this.comboBoxRhy.TabIndex = 65;
            // 
            // labelRhy
            // 
            this.labelRhy.Location = new System.Drawing.Point(63, 20);
            this.labelRhy.Name = "labelRhy";
            this.labelRhy.Size = new System.Drawing.Size(53, 20);
            this.labelRhy.TabIndex = 70;
            this.labelRhy.Text = "Rhythm";
            this.labelRhy.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxMotor
            // 
            this.comboBoxMotor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMotor.Location = new System.Drawing.Point(13, 36);
            this.comboBoxMotor.Name = "comboBoxMotor";
            this.comboBoxMotor.Size = new System.Drawing.Size(44, 21);
            this.comboBoxMotor.TabIndex = 64;
            // 
            // labelMtr
            // 
            this.labelMtr.Location = new System.Drawing.Point(13, 20);
            this.labelMtr.Name = "labelMtr";
            this.labelMtr.Size = new System.Drawing.Size(44, 20);
            this.labelMtr.TabIndex = 71;
            this.labelMtr.Text = "Motor";
            this.labelMtr.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // groupBoxCmd
            // 
            this.groupBoxCmd.Controls.Add(this.radioBtnPattern);
            this.groupBoxCmd.Controls.Add(this.radioBtnComment);
            this.groupBoxCmd.Controls.Add(this.radioBtnStopAll);
            this.groupBoxCmd.Controls.Add(this.radioBtnStop);
            this.groupBoxCmd.Controls.Add(this.radioBtnWait);
            this.groupBoxCmd.Controls.Add(this.radioBtnVibrate);
            this.groupBoxCmd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCmd.Location = new System.Drawing.Point(6, 64);
            this.groupBoxCmd.Name = "groupBoxCmd";
            this.groupBoxCmd.Size = new System.Drawing.Size(76, 134);
            this.groupBoxCmd.TabIndex = 72;
            this.groupBoxCmd.TabStop = false;
            this.groupBoxCmd.Text = "Command";
            // 
            // radioBtnPattern
            // 
            this.radioBtnPattern.AutoSize = true;
            this.radioBtnPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnPattern.Location = new System.Drawing.Point(3, 100);
            this.radioBtnPattern.Name = "radioBtnPattern";
            this.radioBtnPattern.Size = new System.Drawing.Size(61, 30);
            this.radioBtnPattern.TabIndex = 5;
            this.radioBtnPattern.TabStop = true;
            this.radioBtnPattern.Text = "Existing\r\nPattern";
            this.radioBtnPattern.UseVisualStyleBackColor = true;
            // 
            // radioBtnComment
            // 
            this.radioBtnComment.AutoSize = true;
            this.radioBtnComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnComment.Location = new System.Drawing.Point(3, 84);
            this.radioBtnComment.Name = "radioBtnComment";
            this.radioBtnComment.Size = new System.Drawing.Size(69, 17);
            this.radioBtnComment.TabIndex = 4;
            this.radioBtnComment.TabStop = true;
            this.radioBtnComment.Text = "Comment";
            this.radioBtnComment.UseVisualStyleBackColor = true;
            // 
            // radioBtnStopAll
            // 
            this.radioBtnStopAll.AutoSize = true;
            this.radioBtnStopAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnStopAll.Location = new System.Drawing.Point(3, 68);
            this.radioBtnStopAll.Name = "radioBtnStopAll";
            this.radioBtnStopAll.Size = new System.Drawing.Size(61, 17);
            this.radioBtnStopAll.TabIndex = 3;
            this.radioBtnStopAll.TabStop = true;
            this.radioBtnStopAll.Text = "Stop All";
            this.radioBtnStopAll.UseVisualStyleBackColor = true;
            // 
            // radioBtnStop
            // 
            this.radioBtnStop.AutoSize = true;
            this.radioBtnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnStop.Location = new System.Drawing.Point(3, 51);
            this.radioBtnStop.Name = "radioBtnStop";
            this.radioBtnStop.Size = new System.Drawing.Size(47, 17);
            this.radioBtnStop.TabIndex = 2;
            this.radioBtnStop.TabStop = true;
            this.radioBtnStop.Text = "Stop";
            this.radioBtnStop.UseVisualStyleBackColor = true;
            // 
            // radioBtnWait
            // 
            this.radioBtnWait.AutoSize = true;
            this.radioBtnWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnWait.Location = new System.Drawing.Point(3, 35);
            this.radioBtnWait.Name = "radioBtnWait";
            this.radioBtnWait.Size = new System.Drawing.Size(47, 17);
            this.radioBtnWait.TabIndex = 1;
            this.radioBtnWait.TabStop = true;
            this.radioBtnWait.Text = "Wait";
            this.radioBtnWait.UseVisualStyleBackColor = true;
            // 
            // radioBtnVibrate
            // 
            this.radioBtnVibrate.AutoSize = true;
            this.radioBtnVibrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnVibrate.Location = new System.Drawing.Point(3, 19);
            this.radioBtnVibrate.Name = "radioBtnVibrate";
            this.radioBtnVibrate.Size = new System.Drawing.Size(58, 17);
            this.radioBtnVibrate.TabIndex = 0;
            this.radioBtnVibrate.TabStop = true;
            this.radioBtnVibrate.Text = "Vibrate";
            this.radioBtnVibrate.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.LightGreen;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAdd.Location = new System.Drawing.Point(101, 122);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(58, 38);
            this.btnAdd.TabIndex = 73;
            this.btnAdd.Text = "Add to \r\nPattern";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnPatternExist
            // 
            this.btnPatternExist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPatternExist.Location = new System.Drawing.Point(209, 177);
            this.btnPatternExist.Name = "btnPatternExist";
            this.btnPatternExist.Size = new System.Drawing.Size(27, 23);
            this.btnPatternExist.TabIndex = 74;
            this.btnPatternExist.Text = "...";
            this.btnPatternExist.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPatternExist.UseVisualStyleBackColor = true;
            this.btnPatternExist.Click += new System.EventHandler(this.btnPatternExist_Click);
            // 
            // textBoxPatternName
            // 
            this.textBoxPatternName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPatternName.Location = new System.Drawing.Point(101, 2);
            this.textBoxPatternName.Name = "textBoxPatternName";
            this.textBoxPatternName.Size = new System.Drawing.Size(127, 20);
            this.textBoxPatternName.TabIndex = 75;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightBlue;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Location = new System.Drawing.Point(165, 122);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(58, 38);
            this.btnSave.TabIndex = 76;
            this.btnSave.Text = "Save \r\nPattern";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // comboBoxTempo
            // 
            this.comboBoxTempo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTempo.Location = new System.Drawing.Point(49, 232);
            this.comboBoxTempo.Name = "comboBoxTempo";
            this.comboBoxTempo.Size = new System.Drawing.Size(44, 21);
            this.comboBoxTempo.TabIndex = 77;
            this.comboBoxTempo.SelectedIndexChanged += new System.EventHandler(this.comboBoxTempo_SelectedIndexChanged);
            // 
            // labelTempo
            // 
            this.labelTempo.Location = new System.Drawing.Point(49, 216);
            this.labelTempo.Name = "labelTempo";
            this.labelTempo.Size = new System.Drawing.Size(44, 20);
            this.labelTempo.TabIndex = 78;
            this.labelTempo.Text = "Tempo";
            this.labelTempo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnPatternLoad
            // 
            this.btnPatternLoad.BackColor = System.Drawing.Color.LightBlue;
            this.btnPatternLoad.Location = new System.Drawing.Point(1, 215);
            this.btnPatternLoad.Name = "btnPatternLoad";
            this.btnPatternLoad.Size = new System.Drawing.Size(42, 38);
            this.btnPatternLoad.TabIndex = 79;
            this.btnPatternLoad.Text = "Load";
            this.btnPatternLoad.UseVisualStyleBackColor = false;
            this.btnPatternLoad.Click += new System.EventHandler(this.btnPatternLoad_Click);
            // 
            // patternDesign
            // 
            this.patternDesign.Location = new System.Drawing.Point(99, 206);
            this.patternDesign.Multiline = true;
            this.patternDesign.Name = "patternDesign";
            this.patternDesign.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.patternDesign.Size = new System.Drawing.Size(137, 90);
            this.patternDesign.TabIndex = 80;
            this.patternDesign.WordWrap = false;
            // 
            // TempSpatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 299);
            this.Controls.Add(this.patternDesign);
            this.Controls.Add(this.btnPatternLoad);
            this.Controls.Add(this.comboBoxTempo);
            this.Controls.Add(this.labelTempo);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.textBoxPatternName);
            this.Controls.Add(this.btnPatternExist);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBoxCmd);
            this.Controls.Add(this.comboBoxCycles);
            this.Controls.Add(this.labelCyc);
            this.Controls.Add(this.comboBoxMag);
            this.Controls.Add(this.labelMag);
            this.Controls.Add(this.comboBoxRhy);
            this.Controls.Add(this.labelRhy);
            this.Controls.Add(this.comboBoxMotor);
            this.Controls.Add(this.labelMtr);
            this.Controls.Add(this.textBoxComment);
            this.Controls.Add(this.textBoxWaitTime);
            this.Controls.Add(this.textBoxPatternExist);
            this.Controls.Add(this.labelWaitTime);
            this.Controls.Add(this.labelPatternExist);
            this.Controls.Add(this.labelComment);
            this.Controls.Add(this.labelPatternName);
            this.Controls.Add(this.btnTmpSpatStop);
            this.Controls.Add(this.btnTmpSpatStart);
            this.Name = "TempSpatForm";
            this.Text = "Temporal-Spatial";
            this.groupBoxCmd.ResumeLayout(false);
            this.groupBoxCmd.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxComment;
        private System.Windows.Forms.TextBox textBoxWaitTime;
        private System.Windows.Forms.TextBox textBoxPatternExist;
        private System.Windows.Forms.Label labelWaitTime;
        private System.Windows.Forms.Label labelPatternExist;
        private System.Windows.Forms.Label labelComment;
        private System.Windows.Forms.Label labelPatternName;
        internal System.Windows.Forms.Button btnTmpSpatStop;
        internal System.Windows.Forms.Button btnTmpSpatStart;
        private System.Windows.Forms.ComboBox comboBoxCycles;
        private System.Windows.Forms.Label labelCyc;
        private System.Windows.Forms.ComboBox comboBoxMag;
        private System.Windows.Forms.Label labelMag;
        private System.Windows.Forms.ComboBox comboBoxRhy;
        private System.Windows.Forms.Label labelRhy;
        private System.Windows.Forms.ComboBox comboBoxMotor;
        private System.Windows.Forms.Label labelMtr;
        private System.Windows.Forms.GroupBox groupBoxCmd;
        private System.Windows.Forms.RadioButton radioBtnStop;
        private System.Windows.Forms.RadioButton radioBtnWait;
        private System.Windows.Forms.RadioButton radioBtnVibrate;
        private System.Windows.Forms.RadioButton radioBtnPattern;
        private System.Windows.Forms.RadioButton radioBtnComment;
        private System.Windows.Forms.RadioButton radioBtnStopAll;
        internal System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnPatternExist;
        private System.Windows.Forms.TextBox textBoxPatternName;
        internal System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox comboBoxTempo;
        private System.Windows.Forms.Label labelTempo;
        internal System.Windows.Forms.Button btnPatternLoad;
        private System.Windows.Forms.TextBox patternDesign;
    }
}