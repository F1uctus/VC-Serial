using System;
using System.Drawing;
using System.IO;
using System.Reflection;
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
            NumBauds.Value = Options.Bauds;
            TxtPort.Text = Options.Port;
            CBoxGenEventsOnReceive.Checked = Options.GenEventOnReceive;
            CBoxConcateStrings.Checked = Options.ConcateStrings;
            NumConcatenationInterval.Value = Options.ConcatenationInterval;
            CBoxDTR.Checked = Options.DTRenabled;
            CBoxConcateStrings.Enabled = CBoxGenEventsOnReceive.Checked;
            NumConcatenationInterval.Enabled = CBoxConcateStrings.Checked && CBoxConcateStrings.Enabled;
        }

        public IPluginHost PluginHost { get; set; }

        public IPlugin Plugin { get; set; }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Options.Bauds = (int)NumBauds.Value;
            Options.Port = TxtPort.Text;
            Options.GenEventOnReceive = CBoxGenEventsOnReceive.Checked;
            Options.ConcateStrings = CBoxConcateStrings.Checked;
            Options.ConcatenationInterval = (int)NumConcatenationInterval.Value;
            Options.DTRenabled = CBoxDTR.Checked;
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
    }
}