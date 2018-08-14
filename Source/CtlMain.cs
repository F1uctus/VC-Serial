using System;
using System.Drawing;
using System.Windows.Forms;
using RJCP.IO.Ports;

namespace Serial {
    public partial class CtlMain : UserControl {
        private const string noPortsFound = "No available ports found.";

        public CtlMain() {
            InitializeComponent();
            lvPorts.Columns.Add("Port name", lvPorts.Width - 5);
            lvLog.Columns.Add("Message", (int) (lvLog.Width * 0.80));
            lvLog.Columns.Add("Time",    80);

            cbGenEventsOnReceive.Checked = PluginOptions.GenEventOnReceive;
            cbEnableLogging.Checked      = PluginOptions.SelectedPortLogging;
            cbLogAutoscroll.Checked      = PluginOptions.SelectedPortLogAutoScroll;
            //
            UpdatePortsListView();
        }

        private void cbGenEventsOnReceive_CheckedChanged(object sender, EventArgs e) {
            PluginOptions.GenEventOnReceive = cbGenEventsOnReceive.Checked;
        }

        private void cbEnableLogging_CheckedChanged(object sender, EventArgs e) {
            PluginOptions.SelectedPortLogging = cbEnableLogging.Checked;
            if (!cbEnableLogging.Checked && Plugin.SelectedPort.IsAlive()) {
                Log($"========== {Plugin.SelectedPort.PortName}: log pause ==========", false);
            }
        }

        private void cbLogAutoscroll_CheckedChanged(object sender, EventArgs e) {
            PluginOptions.SelectedPortLogAutoScroll = cbLogAutoscroll.Checked;
        }

        private void btSave_Click(object sender, EventArgs e) {
            // receive options from view
            PluginOptions.GenEventOnReceive   = cbGenEventsOnReceive.Checked;
            PluginOptions.SelectedPortLogging = cbEnableLogging.Checked;
            // save options
            try {
                PluginOptions.SaveOptionsToXml();

                // show that settings have been saved
                Color  prevBtBackColor = btSave.BackColor;
                string prevBtMessage   = btSave.Text;
                btSave.Enabled   = false;
                btSave.BackColor = Color.FromArgb(202, 81, 0);
                btSave.Text      = "Options saved.";
                var timer = new Timer { Interval = 2500 };
                timer.Tick += delegate {
                    btSave.BackColor = prevBtBackColor;
                    btSave.Text      = prevBtMessage;
                    btSave.Enabled   = true;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
            catch (Exception ex) {
                MessageBox.Show("An error occurred while saving options:\r\n" + ex);
            }
        }

        private void lvPorts_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                ListViewItem portView = lvPorts.FocusedItem;
                // if item not selected or "No ports" selected
                if (portView == null ||
                    portView.Text == noPortsFound) {
                    return;
                }
                string portLongName = portView.Text;

                // tricky way to show pop-up menu
                MenuItem         mi;
                SerialPortStream port;
                if (Plugin.OpenedPorts.TryGetValue(portLongName, out port)) {
                    mi       =  new MenuItem("Close port");
                    mi.Click += delegate { SerialPortActions.DestroyPort(port); };

                    if (Plugin.SelectedPort.IsAlive() &&
                        port.PortName != Plugin.SelectedPort.PortName) {
                        // add 'select port' entry
                        var mi2 = new MenuItem("Select port");
                        mi2.Click += delegate { SerialPortActions.SelectPort(port.PortName); };

                        new ContextMenu(new[] { mi, mi2 })
                            .Show(lvPorts, new Point(e.X, e.Y));
                        return;
                    }
                }
                else {
                    mi = new MenuItem("Open port");

                    #region 'open port' form design implementation

                    mi.Click += delegate {
                        // 
                        // labTitle
                        // 
                        var labTitle = new Label {
                            AutoSize = true,
                            Location = new Point(12, 4),
                            Size     = new Size(86, 14),
                            Text     = "Open new port"
                        };
                        // 
                        // cbEnableDtr
                        // 
                        var cbEnableDtr = new CheckBox {
                            AutoSize   = true,
                            Location   = new Point(11, 46),
                            Size       = new Size(191, 34),
                            CheckAlign = ContentAlignment.MiddleRight,
                            Text       = "Enable DTR\r\n(reload board after connection)"
                        };
                        // 
                        // numBaudRate
                        // 
                        var numBaudRate = new NumericUpDown {
                            Anchor = AnchorStyles.Top | AnchorStyles.Left
                                                      | AnchorStyles.Right,
                            BackColor = Color.FromArgb(55, 55, 55),
                            ForeColor = Color.Gainsboro,
                            Location  = new Point(75, 14),
                            Size      = new Size(122, 23),
                            Increment = 100m,
                            Maximum   = 999999999999m,
                            Minimum   = 9600m,
                            Value     = 9600m
                        };
                        // 
                        // labBaudRate
                        // 
                        var labBaudRate = new Label {
                            AutoSize = true,
                            Location = new Point(8, 17),
                            Size     = new Size(60, 15),
                            Text     = "Baud rate:"
                        };
                        // 
                        // btOk
                        // 
                        var btOk = new Button {
                            Anchor    = AnchorStyles.Bottom | AnchorStyles.Left,
                            BackColor = Color.FromArgb(0, 122, 204),
                            ForeColor = Color.White,
                            FlatAppearance = {
                                BorderSize = 0
                            },
                            FlatStyle    = FlatStyle.Flat,
                            Location     = new Point(11, 90),
                            Size         = new Size(88, 23),
                            Text         = "OK",
                            DialogResult = DialogResult.OK
                        };
                        // 
                        // btCancel
                        // 
                        var btCancel = new Button {
                            Anchor    = AnchorStyles.Bottom | AnchorStyles.Right,
                            BackColor = Color.FromArgb(0, 122, 204),
                            ForeColor = Color.White,
                            FlatAppearance = {
                                BorderSize = 0
                            },
                            FlatStyle    = FlatStyle.Flat,
                            Location     = new Point(109, 90),
                            Size         = new Size(88, 23),
                            Text         = "Cancel",
                            DialogResult = DialogResult.Cancel
                        };
                        // 
                        // panMain
                        // 
                        var panMain = new Panel {
                            Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                      | AnchorStyles.Left
                                                      | AnchorStyles.Right,
                            BackColor = Color.FromArgb(50, 50, 50),
                            Location  = new Point(1, 25),
                            Size      = new Size(208, 124),
                            Controls = {
                                cbEnableDtr,
                                numBaudRate,
                                labBaudRate,
                                btOk,
                                btCancel
                            }
                        };
                        // 
                        // formOpenPort
                        // 
                        var formOpenPort = new Form {
                            ClientSize      = new Size(210, 150),
                            Font            = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                            BackColor       = Color.FromArgb(202, 81, 0),
                            ForeColor       = Color.Gainsboro,
                            FormBorderStyle = FormBorderStyle.None,
                            StartPosition   = FormStartPosition.CenterParent,
                            Controls = {
                                labTitle,
                                panMain
                            }
                        };

                        btCancel.Click += delegate { formOpenPort.Close(); };
                        btOk.Click += delegate {
                            SerialPortActions.OpenPort(
                                portLongName,
                                (int) numBaudRate.Value,
                                cbEnableDtr.Checked
                            );
                        };

                        formOpenPort.ShowDialog();
                    };

                    #endregion
                }
                new ContextMenu(new[] { mi })
                    .Show(lvPorts, new Point(e.X, e.Y));
            }
        }

        /// <summary>
        ///     Appends <paramref name="message" /> to log
        ///     inside plugin window.
        /// </summary>
        public void Log(string message, bool portInput) {
            Action addLogItem = delegate {
                if (!message.EndsWith("\n")) {
                    message += "\r\n";
                }

                Color messageColor;
                if (portInput) {
                    message      = "< " + message;
                    messageColor = Color.LightGray;
                }
                else {
                    message      = "> " + message;
                    messageColor = Color.White;
                }
                ListViewItem addedItem = lvLog.Items.Add(message);
                addedItem.ForeColor = messageColor;
                addedItem.SubItems.Add(DateTime.Now.ToString("hh:mm:ss"));

                if (PluginOptions.SelectedPortLogAutoScroll) {
                    addedItem.EnsureVisible();
                }
                if (lvLog.Items.Count > PluginOptions.MaxLogItems) {
                    lvLog.Items.RemoveAt(0);
                }
            };

            if (InvokeRequired) {
                Invoke(addLogItem);
            }
            else {
                addLogItem();
            }
        }

        public void ClearLog() {
            lvLog.Items.Clear();
        }

        public void UpdatePortsListView(object sender = null, EventArgs e = null) {
            // clear list
            lvPorts.Items.Clear();

            string[] ports = SerialPortActions.GetPortsList(true, false);
            if (ports.Length == 0) {
                lvPorts.Items.Add("No available ports found.");
            }
            else {
                // populate list
                if (Plugin.OpenedPorts.Count > 0) {
                    foreach (string portLongName in ports) {
                        ListViewItem addedItem = lvPorts.Items.Add(portLongName);
                        // set opened port color
                        if (Plugin.OpenedPorts.ContainsKey(portLongName)) {
                            addedItem.ForeColor = Color.LimeGreen;
                        }
                        // highlight current port
                        if (Plugin.SelectedPort.IsAlive() &&
                            Plugin.OpenedPorts[portLongName].PortName == Plugin.SelectedPort.PortName) {
                            addedItem.ForeColor = Color.Gold;
                        }
                    }
                }
                // skip coloring if 0 opened ports
                else {
                    foreach (string port in ports) {
                        lvPorts.Items.Add(port);
                    }
                }
            }
            tbSendMessage.Enabled = btSend.Enabled = Plugin.SelectedPort.IsAlive();
        }

        private void lvPorts_Resize(object sender, EventArgs e) {
            lvPorts.Columns[0].Width = lvPorts.Width - 5;
        }

        private void lvLog_Resize(object sender, EventArgs e) {
            lvLog.Columns[0].Width = lvLog.Width - lvLog.Columns[1].Width;
        }

        private void btSend_Click(object sender, EventArgs e) {
            SendMessage();
        }

        private void tbSendMessage_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\r') {
                SendMessage();
                e.Handled = true;
            }
        }

        private void SendMessage() {
            Color btPrevColor = btSend.BackColor;
            Color btNewColor;
            btSend.Enabled = false;
            if (tbSendMessage.Text.Length > 0) {
                try {
                    SerialPortActions.WriteOnPort(tbSendMessage.Text);
                    tbSendMessage.Clear();
                    btNewColor = Color.LimeGreen;
                }
                catch {
                    btNewColor = Color.Red;
                }
            }
            else {
                btNewColor = Color.Red;
            }

            btSend.BackColor = btNewColor;
            var timer = new Timer { Interval = 1500 };
            timer.Tick += delegate {
                btSend.BackColor = btPrevColor;
                btSend.Enabled   = true;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}