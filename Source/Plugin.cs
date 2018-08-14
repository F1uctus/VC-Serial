using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PluginInterface;
using RJCP.IO.Ports;

namespace Serial {
    public class Plugin : IPlugin {
        public static readonly Regex                                ComNameRegex = new Regex(@"COM\d+");
        public static readonly Dictionary<string, SerialPortStream> OpenedPorts  = new Dictionary<string, SerialPortStream>();
        public static          SerialPortStream                     SelectedPort = null;

        internal static CtlMain MainCtl;
        internal static string  PortMessage;

        #region Required plugin attributes

        // Declarations of all our internal plugin variables.
        // VoxCommando expects these to be here so don't remove them.

        public string Name { get; } = nameof(Serial);

        public string Description { get; } = "Work with devices interacting with PC through the Serial / COM port.";

        public bool GenEvents { get; } = true;

        public string Author { get; } = "Nikitin Ilya";

        internal static IPluginHost HostInstance;

        public IPluginHost Host {
            get { return HostInstance; }
            set {
                HostInstance = value;
                MainCtl      = (CtlMain) MainInterface;
            }
        }

        public string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public UserControl MainInterface { get; }

        #endregion

        public Plugin() {
            MainCtl       = new CtlMain();
            MainInterface = MainCtl;
        }

        /// <summary>
        ///     First function called by the host.
        ///     Put anything needed to start with here first.
        ///     At this point the host object exists.
        ///     This function is called in a thread so be careful to handle all errors.
        ///     e.g. udpListener.myHost = this.Host;
        /// </summary>
        public void Initialize() {
        }

        /// <summary>
        ///     This is the main method called by VoxCommando when performing plugin actions. All actions go through here.
        /// </summary>
        /// <param name="actionNameArray">
        ///     An array of strings representing action name. Example action: xbmc.send >>>
        ///     actionNameArray[0] is the plugin name (xbmc), actionNameArray[1] is "send".
        /// </param>
        /// <param name="actionParameters">An array of strings representing our action parameters.</param>
        /// <returns>an actionResult</returns>
        public actionResult doAction(string[] actionNameArray, string[] actionParameters) {
            var          ar            = new actionResult();
            const string unknownAction = "Unknown " + nameof(Serial) + " plugin action.";

            if (actionNameArray.Length < 2) {
                ar.setError(unknownAction);
                return ar;
            }

            try {
                switch (actionNameArray[1].ToUpper()) {
                    case "OPEN": { // [PortName pattern], (BaudRate), (DTR), (Parity), (StopBits), (DataBits)

                        #region Parsing action parameters

                        Regex portNameRegex;
                        if (actionParameters.Length < 1 || !IsValidRegex(actionParameters[0], out portNameRegex)) {
                            ar.setError("'Port name pattern' missing or invalid.");
                            return ar;
                        }

                        var info = "";
                        // parse baud rate
                        int baudRate;
                        if (actionParameters.Length < 2 || !int.TryParse(actionParameters[1], out baudRate)) {
                            baudRate = 9600;
                            info     = "'Baud rate' undefined. Using 9600.\r\n";
                        }
                        // parse DTR
                        bool dtrEnable;
                        if (actionParameters.Length < 3 || !bool.TryParse(actionParameters[2], out dtrEnable)) {
                            dtrEnable =  false;
                            info      += "'Use DTR' undefined. Using false.\r\n";
                        }
                        // parse parity
                        Parity parity;
                        if (actionParameters.Length < 4 || !Enum.TryParse(actionParameters[3], true, out parity)) {
                            parity =  Parity.None;
                            info   += "'Parity' undefined. Using none.\r\n";
                        }
                        // parse stop bits
                        StopBits stopBits;
                        if (actionParameters.Length < 5 || !Enum.TryParse(actionParameters[4], true, out stopBits)) {
                            stopBits =  StopBits.One;
                            info     += "'Stop bits' undefined. Using one.\r\n";
                        }
                        // parse data bits
                        int dataBits;
                        if (actionParameters.Length < 6 || !int.TryParse(actionParameters[5], out dataBits)) {
                            dataBits =  8;
                            info     += "'Data bits' undefined. Using 8.\r\n";
                        }

                        #endregion

                        // [select] First step - searching it in opened ports
                        foreach (string portLongName in OpenedPorts.Keys) {
                            if (portNameRegex.Match(portLongName).Success) {
                                Match match = ComNameRegex.Match(portLongName);
                                if (!match.Success) {
                                    ar.setError("Port must have 'COM' type.");
                                    return ar;
                                }
                                string portComName = match.Value;
                                if (SelectedPort.PortName == portComName) {
                                    ar.setError($"{portLongName} already selected.");
                                }
                                else {
                                    SerialPortActions.SelectPort(portComName);
                                    ar.setInfo($"'{portLongName}' selected.");
                                }
                                return ar;
                            }
                        }

                        // [open] If not found, look for it through all system ports.
                        string[] portsNames = SerialPortActions.GetPortsList(true, false);
                        foreach (string portLongName in portsNames) {
                            // user pattern match
                            if (portNameRegex.Match(portLongName).Success) {
                                try {
                                    SerialPortActions.OpenPort(portLongName, baudRate, dtrEnable, parity, stopBits, dataBits);
                                    info += $"'{portLongName}' opened and selected.";
                                    ar.setInfo(info);
                                }
                                catch (Exception ex) {
                                    ar.setError(ex.Message);
                                }
                                return ar;
                            }
                        }

                        // failed
                        ar.setError("Matching port not found.");
                        return ar;
                    }

                    case "GETPORTS": { // (Friendly names), (Show only opened)
                        bool friendlyNames;
                        if (actionParameters.Length < 1 || !bool.TryParse(actionParameters[0], out friendlyNames)) {
                            friendlyNames = false;
                        }
                        bool onlyOpenedPorts;
                        if (actionParameters.Length < 2 || !bool.TryParse(actionParameters[1], out onlyOpenedPorts)) {
                            onlyOpenedPorts = false;
                        }

                        string[] portsNames = SerialPortActions.GetPortsList(friendlyNames, onlyOpenedPorts);
                        if (portsNames.Length == 0) {
                            ar.setError("No open ports.");
                        }
                        else {
                            ar.setSuccess(string.Join("\r\n", portsNames));
                        }
                        break;
                    }

                    case "WRITE": { // [Message to write on port]
                        if (actionParameters.Length < 1) {
                            ar.setError("1 parameter expected.");
                            return ar;
                        }

                        ar = SerialPortActions.WriteOnPort(actionParameters[0]);
                        break;
                    }

                    case "CLOSE": { // <No parameters>
                        ar = SerialPortActions.DestroyPort(SelectedPort);
                        break;
                    }

                    case "MICRO": {
                        if (actionNameArray.Length < 3) {
                            ar.setError(unknownAction);
                            return ar;
                        }
                        // any that action requires port to be closed.
                        SelectedPort.Close();
                        ar = ArduinoActions.DoDriverAction(actionNameArray, actionParameters);
                        SelectedPort.Open();
                        break;
                    }

                    default: {
                        ar.setError(unknownAction);
                        break;
                    }
                }
            }
            catch (Exception ex) {
                ar.setError(ex.ToString());
            }

            return ar;
        }

        /// <summary>
        ///     Releases all unmanaged resources (which implements IDisposable)
        /// </summary>
        public void Dispose() {
            // You must release all unmanaged resources here when program is stopped to prevent the memory leaks.
            for (var i = 0; i < OpenedPorts.Count; i++) {
                SerialPortActions.DestroyPort(OpenedPorts.Values.ElementAt(i));
            }
        }

        #region Other methods

        private static bool IsValidRegex(string pattern, out Regex regex) {
            if (string.IsNullOrWhiteSpace(pattern)) {
                regex = null;
                return false;
            }
            try {
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
                return true;
            }
            catch {
                regex = null;
                return false;
            }
        }

        internal static byte[] ParseHexString(string stringToParse, out bool converted) {
            var   bytesList = new List<byte>();
            Match match     = new Regex(@"\\[xX]([0-9A-Fa-f]{2})|(.)").Match(stringToParse);
            converted = false;
            while (match.Success) {
                if (match.Value.Length == 1) {
                    bytesList.AddRange(Encoding.UTF8.GetBytes(match.Value));
                }
                else {
                    bytesList.Add(byte.Parse(match.Value.Substring(2), NumberStyles.HexNumber));
                    converted = true;
                }

                match = match.NextMatch();
            }

            return bytesList.ToArray();
        }

        #endregion
    }
}