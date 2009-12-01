namespace Haptikos
{
    partial class PatternProgForm
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.labelMagProg = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboBoxMagSel = new System.Windows.Forms.ComboBox();
            this.comboBoxRhySel = new System.Windows.Forms.ComboBox();
            this.textBoxMagPercent = new System.Windows.Forms.TextBox();
            this.textBoxRhyPattern = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnProgMag = new System.Windows.Forms.Button();
            this.btnProgRhy = new System.Windows.Forms.Button();
            this.btnZap = new System.Windows.Forms.Button();
            this.btnQryMag = new System.Windows.Forms.Button();
            this.txtLogProg = new System.Windows.Forms.TextBox();
            this.btnQryRhy = new System.Windows.Forms.Button();
            this.btnQryAll = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxRhyTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelMagProg
            // 
            this.labelMagProg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelMagProg.Location = new System.Drawing.Point(5, 76);
            this.labelMagProg.Name = "labelMagProg";
            this.labelMagProg.Size = new System.Drawing.Size(120, 20);
            this.labelMagProg.Text = "Magnitude Settings:";
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
            // comboBoxMagSel
            // 
            this.comboBoxMagSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMagSel.Items.Add("A");
            this.comboBoxMagSel.Items.Add("B");
            this.comboBoxMagSel.Items.Add("C");
            this.comboBoxMagSel.Items.Add("D");
            this.comboBoxMagSel.Location = new System.Drawing.Point(14, 109);
            this.comboBoxMagSel.Name = "comboBoxMagSel";
            this.comboBoxMagSel.Size = new System.Drawing.Size(42, 21);
            this.comboBoxMagSel.TabIndex = 10;
            this.comboBoxMagSel.SelectedIndexChanged += new System.EventHandler(this.comboBoxMagSel_SelectedIndexChanged);
            // 
            // comboBoxRhySel
            // 
            this.comboBoxRhySel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRhySel.Items.Add("A");
            this.comboBoxRhySel.Items.Add("B");
            this.comboBoxRhySel.Items.Add("C");
            this.comboBoxRhySel.Items.Add("D");
            this.comboBoxRhySel.Items.Add("E");
            this.comboBoxRhySel.Items.Add("F");
            this.comboBoxRhySel.Items.Add("G");
            this.comboBoxRhySel.Items.Add("H");
            this.comboBoxRhySel.Location = new System.Drawing.Point(15, 35);
            this.comboBoxRhySel.Name = "comboBoxRhySel";
            this.comboBoxRhySel.Size = new System.Drawing.Size(42, 21);
            this.comboBoxRhySel.TabIndex = 14;
            this.comboBoxRhySel.SelectedIndexChanged += new System.EventHandler(this.comboBoxRhySel_SelectedIndexChanged);
            // 
            // textBoxMagPercent
            // 
            this.textBoxMagPercent.Location = new System.Drawing.Point(72, 110);
            this.textBoxMagPercent.MaxLength = 3;
            this.textBoxMagPercent.Name = "textBoxMagPercent";
            this.textBoxMagPercent.Size = new System.Drawing.Size(38, 20);
            this.textBoxMagPercent.TabIndex = 15;
            // 
            // textBoxRhyPattern
            // 
            this.textBoxRhyPattern.Location = new System.Drawing.Point(72, 35);
            this.textBoxRhyPattern.MaxLength = 16;
            this.textBoxRhyPattern.Name = "textBoxRhyPattern";
            this.textBoxRhyPattern.Size = new System.Drawing.Size(113, 20);
            this.textBoxRhyPattern.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 22);
            this.label1.Text = "Selection";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(62, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 19);
            this.label2.Text = "Percentage";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(6, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 20);
            this.label3.Text = "Rhythm Settings:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(69, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 22);
            this.label4.Text = "Rhythm Pattern (hex)";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 22);
            this.label5.Text = "Selection";
            // 
            // btnProgMag
            // 
            this.btnProgMag.Location = new System.Drawing.Point(131, 106);
            this.btnProgMag.Name = "btnProgMag";
            this.btnProgMag.Size = new System.Drawing.Size(96, 24);
            this.btnProgMag.TabIndex = 24;
            this.btnProgMag.Text = "Program Mag";
            this.btnProgMag.Click += new System.EventHandler(this.btnProgMag_Click);
            // 
            // btnProgRhy
            // 
            this.btnProgRhy.Location = new System.Drawing.Point(131, 61);
            this.btnProgRhy.Name = "btnProgRhy";
            this.btnProgRhy.Size = new System.Drawing.Size(96, 24);
            this.btnProgRhy.TabIndex = 25;
            this.btnProgRhy.Text = "Program Rhy";
            this.btnProgRhy.Click += new System.EventHandler(this.btnProgRhy_Click);
            // 
            // btnZap
            // 
            this.btnZap.Location = new System.Drawing.Point(160, 245);
            this.btnZap.Name = "btnZap";
            this.btnZap.Size = new System.Drawing.Size(68, 20);
            this.btnZap.TabIndex = 26;
            this.btnZap.Text = "Erase All";
            this.btnZap.Click += new System.EventHandler(this.btnZap_Click);
            // 
            // btnQryMag
            // 
            this.btnQryMag.Location = new System.Drawing.Point(151, 135);
            this.btnQryMag.Name = "btnQryMag";
            this.btnQryMag.Size = new System.Drawing.Size(77, 21);
            this.btnQryMag.TabIndex = 27;
            this.btnQryMag.Text = "Query Mag";
            this.btnQryMag.Click += new System.EventHandler(this.btnQryMag_Click);
            // 
            // txtLogProg
            // 
            this.txtLogProg.Enabled = false;
            this.txtLogProg.Location = new System.Drawing.Point(4, 161);
            this.txtLogProg.Multiline = true;
            this.txtLogProg.Name = "txtLogProg";
            this.txtLogProg.ReadOnly = true;
            this.txtLogProg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogProg.Size = new System.Drawing.Size(223, 78);
            this.txtLogProg.TabIndex = 28;
            // 
            // btnQryRhy
            // 
            this.btnQryRhy.Location = new System.Drawing.Point(77, 135);
            this.btnQryRhy.Name = "btnQryRhy";
            this.btnQryRhy.Size = new System.Drawing.Size(71, 21);
            this.btnQryRhy.TabIndex = 29;
            this.btnQryRhy.Text = "Query Rhy";
            this.btnQryRhy.Click += new System.EventHandler(this.btnQryRhy_Click);
            // 
            // btnQryAll
            // 
            this.btnQryAll.Location = new System.Drawing.Point(4, 135);
            this.btnQryAll.Name = "btnQryAll";
            this.btnQryAll.Size = new System.Drawing.Size(70, 21);
            this.btnQryAll.TabIndex = 30;
            this.btnQryAll.Text = "Query All";
            this.btnQryAll.Click += new System.EventHandler(this.btnQryAll_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(190, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 33);
            this.label6.Text = "Timing (50 ms)";
            // 
            // textBoxRhyTime
            // 
            this.textBoxRhyTime.Location = new System.Drawing.Point(193, 35);
            this.textBoxRhyTime.MaxLength = 2;
            this.textBoxRhyTime.Name = "textBoxRhyTime";
            this.textBoxRhyTime.Size = new System.Drawing.Size(34, 20);
            this.textBoxRhyTime.TabIndex = 31;
            // 
            // PatternProgForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.textBoxRhyTime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnQryAll);
            this.Controls.Add(this.btnQryRhy);
            this.Controls.Add(this.txtLogProg);
            this.Controls.Add(this.btnQryMag);
            this.Controls.Add(this.btnZap);
            this.Controls.Add(this.btnProgRhy);
            this.Controls.Add(this.textBoxRhyPattern);
            this.Controls.Add(this.comboBoxRhySel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.comboBoxMagSel);
            this.Controls.Add(this.btnProgMag);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxMagPercent);
            this.Controls.Add(this.labelMagProg);
            this.Menu = this.mainMenu1;
            this.Name = "PatternProgForm";
            this.Text = "Pattern Programming";
            this.ResumeLayout(false);
            //this.PerformLayout(); Windows PC App specific

        }

        #endregion

        private System.Windows.Forms.Label labelMagProg;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox comboBoxMagSel;
        private System.Windows.Forms.ComboBox comboBoxRhySel;
        private System.Windows.Forms.TextBox textBoxMagPercent;
        private System.Windows.Forms.TextBox textBoxRhyPattern;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnProgMag;
        private System.Windows.Forms.Button btnProgRhy;
        private System.Windows.Forms.Button btnZap;
        private System.Windows.Forms.Button btnQryMag;
        private System.Windows.Forms.TextBox txtLogProg;
        private System.Windows.Forms.Button btnQryRhy;
        private System.Windows.Forms.Button btnQryAll;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxRhyTime;
    }
}