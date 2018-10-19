using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using PluginInterface;
using RJCP.IO.Ports;

namespace Serial {
    internal static class SerialPortActions {
        private static readonly ManagementObjectSearcher portsSearcher = new ManagementObjectSearcher(
            "root\\CIMV2",
            "SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\""
        );

        internal static string[] GetPortsList(bool friendlyNames, bool onlyOpened) {
            var portsNames = new List<string>();
            Plugin.HostInstance.log(nameof(Serial) + ": Updating ports list");

            if (onlyOpened && Plugin.OpenedPorts.Count == 0) {
                Plugin.HostInstance.log(nameof(Serial) + ": no ports found");
                return new string[0];
            }

            using (ManagementObjectCollection ports = portsSearcher.Get()) {
                foreach (ManagementBaseObject port in ports) {
                    string portLongName = port["Name"].ToString();
                    Plugin.HostInstance.log(nameof(Serial) + ": found port: " + portLongName);
                    try {
                        if (onlyOpened && !Plugin.OpenedPorts.Keys.Contains(portLongName)) {
                            continue;
                        }
                        if (friendlyNames) {
                            portsNames.Add(portLongName);
                            continue;
                        }
                        Match match = Plugin.ComNameRegex.Match(portLongName);
                        portsNames.Add(match.Value);
                    }
                    finally {
                        port.Dispose();
                    }
                }
            }
            // clear opened ports from non-existing ports
            if (!onlyOpened) {
                foreach (string portLongName in Plugin.OpenedPorts.Keys) {
                    if (!portsNames.Contains(portLongName)) {
                        Plugin.HostInstance.log(nameof(Serial) + ": removed old port: " + portLongName);
                        DestroyPort(Plugin.OpenedPorts[portLongName]);
                        Plugin.OpenedPorts.Remove(portLongName);
                    }
                }
            }

            return portsNames.ToArray();
        }

        internal static void OpenPort(
            string   portLongName,
            int      baudRate  = 9600,
            bool     dtrEnable = false,
            Parity   parity    = Parity.None,
            StopBits stopBits  = StopBits.One,
            int      dataBits  = 8
        ) {
            Match match = Plugin.ComNameRegex.Match(portLongName);
            if (!match.Success) {
                throw new Exception("Port must have 'COM' type.");
            }
            Plugin.HostInstance.log(nameof(Serial) + ": initializing port: " + portLongName);
            string portComName = match.Value;
            var port = new SerialPortStream {
                PortName  = portComName,
                BaudRate  = baudRate,
                DtrEnable = dtrEnable,
                Parity    = parity,
                StopBits  = stopBits,
                DataBits  = dataBits,
                Encoding  = Encoding.UTF8
            };
            port.Open();

            Plugin.OpenedPorts.Add(portLongName, port);

            Plugin.HostInstance.log(nameof(Serial) + ": subscribing for port: " + portLongName);
            port.DataReceived += delegate {
                // added try catch to fix crashing when closing VC
                try {
                    Plugin.PortMessage = port.ReadExisting();
                    if (!string.IsNullOrWhiteSpace(Plugin.PortMessage)) {
                        if (PluginOptions.GenEventOnReceive) {
                            Plugin.HostInstance.triggerEvent(
                                "Serial.Received", new List<string> {
                                    port.PortName,
                                    Plugin.PortMessage
                                }
                            );
                        }
                    }
                }
                catch {
                    //
                }
            };

            SelectPort(portComName);
            Plugin.HostInstance.log(nameof(Serial) + ": opened port: " + portLongName);
        }

        internal static void SelectPort(string portComName) {
            Plugin.HostInstance.log(nameof(Serial) + ": select port: " + portComName);
            // unsubscribe previous selected port
            if (Plugin.SelectedPort.IsAlive()) {
                Plugin.SelectedPort.DataReceived -= SelectedPortLogOnDataReceived;
            }
            Plugin.SelectedPort = Plugin.OpenedPorts.Values.First(port => port.PortName == portComName);
            Plugin.MainCtl.UpdatePortsListView();
            Plugin.MainCtl.ClearLog();
            Plugin.MainCtl.Log($"========== {portComName}: log start ==========", false);
            // subscribe new port
            Plugin.SelectedPort.DataReceived += SelectedPortLogOnDataReceived;
        }

        private static void SelectedPortLogOnDataReceived(object sender, SerialDataReceivedEventArgs e) {
            // writing data to log
            if (PluginOptions.SelectedPortLogging) {
                Plugin.MainCtl.Log(Plugin.PortMessage, false);
            }
        }

        internal static actionResult WriteOnPort(string message) {
            var ar = new actionResult();
            if (!Plugin.SelectedPort.IsAlive() ||
                !Plugin.SelectedPort.IsOpen) {
                ar.setError("Current port isn't open.");
                return ar;
            }
            bool   converted;
            byte[] parsedBytes = Plugin.ParseHexString(message, out converted);
            if (converted) {
                Plugin.SelectedPort.Write(parsedBytes, 0, parsedBytes.Length);
                string byteMsg = Encoding.UTF8.GetString(parsedBytes);
                Plugin.MainCtl.Log(byteMsg, true);
                ar.setInfo("Bytes array sent: " + byteMsg);
                return ar;
            }

            Plugin.SelectedPort.Write(message);
            Plugin.MainCtl.Log(message, true);
            ar.setInfo("Successfully sent: " + message);
            return ar;
        }

        internal static actionResult DestroyPort(SerialPortStream port) {
            var ar = new actionResult();
            if (!port.IsAlive()) {
                ar.setError("Port already closed.");
                return ar;
            }

            if (port == Plugin.SelectedPort) {
                Plugin.MainCtl.Log($"=========== {port.PortName}: log end ===========", false);
            }
            string portLongName = Plugin.OpenedPorts.First(kvp => kvp.Value == port).Key;
            Plugin.OpenedPorts.Remove(portLongName);
            if (port.IsOpen) {
                port.Close();
            }
            port.Dispose();
            // ReSharper disable once RedundantAssignment
            port = null;
            Plugin.MainCtl.UpdatePortsListView();
            ar.setInfo("Port closed.");
            Plugin.HostInstance.log(nameof(Serial) + ": destroyed port: " + portLongName);
            return ar;
        }

        internal static bool IsAlive(this SerialPortStream port) {
            return port != null &&
                   !port.IsDisposed;
        }
    }
}