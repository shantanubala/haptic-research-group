using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
namespace Haptikos
{
    public partial class SettingsForm : Form
    {
        private string  inboundPort;
        private string outboundPort;
        public SettingsForm(string inbound,string outbound)
        {
            InitializeComponent();
            
            // Setup ports
            inboundPort = inbound;
            outboundPort = outbound;
            string[] ports = SerialPort.GetPortNames();
            
            // ComboBox 1 = inbound ports
            comboBoxInbound.Items.Add("NO PORT SELECTED");
            for (int i = 0; i < ports.Length; i++)
                comboBoxInbound.Items.Add(ports[i]);

            // ComboBox 2 = outbound ports
            comboBoxOutbound.Items.Add("NO PORT SELECTED");
            for (int i = 0; i < ports.Length; i++)
                comboBoxOutbound.Items.Add(ports[i]);
           
            // Selection of port names
            if (comboBoxInbound.Items.Contains(inboundPort))
                comboBoxInbound.SelectedItem=inboundPort;
            else
                comboBoxInbound.SelectedIndex = 0;
            
            if (comboBoxOutbound.Items.Contains(outboundPort))
                comboBoxOutbound.SelectedItem=outboundPort;
            else
                comboBoxOutbound.SelectedIndex = 0;

            // TODO future work Setup other Serial port settings such as Parity, Stop bits ,etc.

            // CONVERT TO FOR LOOP (foreach is not efficient on embedded processor)
            //foreach (string str in Enum.GetNames(typeof(StopBits)))
            //{
            //    ((ComboBox)obj).Items.Add(str);
            //}

            // CONVERT TO FOR LOOP (foreach is not efficient on embedded processor)
            //foreach (string str in Enum.GetNames(typeof(Parity)))
            //{
            //    ((ComboBox)obj).Items.Add(str);
            //}

        }
        public string GetInboundPort()
        {
            return (string)comboBoxInbound.SelectedItem;
        }
        public string GetOutboundPort()
        {
            return (string)comboBoxOutbound.SelectedItem;
        }
        //public string CheckHardwareEnable() {
        //    return (string)System.IO.Ports.SerialPort.;
        //}
    }
}