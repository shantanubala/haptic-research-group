using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HapticGUI
{
    public partial class ErrorForm : Form
    {
        TextWriter writeStream;
        
        /* Writes specified parameters to the form
         * if Boolean write is true, we will output 
         * this error event to GUI_Error_Log.txt
         */
        public ErrorForm(String errorSts, String errorLoc, Boolean write)
        {
            InitializeComponent();
            ErrorStatus.Text = "Error Status: " + errorSts;
            ErrorLocation.Text = "Error Location: " + errorLoc;

            if (write) //Writes errors to an error log
            {  
                try
                {
                    writeStream = new StreamWriter("GUI_Error_Log.txt");

                    writeStream.WriteLine(DateTime.Now);
                    writeStream.WriteLine(ErrorStatus.Text);
                    writeStream.WriteLine(ErrorLocation.Text);
                    writeStream.Close();
                }
                catch (Exception)
                {
                    //Grabs the error name from exception, ignores the rest of the information
                    ErrorStatus.Text = "Error Occured when trying to write to GUI_Error_Log.txt.\n" + ErrorStatus.Text;
                }
            }
        }
    }
}
