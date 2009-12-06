namespace Haptikos
{
    /// <summary>
    /// 
    /// </summary>
    partial class SettingsForm
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboBoxInbound = new System.Windows.Forms.ComboBox();
            this.comboBoxOutbound = new System.Windows.Forms.ComboBox();
            this.checkBoxComPortSame = new System.Windows.Forms.CheckBox();
            this.textBoxTimeout = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Inbound";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Outbound";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(3, 245);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 20);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(81, 245);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 20);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            // 
            // comboBoxInbound
            // 
            this.comboBoxInbound.Location = new System.Drawing.Point(81, 30);
            this.comboBoxInbound.Name = "comboBoxInbound";
            this.comboBoxInbound.Size = new System.Drawing.Size(100, 21);
            this.comboBoxInbound.TabIndex = 10;
            this.comboBoxInbound.SelectedIndexChanged += new System.EventHandler(this.comboBoxInbound_SelectedIndexChanged);
            // 
            // comboBoxOutbound
            // 
            this.comboBoxOutbound.BackColor = System.Drawing.SystemColors.ControlLight;
            this.comboBoxOutbound.Enabled = false;
            this.comboBoxOutbound.Location = new System.Drawing.Point(81, 58);
            this.comboBoxOutbound.Name = "comboBoxOutbound";
            this.comboBoxOutbound.Size = new System.Drawing.Size(100, 21);
            this.comboBoxOutbound.TabIndex = 11;
            // 
            // checkBoxComPortSame
            // 
            this.checkBoxComPortSame.AutoSize = true;
            this.checkBoxComPortSame.Checked = true;
            this.checkBoxComPortSame.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxComPortSame.Location = new System.Drawing.Point(15, 82);
            this.checkBoxComPortSame.Name = "checkBoxComPortSame";
            this.checkBoxComPortSame.Size = new System.Drawing.Size(178, 17);
            this.checkBoxComPortSame.TabIndex = 14;
            this.checkBoxComPortSame.Text = "Use the same COM port for both";
            this.checkBoxComPortSame.UseVisualStyleBackColor = true;
            this.checkBoxComPortSame.CheckedChanged += new System.EventHandler(this.checkBoxComPortSame_CheckedChanged);
            // 
            // textBoxTimeout
            // 
            this.textBoxTimeout.Location = new System.Drawing.Point(87, 133);
            this.textBoxTimeout.MaxLength = 5;
            this.textBoxTimeout.Name = "textBoxTimeout";
            this.textBoxTimeout.Size = new System.Drawing.Size(56, 20);
            this.textBoxTimeout.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "COM port Read Timeout (ms):";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTimeout);
            this.Controls.Add(this.checkBoxComPortSame);
            this.Controls.Add(this.comboBoxOutbound);
            this.Controls.Add(this.comboBoxInbound);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox comboBoxInbound;
        private System.Windows.Forms.ComboBox comboBoxOutbound;
        private System.Windows.Forms.CheckBox checkBoxComPortSame;
        private System.Windows.Forms.TextBox textBoxTimeout;
        private System.Windows.Forms.Label label3;
    }
}