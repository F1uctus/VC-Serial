using ArduinoUploader;
using ArduinoUploader.Hardware;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Serial
{
    public class Plugin : IPlugin
    {
        //option variables
        private readonly PluginOptions Options = new PluginOptions();
        private readonly List<SerialPort> OpenedPorts = new List<SerialPort>();
        
        private CtlMain MainCtl;
        private SerialPort CurrentPort;
        private string PortMessage;

        private string ArduinoPort = "";
        private int ArduinoBauds = 9600;
        private bool ArduinoDTR;

        public Plugin()
        {
            //
            // TODO: Add constructor logic here
            //
            MainInterface = new CtlMain(Options);
            MainCtl = (CtlMain)MainInterface;
        }

        #region stuff to edit for new plugins

        private IPluginHost myHost;

        public string Name { get; } = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public string Description { get; } = "Plugin for working with COM port.";

        public bool GenEvents { get; } = true;

        public string Author { get; } = "Nikitin Ilya";

        public IPluginHost Host
        {
            get => myHost;
            set
            {
                myHost = value;
                MainCtl = (CtlMain)MainInterface;
                MainCtl.PluginHost = Host;
                MainCtl.Plugin = this;
            }
        }
        public string Version { get; } = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public UserControl MainInterface { get; }

        #endregion

        public void Initialize()
        {
            //This is the first Function called by the host...
            //Put anything needed to start with here first
            //At this point the host object exists.
            //This function is called in a thread so be careful to handle all errors.
            //e.g.    udpListener.myHost = this.Host;
        }

        private actionResult OpenPortFromList(string portName)
        {
            actionResult AR = new actionResult();
            try
            {
                CurrentPort = OpenedPorts.Find(port => port.PortName == portName);
                CurrentPort.DataReceived += CurrentPort_DataReceived;
                CurrentPort.Open();
                AR.setInfo("Port selected.");
            }
            catch (Exception ex)
            {
                AR.setError(ex.Message);
            }
            return AR;
        }

        private void CurrentPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (CurrentPort == null) return;
            if (!Options.GenEventOnReceive)
            {
                CurrentPort.DiscardInBuffer();
                return;
            }
            if (Options.ConcateStrings)
            {
                new Thread(delegate ()
                {
                    Thread.Sleep(Options.ConcatenationInterval);
                    PortMessage = CurrentPort.ReadExisting();
                    if (PortMessage != "")
                    {
                        Host.triggerEvent("Serial.Received", new List<string> { ((SerialPort)sender).PortName, PortMessage });
                    }
                }).Start();
            }
            else
            {
                PortMessage = CurrentPort.ReadExisting();
                if (PortMessage != "")
                {
                    Host.triggerEvent("Serial.Received", new List<string> { ((SerialPort)sender).PortName, PortMessage });
                }
            }
        }

        public actionResult doAction(string[] parsedActions, string[] parsedParams)
        {
            string[] AllPorts = SerialPort.GetPortNames();
            actionResult AR = new actionResult();
            string LowerActionName = parsedActions[1].ToLower();
            if ((LowerActionName == "write" || LowerActionName == "upload" || LowerActionName == "close") &&
                (CurrentPort == null || !AllPorts.Contains(CurrentPort.PortName) || !CurrentPort.IsOpen))
            {
                AR.setError("Selected port doesn't exists\\removed\\closed.");
                return AR;
            }
            try
            {
                switch (LowerActionName)
                {
                    case "select":
                        {
                            if (string.IsNullOrWhiteSpace(parsedParams[0]))
                            {
                                AR.setError("Incorrect port name. Using last selected port.");
                            }
                            else if (!MainCtl.OpenedPortsNames.Contains(parsedParams[0]))
                            {
                                AR.setError("Specified port isn't open. Using last selected port.");
                            }
                            else if (CurrentPort != null && CurrentPort.PortName == parsedParams[0])
                            {
                                AR.setInfo("Port already selected.");
                            }
                            else
                            {
                                if (parsedParams.Length == 1 || parsedParams[1].ToLower() != "true")
                                {
                                    OpenPortFromList(parsedParams[0]);
                                    AR.setInfo("Using specified port.");
                                }
                                else // Scan Arduino
                                {
                                    CurrentPort = new SerialPort
                                    {
                                        PortName = ArduinoPort,
                                        BaudRate = ArduinoBauds,
                                        DtrEnable = ArduinoDTR
                                    };
                                }
                            }
                            return AR;
                        }
                    case "open": // [0] - PortName, [1] - Bauds, [2] - DTR, [3] - "Find Arduino"
                        {
                            if (!MainCtl.OpenedPortsNames.Contains(parsedParams[0]))
                            {
                                if (!int.TryParse(parsedParams[1], out int Bauds))
                                {
                                    AR.setError("Incorrect port Baud rate.");
                                }
                                else if (!bool.TryParse(parsedParams[2], out bool UseDtr))
                                {
                                    AR.setError("Incorrect \"Use DTR\" parameter.");
                                }
                                else
                                {
                                    if (parsedParams.Length < 4 || parsedParams[3].ToLower() != "true")
                                    {
                                        OpenedPorts.Add(new SerialPort
                                        {
                                            PortName = parsedParams[0],
                                            BaudRate = Bauds,
                                            DtrEnable = UseDtr
                                        });
                                        MainCtl.OpenedPortsNames.Add(parsedParams[0]);
                                        AR = OpenPortFromList(parsedParams[0]);
                                    }
                                    else // Scan Arduino
                                    {
                                        if (AutoOpenArduinoPort(Bauds, UseDtr))
                                        {
                                            AR.setInfo("Arduino port selected.");
                                        }
                                        else
                                        {
                                            AR.setError("Arduino not found.");
                                        }
                                    }
                                    MainCtl.UpdatePortsList(null, null);
                                }
                            }
                            else
                            {
                                AR.setError("Port already opened.");
                            }
                            return AR;
                        }
                    case "getports":
                        {
                            if (!bool.TryParse(parsedParams[0], out bool FriendlyNames) ||
                                !bool.TryParse(parsedParams[1], out bool ShowOnlyOpenedPorts))
                            {
                                FriendlyNames = false;
                                ShowOnlyOpenedPorts = false;
                            }
                            if (FriendlyNames)
                            {
                                List<string> Names = new List<string>();
                                using (ManagementObjectCollection Ports = new ManagementObjectSearcher("Select * from Win32_SerialPort").Get())
                                {
                                    foreach (ManagementBaseObject Dev in Ports)
                                    {
                                        try
                                        {
                                            if (ShowOnlyOpenedPorts)
                                            {
                                                if (MainCtl.OpenedPortsNames.Contains(Dev["DeviceID"].ToString()))
                                                {
                                                    Names.Add($"{Dev["Name"]}\r\n");
                                                }
                                            }
                                            else
                                            {
                                                Names.Add($"{Dev["Name"]}\r\n");
                                            }
                                        }
                                        finally
                                        {
                                            Dev.Dispose();
                                        }
                                    }
                                }
                                AR.setSuccess(string.Join("\r\n", Names));
                            }
                            else
                            {
                                AR.setSuccess(ShowOnlyOpenedPorts
                                    ? string.Join("\r\n", MainCtl.OpenedPortsNames)
                                    : string.Join("\r\n", AllPorts));
                            }
                            break;
                        }
                    case "write":
                        {
                            byte[] ParsedBytes = ParseHexString(parsedParams[0], out bool converted);
                            if (converted)
                            {
                                CurrentPort.Write(ParsedBytes, 0, ParsedBytes.Length);
                                AR.setInfo("Bytes array sent: " + Encoding.UTF8.GetString(ParsedBytes));
                                break;
                            }
                            CurrentPort.Write(parsedParams[0]);
                            AR.setInfo("Succesfully sent: " + parsedParams[0]);
                            break;
                        }
                    case "upload":
                        {
                            AR = UploadSketch(parsedParams[0], parsedParams[1]);
                            break;
                        }
                    case "close":
                        {
                            OpenedPorts.Remove(CurrentPort);
                            MainCtl.OpenedPortsNames.Remove(CurrentPort.PortName);
                            CurrentPort.Close();
                            CurrentPort.Dispose();
                            CurrentPort = null;
                            MainCtl.UpdatePortsList(null, null);
                            AR.setInfo("Port closed");
                            break;
                        }
                    default:
                        {
                            AR.setError("Unknown Serial plugin action");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                AR.setError(ex.ToString());
            }
            return AR;
        }

        #region other methods

        // put other methods here.  perhaps called by 'doAction'
        private bool AutoOpenArduinoPort(int bauds, bool dtr)
        {
            using (ManagementObjectCollection Ports = new ManagementObjectSearcher("Select * from Win32_SerialPort").Get())
            {
                foreach (ManagementBaseObject Dev in Ports)
                {
                    try
                    {
                        if ($"{Dev["Description"]}{Dev["Name"]}".Contains("duino"))
                        {
                            OpenedPorts.Add(new SerialPort
                            {
                                PortName = ArduinoPort = Dev["DeviceID"].ToString(),
                                BaudRate = ArduinoBauds = bauds,
                                DtrEnable = ArduinoDTR = dtr
                            });
                            MainCtl.OpenedPortsNames.Add(ArduinoPort);
                            OpenPortFromList(ArduinoPort);
                            return true;
                        }
                    }
                    finally
                    {
                        Dev.Dispose();
                    }
                }
            }
            return false;
        }

        private actionResult UploadSketch(string arduinoModel, string file)
        {
            actionResult AR = new actionResult();
            ArduinoModel Model;
            switch (arduinoModel.ToLower())
            {
                case "unor3":
                    Model = ArduinoModel.UnoR3;
                    break;
                case "nanor3":
                    Model = ArduinoModel.NanoR3;
                    break;
                case "micro":
                    Model = ArduinoModel.Micro;
                    break;
                case "mega2560":
                    Model = ArduinoModel.Mega2560;
                    break;
                case "mega1284":
                    Model = ArduinoModel.Mega1284;
                    break;
                default:
                    AR.setError("Некорректная модель Arduino");
                    return AR;
            }
            if (!File.Exists(file))
            {
                AR.setError("Указанный файл не существует");
                return AR;
            }
            string PortToUpload = CurrentPort.PortName;
            CurrentPort.Close();
            CurrentPort.Dispose();
            new ArduinoSketchUploader(new ArduinoSketchUploaderOptions
            {
                ArduinoModel = Model,
                FileName = file,
                PortName = PortToUpload
            }).UploadSketch();
            AR.setInfo("OK");
            return AR;
        }

        private static byte[] ParseHexString(string stringToParse, out bool converted)
        {
            List<byte> BytesList = new List<byte>();
            Match M = new Regex(@"\\[xX]([0-9A-Fa-f]{2})|(.)").Match(stringToParse);
            converted = false;
            while (M.Success)
            {
                if (M.Value.Length == 1)
                {
                    BytesList.AddRange(Encoding.UTF8.GetBytes(M.Value));
                }
                else
                {
                    BytesList.Add(byte.Parse(M.Value.Substring(2), NumberStyles.HexNumber));
                    converted = true;
                }
                M = M.NextMatch();
            }
            return BytesList.ToArray();
        }

        #endregion other methods

        public void Dispose()
        {
            for (int i = 0; i < OpenedPorts.Count; i++)
            {
                OpenedPorts[i].Close();
                OpenedPorts[i].Dispose();
            }
        }
    }
}