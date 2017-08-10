using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Windows.Forms;
using PluginInterface;

namespace Serial
{
    public partial class CtlMain : UserControl
    {
        public readonly List<string> OpenedPortsNames = new List<string>();

        private readonly PluginOptions Options;
        public CtlMain(PluginOptions pluginOptions)
        {
            InitializeComponent();
            PluginIcon.Image = Image.FromFile(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\icon.png");
            Options = pluginOptions;
            CBoxGenEventsOnReceive.Checked = Options.GenEventOnReceive;
            CBoxConcateStrings.Checked = Options.ConcateStrings;
            NumConcatenationInterval.Value = Options.ConcatenationInterval;
            CBoxConcateStrings.Enabled = CBoxGenEventsOnReceive.Checked;
            NumConcatenationInterval.Enabled = CBoxConcateStrings.Checked && CBoxConcateStrings.Enabled;
            UpdatePortsList(null, null);
        }

        public IPluginHost PluginHost { get; set; }

        public IPlugin Plugin { get; set; }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Options.GenEventOnReceive = CBoxGenEventsOnReceive.Checked;
            Options.ConcateStrings = CBoxConcateStrings.Checked;
            Options.ConcatenationInterval = (int)NumConcatenationInterval.Value;
            try
            {
                Options.SaveOptionsToXml();
                LabelSaved.Show();
                Timer t = new Timer { Interval = 2500 };
                t.Tick += delegate
                {
                    LabelSaved.Hide();
                    t.Stop();
                    t.Dispose();
                };
                t.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while saving options:\r\n" + ex);
            }
        }

        private void Header_Click(object sender, EventArgs e)
        {
            using (System.Diagnostics.Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))) { }
        }

        private void CBoxConcateStrings_CheckedChanged(object sender, EventArgs e)
        {
            NumConcatenationInterval.Enabled = CBoxConcateStrings.Checked;
        }

        private void CBoxGenEventsOnReceive_CheckedChanged(object sender, EventArgs e)
        {
            CBoxConcateStrings.Enabled = CBoxGenEventsOnReceive.Checked;
            NumConcatenationInterval.Enabled = CBoxConcateStrings.Checked && CBoxConcateStrings.Enabled;
        }

        public void UpdatePortsList(object sender, EventArgs e)
        {
            ListAllPorts.Clear();
            string[] Ports = SerialPort.GetPortNames();
            for (var Index = 0; Index < Ports.Length; Index++)
            {
                string PortName = Ports[Index];
                ListAllPorts.Items.Add(PortName);
                ListAllPorts.Items[Index].ForeColor = OpenedPortsNames.Contains(PortName)
                    ? Color.ForestGreen
                    : ListAllPorts.ForeColor;
            }
        }
    }
}