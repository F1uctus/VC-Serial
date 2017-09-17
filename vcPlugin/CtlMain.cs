using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PluginInterface;

namespace Serial
{
    public partial class CtlMain : UserControl
    {
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
                var t = new Timer { Interval = 2500 };
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
            Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
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
            using (ManagementObjectCollection Ports = new ManagementObjectSearcher("root\\CIMV2", Serial.Plugin.SerialPortsQuery).Get())
            {
                int i = 0;
                foreach (ManagementBaseObject Device in Ports)
                {
                    try
                    {
                        ListAllPorts.Items.Add(Device["Name"].ToString());
                        if (Serial.Plugin.OpenedPorts.Keys.Contains(Device["Name"].ToString()))
                        {
                            ListAllPorts.Items[i].ForeColor = Color.ForestGreen;
                        }
                        if (Serial.Plugin.CurrentPort.PortName == new Regex(@"(COM|LPT)\d+").Match(Device["Name"].ToString()).Value)
                        {
                            ListAllPorts.Items[i].ForeColor = Color.Goldenrod;
                        }
                    }
                    catch { }
                    finally
                    {
                        Device.Dispose();
                    }
                    i++;
                }
            }
        }
    }
}