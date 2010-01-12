namespace HapticGUI
{
    public partial class ConfigForm
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
            this.MotorList = new System.Windows.Forms.ListBox();
            this.MotorsLabel = new System.Windows.Forms.Label();
            this.ConfigDone = new System.Windows.Forms.Button();
            this.ConfigPanel = new System.Windows.Forms.Panel();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.ConfigReset = new System.Windows.Forms.Button();
            this.ConfigPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MotorList
            // 
            this.MotorList.FormattingEnabled = true;
            this.MotorList.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.MotorList.Location = new System.Drawing.Point(131, 14);
            this.MotorList.Name = "MotorList";
            this.MotorList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.MotorList.Size = new System.Drawing.Size(22, 212);
            this.MotorList.TabIndex = 50;
            this.MotorList.SelectedIndexChanged += new System.EventHandler(this.MotorList_SelectedIndexChanged);
            // 
            // MotorsLabel
            // 
            this.MotorsLabel.AutoSize = true;
            this.MotorsLabel.Location = new System.Drawing.Point(106, 16);
            this.MotorsLabel.Name = "MotorsLabel";
            this.MotorsLabel.Size = new System.Drawing.Size(19, 208);
            this.MotorsLabel.TabIndex = 51;
            this.MotorsLabel.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7\r\n8\r\n9\r\n10\r\n11\r\n12\r\n13\r\n14\r\n15\r\n16";
            // 
            // ConfigDone
            // 
            this.ConfigDone.Location = new System.Drawing.Point(16, 226);
            this.ConfigDone.Name = "ConfigDone";
            this.ConfigDone.Size = new System.Drawing.Size(75, 23);
            this.ConfigDone.TabIndex = 52;
            this.ConfigDone.Text = "Done";
            this.ConfigDone.UseVisualStyleBackColor = true;
            this.ConfigDone.Click += new System.EventHandler(this.ConfigDone_Click);
            // 
            // ConfigPanel
            // 
            this.ConfigPanel.Controls.Add(this.HeaderLabel);
            this.ConfigPanel.Controls.Add(this.ConfigReset);
            this.ConfigPanel.Controls.Add(this.MotorsLabel);
            this.ConfigPanel.Controls.Add(this.ConfigDone);
            this.ConfigPanel.Controls.Add(this.MotorList);
            this.ConfigPanel.Location = new System.Drawing.Point(7, 7);
            this.ConfigPanel.Name = "ConfigPanel";
            this.ConfigPanel.Size = new System.Drawing.Size(270, 252);
            this.ConfigPanel.TabIndex = 53;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Location = new System.Drawing.Point(91, 1);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(86, 13);
            this.HeaderLabel.TabIndex = 54;
            this.HeaderLabel.Text = "Current/Modified";
            // 
            // ConfigReset
            // 
            this.ConfigReset.Location = new System.Drawing.Point(180, 226);
            this.ConfigReset.Name = "ConfigReset";
            this.ConfigReset.Size = new System.Drawing.Size(75, 23);
            this.ConfigReset.TabIndex = 53;
            this.ConfigReset.Text = "Reset";
            this.ConfigReset.UseVisualStyleBackColor = true;
            this.ConfigReset.Click += new System.EventHandler(this.ConfigReset_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.ConfigPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Motors";
            this.ConfigPanel.ResumeLayout(false);
            this.ConfigPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox MotorList;
        private System.Windows.Forms.Label MotorsLabel;
        private System.Windows.Forms.Button ConfigDone;
        private System.Windows.Forms.Panel ConfigPanel;
        private System.Windows.Forms.Button ConfigReset;
        private System.Windows.Forms.Label HeaderLabel;
    }
}