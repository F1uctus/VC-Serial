using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PluginInterface;
using System.Management;

namespace Serial
{
    public partial class CtlMain : UserControl
    {
        public const string SerialPortsQuery = "SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\"";
        public readonly Dictionary<string, SerialPort> OpenedPorts = new Dictionary<string, SerialPort>();

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
            using (Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))) { }
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
            using (ManagementObjectCollection Ports = new ManagementObjectSearcher("root\\CIMV2", SerialPortsQuery).Get())
            {
                int i = 0;
                foreach (ManagementBaseObject Dev in Ports)
                {
                    try
                    {
                        ListAllPorts.Items.Add(Dev["Name"].ToString());
                        if (OpenedPorts.Keys.Contains(Dev["Name"].ToString()))
                        {
                            ListAllPorts.Items.Cast<ListViewItem>().ElementAt(i).ForeColor = Color.ForestGreen;
                        }
                    }
                    catch { }
                    finally
                    {
                        Dev.Dispose();
                    }
                    i++;
                }
            }
        }
    }
}