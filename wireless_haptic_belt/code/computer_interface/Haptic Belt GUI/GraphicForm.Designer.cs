namespace HapticBeltGUI
{
    partial class GraphicForm
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
            this.content = new System.Windows.Forms.Panel();
            this.cursor = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // content
            // 
            this.content.Location = new System.Drawing.Point(12, 12);
            this.content.Name = "content";
            this.content.Size = new System.Drawing.Size(200, 25);
            this.content.TabIndex = 2;
            this.content.SizeChanged += new System.EventHandler(this.content_SizeChanged);
            // 
            // cursor
            // 
            this.cursor.Location = new System.Drawing.Point(12, 12);
            this.cursor.Name = "cursor";
            this.cursor.Size = new System.Drawing.Size(1, 25);
            this.cursor.TabIndex = 3;
            // 
            // GraphicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(284, 64);
            this.Controls.Add(this.cursor);
            this.Controls.Add(this.content);
            this.Name = "GraphicForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.Shown += new System.EventHandler(this.GraphicForm_Shown);

        }

        #endregion

        private System.Windows.Forms.Panel content;
        private System.Windows.Forms.Panel cursor;

    }
}