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
using ArduinoUploader;
using ArduinoUploader.Hardware;
using PluginInterface;

namespace Serial
{
    public class Plugin : IPlugin
    {
        //option variables
        private readonly PluginOptions Options = new PluginOptions();
        private CtlMain MainCtl;
        private SerialPort CurrentPort;
        private string PortMessage;
        private string ArduinoPort;

        public Plugin()
        {
            //
            // TODO: Add constructor logic here
            //
            MainInterface = new CtlMain(Options);
            MainCtl = (CtlMain)MainInterface;
        }

        #region stuff to edit for new plugins

        IPluginHost myHost;
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

        private void OpenNewPort(string portName)
        {
            CurrentPort = new SerialPort(portName, Options.Bauds)
            {
                DtrEnable = Options.DTRenabled
            };
            CurrentPort.DataReceived += CurrentPort_DataReceived;
            CurrentPort.Open();
        }

        private void CurrentPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!Options.GenEventOnReceive)
            {
                CurrentPort.DiscardInBuffer();
                return;
            }
            if (CurrentPort == null) return;
            if (Options.ConcateStrings && Options.ConcatenationInterval != 0)
            {
                new Thread(delegate ()
                   {
                       Thread.Sleep(Options.ConcatenationInterval);
                       PortMessage = CurrentPort.ReadExisting();
                       if (PortMessage != "")
                       {
                           Host.triggerEvent("Serial.Received", new List<string> { PortMessage });
                       }
                   }).Start();
            }
            else
            {
                PortMessage = CurrentPort.ReadExisting();
                if (PortMessage != "")
                {
                    Host.triggerEvent("Serial.Received", new List<string> { PortMessage });
                }
            }
        }

        public actionResult doAction(string[] parsedActions, string[] parsedParams)
        {
            actionResult AR = new actionResult();
            try
            {
                if (parsedActions[1].ToLower() == "close")
                {
                    if (CurrentPort != null && CurrentPort.IsOpen)
                    {
                        CurrentPort.Close();
                        CurrentPort.Dispose();
                        AR.setInfo("Port closed");
                        return AR;
                    }
                    AR.setError("Port haven't been opened.");
                    return AR;
                }
                if (CurrentPort == null || CurrentPort.PortName != Options.Port && CurrentPort.PortName != ArduinoPort) // if port empty
                {
                    try { CurrentPort.Close(); }
                    catch { }
                    if (SerialPort.GetPortNames().Contains(Options.Port.Trim())) // port from options exists
                    {
                        OpenNewPort(Options.Port);
                    }
                    else if (!AutoOpenArduinoPort())
                    {
                        AR.setError("Port not found.");
                        return AR;
                    }
                }
                switch (parsedActions[1].ToLower())
                {
                    case "upload":
                        AR = UploadSketch(parsedParams[0], parsedParams[1]);
                        break;
                    case "open":
                        AR.setInfo("OK");
                        break;
                    case "write":
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
                    default:
                        AR.setError("Unknown Serial plugin action");
                        break;
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
        private bool AutoOpenArduinoPort()
        {
            using (ManagementObjectCollection Ports = new ManagementObjectSearcher("Select * from Win32_SerialPort").Get())
            {
                foreach (ManagementBaseObject Dev in Ports)
                {
                    try
                    {
                        if (!$"{Dev["Description"]}{Dev["Name"]}".Contains("duino")) continue;

                        ArduinoPort = Dev["DeviceID"].ToString();
                        OpenNewPort(ArduinoPort);
                        return true;
                    }
                    catch
                    {
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
            ArduinoModel model;
            switch (arduinoModel.ToLower())
            {
                case "unor3":
                    model = ArduinoModel.UnoR3;
                    break;
                case "nanor3":
                    model = ArduinoModel.NanoR3;
                    break;
                case "micro":
                    model = ArduinoModel.Micro;
                    break;
                case "mega2560":
                    model = ArduinoModel.Mega2560;
                    break;
                case "mega1284":
                    model = ArduinoModel.Mega1284;
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
                ArduinoModel = model,
                FileName = file,
                PortName = PortToUpload
            }).UploadSketch();
            AR.setInfo("OK");
            return AR;
        }

        private static byte[] ParseHexString(string stringToParse, out bool converted)
        {
            List<byte> list = new List<byte>();
            Match M = new Regex(@"\\[xX]([0-9A-Fa-f]{2})|(.)").Match(stringToParse);
            converted = false;
            while (M.Success)
            {
                if (M.Value.Length == 1)
                {
                    list.AddRange(Encoding.UTF8.GetBytes(M.Value));
                }
                else
                {
                    list.Add(byte.Parse(M.Value.Substring(2), NumberStyles.HexNumber));
                    converted = true;
                }
                M = M.NextMatch();
            }
            return list.ToArray();
        }

        #endregion other methods

        public void Dispose()
        {
            CurrentPort.Close();
            CurrentPort.Dispose();
        }
    }
}