using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GloveFeedbackInterface
{
    partial class HandForm
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
            this.components = new System.ComponentModel.Container();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.hand = new System.Windows.Forms.PictureBox();
            this.buzz = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.durationBox = new System.Windows.Forms.TextBox();
            this.durationLabel = new System.Windows.Forms.Label();
            this.repeatLabel = new System.Windows.Forms.Label();
            this.repeatBox = new System.Windows.Forms.TextBox();
            this.HapticGlove = new System.IO.Ports.SerialPort(this.components);
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hand)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 575);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(497, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(257, 17);
            this.statusLabel.Text = "Click the hand and press the \"Buzz\" button to begin.";
            // 
            // hand
            // 
            this.hand.Image = global::GloveFeedbackInterface.Properties.Resources.hand;
            this.hand.Location = new System.Drawing.Point(12, 12);
            this.hand.Name = "hand";
            this.hand.Size = new System.Drawing.Size(467, 427);
            this.hand.TabIndex = 2;
            this.hand.TabStop = false;
            // 
            // buzz
            // 
            this.buzz.Location = new System.Drawing.Point(12, 446);
            this.buzz.Name = "buzz";
            this.buzz.Size = new System.Drawing.Size(467, 39);
            this.buzz.TabIndex = 3;
            this.buzz.Text = "Buzz";
            this.buzz.UseVisualStyleBackColor = true;
            this.buzz.Click += new System.EventHandler(this.buzz_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(47, 275);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 43);
            this.button1.TabIndex = 4;
            this.button1.Text = "A";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(102, 369);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(39, 36);
            this.button2.TabIndex = 5;
            this.button2.Text = "B";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(132, 77);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(32, 39);
            this.button3.TabIndex = 6;
            this.button3.Text = "C";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(149, 156);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(32, 40);
            this.button4.TabIndex = 7;
            this.button4.Text = "D";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(171, 258);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(47, 60);
            this.button5.TabIndex = 8;
            this.button5.Text = "E";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(244, 68);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(30, 39);
            this.button6.TabIndex = 9;
            this.button6.Text = "F";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point(244, 143);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(30, 44);
            this.button7.TabIndex = 10;
            this.button7.Text = "G";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Location = new System.Drawing.Point(234, 247);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(50, 53);
            this.button8.TabIndex = 11;
            this.button8.Text = "H";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button9.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Location = new System.Drawing.Point(360, 77);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(33, 39);
            this.button9.TabIndex = 12;
            this.button9.Text = "I";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button10.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Location = new System.Drawing.Point(338, 156);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(34, 40);
            this.button10.TabIndex = 13;
            this.button10.Text = "J";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Location = new System.Drawing.Point(310, 258);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(45, 42);
            this.button11.TabIndex = 14;
            this.button11.Text = "K";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button12.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Location = new System.Drawing.Point(360, 320);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(33, 53);
            this.button12.TabIndex = 15;
            this.button12.Text = "N";
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button13.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button13.Location = new System.Drawing.Point(407, 229);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(29, 30);
            this.button13.TabIndex = 16;
            this.button13.Text = "M";
            this.button13.UseVisualStyleBackColor = false;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            this.button14.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button14.Location = new System.Drawing.Point(428, 156);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(32, 31);
            this.button14.TabIndex = 17;
            this.button14.Text = "L";
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.durationBox);
            this.groupBox1.Controls.Add(this.durationLabel);
            this.groupBox1.Controls.Add(this.repeatLabel);
            this.groupBox1.Controls.Add(this.repeatBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 492);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(467, 67);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // durationBox
            // 
            this.durationBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.durationBox.Location = new System.Drawing.Point(342, 19);
            this.durationBox.Name = "durationBox";
            this.durationBox.Size = new System.Drawing.Size(100, 31);
            this.durationBox.TabIndex = 3;
            this.durationBox.Text = "1";
            // 
            // durationLabel
            // 
            this.durationLabel.AutoSize = true;
            this.durationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.durationLabel.Location = new System.Drawing.Point(237, 22);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(99, 25);
            this.durationLabel.TabIndex = 2;
            this.durationLabel.Text = "Duration:";
            // 
            // repeatLabel
            // 
            this.repeatLabel.AutoSize = true;
            this.repeatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.repeatLabel.Location = new System.Drawing.Point(6, 22);
            this.repeatLabel.Name = "repeatLabel";
            this.repeatLabel.Size = new System.Drawing.Size(87, 25);
            this.repeatLabel.TabIndex = 1;
            this.repeatLabel.Text = "Repeat:";
            // 
            // repeatBox
            // 
            this.repeatBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.repeatBox.Location = new System.Drawing.Point(99, 19);
            this.repeatBox.Name = "repeatBox";
            this.repeatBox.Size = new System.Drawing.Size(116, 31);
            this.repeatBox.TabIndex = 0;
            this.repeatBox.Tag = "";
            this.repeatBox.Text = "1";
            // 
            // HapticGlove
            // 
            this.HapticGlove.BaudRate = 19200;
            this.HapticGlove.PortName = "COM6";
            // 
            // HandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.BackgroundImage = global::GloveFeedbackInterface.Properties.Resources.bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(497, 597);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buzz);
            this.Controls.Add(this.hand);
            this.Controls.Add(this.statusStrip);
            this.Name = "HandForm";
            this.Text = "Form1";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hand)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.PictureBox hand;
        private System.Windows.Forms.Button buzz;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.Label repeatLabel;
        private System.Windows.Forms.TextBox repeatBox;
        private System.Windows.Forms.TextBox durationBox;
        private System.IO.Ports.SerialPort HapticGlove;
    }
}

