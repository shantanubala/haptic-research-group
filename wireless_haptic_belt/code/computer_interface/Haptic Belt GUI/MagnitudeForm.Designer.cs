namespace HapticGUI
{
    partial class MagnitudeForm
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
            this.MagPanel = new System.Windows.Forms.Panel();
            this.MagDone = new System.Windows.Forms.Button();
            this.MagSet = new System.Windows.Forms.Button();
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
            this.MagComboBox = new System.Windows.Forms.ComboBox();
            this.MagSelLabel = new System.Windows.Forms.Label();
            this.MagPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DutyCycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Period)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Percentage)).BeginInit();
            this.SuspendLayout();
            // 
            // MagPanel
            // 
            this.MagPanel.Controls.Add(this.MagDone);
            this.MagPanel.Controls.Add(this.MagSet);
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
            this.MagPanel.Controls.Add(this.MagComboBox);
            this.MagPanel.Controls.Add(this.MagSelLabel);
            this.MagPanel.Location = new System.Drawing.Point(7, 7);
            this.MagPanel.Name = "MagPanel";
            this.MagPanel.Size = new System.Drawing.Size(270, 248);
            this.MagPanel.TabIndex = 27;
            // 
            // MagDone
            // 
            this.MagDone.Location = new System.Drawing.Point(16, 222);
            this.MagDone.Name = "MagDone";
            this.MagDone.Size = new System.Drawing.Size(75, 23);
            this.MagDone.TabIndex = 39;
            this.MagDone.Text = "Done";
            this.MagDone.UseVisualStyleBackColor = true;
            this.MagDone.Click += new System.EventHandler(this.MagDone_Click);
            // 
            // MagSet
            // 
            this.MagSet.Location = new System.Drawing.Point(180, 222);
            this.MagSet.Name = "MagSet";
            this.MagSet.Size = new System.Drawing.Size(75, 23);
            this.MagSet.TabIndex = 38;
            this.MagSet.Text = "Set";
            this.MagSet.UseVisualStyleBackColor = true;
            // 
            // MagTestStop
            // 
            this.MagTestStop.Enabled = false;
            this.MagTestStop.Location = new System.Drawing.Point(98, 222);
            this.MagTestStop.Name = "MagTestStop";
            this.MagTestStop.Size = new System.Drawing.Size(75, 23);
            this.MagTestStop.TabIndex = 37;
            this.MagTestStop.Text = "Stop";
            this.MagTestStop.UseVisualStyleBackColor = true;
            this.MagTestStop.Visible = false;
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
            this.DutyCycle.Location = new System.Drawing.Point(107, 125);
            this.DutyCycle.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.DutyCycle.Name = "DutyCycle";
            this.DutyCycle.Size = new System.Drawing.Size(66, 20);
            this.DutyCycle.TabIndex = 13;
            this.DutyCycle.Visible = false;
            // 
            // Period
            // 
            this.Period.Location = new System.Drawing.Point(107, 99);
            this.Period.Maximum = new decimal(new int[] {
            50000,
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
            // 
            // PeriodLabel
            // 
            this.PeriodLabel.AutoSize = true;
            this.PeriodLabel.Location = new System.Drawing.Point(49, 101);
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
            this.MagOption.Location = new System.Drawing.Point(59, 163);
            this.MagOption.Name = "MagOption";
            this.MagOption.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MagOption.Size = new System.Drawing.Size(114, 17);
            this.MagOption.TabIndex = 7;
            this.MagOption.Text = "Advanced Options";
            this.MagOption.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MagOption.UseVisualStyleBackColor = true;
            this.MagOption.CheckedChanged += new System.EventHandler(this.MagOption_CheckedChanged);
            // 
            // Percentage
            // 
            this.Percentage.Location = new System.Drawing.Point(108, 71);
            this.Percentage.Name = "Percentage";
            this.Percentage.Size = new System.Drawing.Size(65, 20);
            this.Percentage.TabIndex = 6;
            // 
            // PercentLabel
            // 
            this.PercentLabel.AutoSize = true;
            this.PercentLabel.Location = new System.Drawing.Point(32, 74);
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
            // 
            // MagSelLabel
            // 
            this.MagSelLabel.AutoSize = true;
            this.MagSelLabel.BackColor = System.Drawing.SystemColors.Control;
            this.MagSelLabel.Location = new System.Drawing.Point(13, 19);
            this.MagSelLabel.Name = "MagSelLabel";
            this.MagSelLabel.Size = new System.Drawing.Size(90, 13);
            this.MagSelLabel.TabIndex = 1;
            this.MagSelLabel.Text = "Select Magnitude";
            // 
            // MagnitudeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.MagPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MagnitudeForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Magnitude Mode";
            this.MagPanel.ResumeLayout(false);
            this.MagPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DutyCycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Period)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Percentage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MagPanel;
        private System.Windows.Forms.Button MagDone;
        private System.Windows.Forms.Button MagSet;
        private System.Windows.Forms.Button MagTestStop;
        private System.Windows.Forms.Label DutyLabel;
        private System.Windows.Forms.NumericUpDown DutyCycle;
        private System.Windows.Forms.NumericUpDown Period;
        private System.Windows.Forms.Label PeriodLabel;
        private System.Windows.Forms.Label PeriodDefaultLabel;
        private System.Windows.Forms.CheckBox MagOption;
        private System.Windows.Forms.NumericUpDown Percentage;
        private System.Windows.Forms.Label PercentLabel;
        private System.Windows.Forms.Button MagTest;
        private System.Windows.Forms.ComboBox MagComboBox;
        private System.Windows.Forms.Label MagSelLabel;

    }
}