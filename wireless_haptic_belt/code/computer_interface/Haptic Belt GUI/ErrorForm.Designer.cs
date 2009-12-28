namespace HapticGUI
{
    partial class ErrorForm
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
            this.ErrorLocation = new System.Windows.Forms.Label();
            this.ErrorStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ErrorLocation
            // 
            this.ErrorLocation.AutoSize = true;
            this.ErrorLocation.Location = new System.Drawing.Point(12, 9);
            this.ErrorLocation.Name = "ErrorLocation";
            this.ErrorLocation.Size = new System.Drawing.Size(76, 13);
            this.ErrorLocation.TabIndex = 31;
            this.ErrorLocation.Text = "Error Location:";
            // 
            // ErrorStatus
            // 
            this.ErrorStatus.AutoSize = true;
            this.ErrorStatus.Location = new System.Drawing.Point(12, 25);
            this.ErrorStatus.Name = "ErrorStatus";
            this.ErrorStatus.Size = new System.Drawing.Size(65, 13);
            this.ErrorStatus.TabIndex = 30;
            this.ErrorStatus.Text = "Error Status:";
            // 
            // ErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(313, 43);
            this.Controls.Add(this.ErrorLocation);
            this.Controls.Add(this.ErrorStatus);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ErrorLocation;
        private System.Windows.Forms.Label ErrorStatus;

    }
}