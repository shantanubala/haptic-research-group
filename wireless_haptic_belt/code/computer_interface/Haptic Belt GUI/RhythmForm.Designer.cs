namespace HapticGUI
{
    partial class RhythmForm
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
            this.RhythmPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.RhythmDone = new System.Windows.Forms.Button();
            this.RhythmSet = new System.Windows.Forms.Button();
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
            this.RhythmTest = new System.Windows.Forms.Button();
            this.RhythmLabel = new System.Windows.Forms.Label();
            this.RhythmComboBox = new System.Windows.Forms.ComboBox();
            this.RhythmPatternList = new System.Windows.Forms.ListBox();
            this.RhythmPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RhythmOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RhythmOn)).BeginInit();
            this.SuspendLayout();
            // 
            // RhythmPanel
            // 
            this.RhythmPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.RhythmPanel.Controls.Add(this.label1);
            this.RhythmPanel.Controls.Add(this.RhythmDone);
            this.RhythmPanel.Controls.Add(this.RhythmSet);
            this.RhythmPanel.Controls.Add(this.RhythmTestStop);
            this.RhythmPanel.Controls.Add(this.RhythmTime);
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
            this.RhythmPanel.Controls.Add(this.RhythmLabel);
            this.RhythmPanel.Controls.Add(this.RhythmComboBox);
            this.RhythmPanel.Controls.Add(this.RhythmPatternList);
            this.RhythmPanel.Location = new System.Drawing.Point(7, 7);
            this.RhythmPanel.Name = "RhythmPanel";
            this.RhythmPanel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RhythmPanel.Size = new System.Drawing.Size(270, 248);
            this.RhythmPanel.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "label1";
            // 
            // RhythmDone
            // 
            this.RhythmDone.Location = new System.Drawing.Point(16, 222);
            this.RhythmDone.Name = "RhythmDone";
            this.RhythmDone.Size = new System.Drawing.Size(75, 23);
            this.RhythmDone.TabIndex = 40;
            this.RhythmDone.Text = "Done";
            this.RhythmDone.UseVisualStyleBackColor = true;
            this.RhythmDone.Click += new System.EventHandler(this.RhythmDone_Click);
            // 
            // RhythmSet
            // 
            this.RhythmSet.Location = new System.Drawing.Point(180, 222);
            this.RhythmSet.Name = "RhythmSet";
            this.RhythmSet.Size = new System.Drawing.Size(75, 23);
            this.RhythmSet.TabIndex = 39;
            this.RhythmSet.Text = "Set";
            this.RhythmSet.UseVisualStyleBackColor = true;
            this.RhythmSet.Click += new System.EventHandler(this.RhythmSet_Click);
            // 
            // RhythmTestStop
            // 
            this.RhythmTestStop.Enabled = false;
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
            this.RhythmComboBox.SelectedIndexChanged += new System.EventHandler(this.RhythmComboBox_SelectedIndexChanged);
            // 
            // RhythmPatternList
            // 
            this.RhythmPatternList.FormattingEnabled = true;
            this.RhythmPatternList.Location = new System.Drawing.Point(77, 74);
            this.RhythmPatternList.Name = "RhythmPatternList";
            this.RhythmPatternList.Size = new System.Drawing.Size(97, 95);
            this.RhythmPatternList.TabIndex = 4;
            // 
            // RhythmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.RhythmPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RhythmForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rhythm Mode";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.RhythmForm_Shown);
            this.RhythmPanel.ResumeLayout(false);
            this.RhythmPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RhythmOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RhythmOn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel RhythmPanel;
        private System.Windows.Forms.Button RhythmDone;
        private System.Windows.Forms.Button RhythmSet;
        private System.Windows.Forms.Button RhythmTestStop;
        private System.Windows.Forms.Label RhythmTime;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Button RhythmClear;
        private System.Windows.Forms.Panel RhythmPaint;
        private System.Windows.Forms.Button RhythmDelete;
        private System.Windows.Forms.Button RhythmReplace;
        private System.Windows.Forms.Button RhythmInsert;
        private System.Windows.Forms.Button RhythmAdd;
        private System.Windows.Forms.Label RhythmOffLabel;
        private System.Windows.Forms.Label RhythmOnLabel;
        private System.Windows.Forms.NumericUpDown RhythmOff;
        private System.Windows.Forms.NumericUpDown RhythmOn;
        private System.Windows.Forms.Button RhythmTest;
        private System.Windows.Forms.Label RhythmLabel;
        private System.Windows.Forms.ComboBox RhythmComboBox;
        private System.Windows.Forms.ListBox RhythmPatternList;
        private System.Windows.Forms.Label label1;
    }
}