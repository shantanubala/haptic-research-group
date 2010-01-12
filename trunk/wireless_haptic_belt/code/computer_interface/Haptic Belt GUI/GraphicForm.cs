using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HapticBeltGUI
{
    public partial class GraphicForm : Form
    {
        Bitmap graphic;
        int motors;
        
        public GraphicForm(int motor_count)
        {
            InitializeComponent();
            motors = motor_count;
        }

        //Changes the cursor size in accordence to the content size, also redraws its background image
        private void content_SizeChanged(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(1, content.Height);
            cursor.Height = content.Height;
            for (int y = 0; y < content.Height; ++y)
                bmp.SetPixel(1, y, Color.Red);

            cursor.BackgroundImage = bmp;
        }

        private void GraphicForm_Shown(object sender, EventArgs e)
        {
            content.Height = motors * 25;
        }
        //Sets all given content to the content panel
        public void setContent(HapticGUI.GUI.Motor[] allActivations)
        {

        }

        public void addActivation(int motor_num, String[] activations, int delay)
        {
            int x, y = 0;

            /* Reset this motor # row */
            for (x = (delay * 10) / 125 ; x < content.Width; ++x) //Each 50ms = 4 pixels. We we multiple the numerator and divisor by 10, to avoid using doubles
                for (y = motor_num * 25; y < (motor_num * 25 + 25); ++y)
                    graphic.SetPixel(x, y, BackColor);
        }

        public void deleteActivation(int motor_num, int delay)
        {


        }

    }
}
