using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ArduinoUploader;
using ArduinoUploader.Hardware;
using Microsoft.Win32;
using PluginInterface;

namespace Serial
{
	public class Plugin : IPlugin
	{
		//option variables
		public const string SerialPortsQuery = "SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\"";
		public static readonly Dictionary<string, SerialPort> OpenedPorts = new Dictionary<string, SerialPort>();
		private readonly PluginOptions Options = new PluginOptions();
		private CtlMain MainCtl;
		public static SerialPort CurrentPort;
		private string PortMessage;

		public Plugin()
		{
			//
			// TODO: Add constructor logic here
			//
			MainInterface = new CtlMain(Options);
			MainCtl = (CtlMain)MainInterface;
		}

		#region Plugin Attributes (get values from assembly attributes)

		public string Name { get; } = Assembly.GetExecutingAssembly().GetName().Name;

		public string Description { get; }
			= Assembly.GetExecutingAssembly()
			.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
			.OfType<AssemblyDescriptionAttribute>()
		.FirstOrDefault()?.ToString();

		public bool GenEvents { get; } = true;

		public string Author { get; }
			= Assembly.GetExecutingAssembly()
			.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)
			.OfType<AssemblyCompanyAttribute>()
		.FirstOrDefault()?.ToString();

		private IPluginHost myHost;

		public IPluginHost Host
		{
			get { return myHost; }
			set
			{
				myHost = value;
				MainCtl = (CtlMain)MainInterface;
				MainCtl.PluginHost = Host;
				MainCtl.Plugin = this;
			}
		}

		public string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

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

		private void SelectPortFromOpenedList(string portName)
		{
			try
			{
				CurrentPort = OpenedPorts.Values.First(port => port.PortName == portName);
				CurrentPort.DataReceived += CurrentPort_DataReceived;
				if (!CurrentPort.IsOpen) CurrentPort.Open();
				if (MainCtl.InvokeRequired) // Updating plugin window ports list.
				{
					MainCtl.Invoke(new Action(delegate { MainCtl.UpdatePortsList(null, null); }));
				}
				else { MainCtl.UpdatePortsList(null, null); }
			}
			catch { }
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
					// added try catch to fix crashing when closing VC
					try
					{
						Thread.Sleep(Options.ConcatenationInterval);
						PortMessage = CurrentPort.ReadExisting();
						if (PortMessage != "")
						{
							Host.triggerEvent("Serial.Received", new List<string> { ((SerialPort)sender).PortName, PortMessage });
						}
					}
					catch { }
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
			var AR = new actionResult();
			if (parsedActions.Length < 2)
			{
				AR.setError("Unknown Serial plugin action.");
				return AR;
			}
			string LowerActionName = parsedActions[1].ToLower();
			if ((LowerActionName == "write" || LowerActionName == "upload" || LowerActionName == "close") &&
				(CurrentPort == null || !CurrentPort.IsOpen))
			{
				AR.setError("Selected port doesn't exists\\removed\\closed.");
				return AR;
			}
			try
			{
				switch (LowerActionName)
				{
					case "open": // [0] - PortName pattern, (1) - BaudRate, (2) - DTR
						{
							if (parsedParams.Length < 1)
							{
								AR.setError("Parameters missing.");
								return AR;
							}
							if (!IsValidRegEx(parsedParams[0]))
							{
								AR.setError("Invalid PortName Regex pattern.");
								return AR;
							}
							if (parsedParams.Length < 3) // then select opened port ==============
							{
								foreach (string LongPortName in OpenedPorts.Keys)
								{
									if (new Regex(parsedParams[0]).Match(LongPortName).Success) // port matched by user pattern
									{
										Match M = new Regex(@"COM\d+").Match(LongPortName);
										if (!M.Success)
										{
											AR.setError("Port type must be 'COM'.");
											return AR;
										}
										string ComPortName = M.Value;
										if (CurrentPort != null && CurrentPort.PortName == ComPortName)
										{
											AR.setInfo($"{LongPortName} already selected.");
										}
										else
										{
											SelectPortFromOpenedList(ComPortName);
											AR.setInfo($"'{LongPortName}' selected.");
										}
										return AR;
									}
								}
								AR.setError("Matching port didn't found.");
								return AR;
							}
							// else open new port ================================================
							bool useDtr;
							int baudRate;
							string Info = "";
							if (!int.TryParse(parsedParams[1], out baudRate))
							{
								Info = "Incorrect port Baud rate. Using 9600.\r\n";
								baudRate = 9600;
							}
							if (!bool.TryParse(parsedParams[2], out useDtr))
							{
								Info += "Incorrect \"Use DTR\" parameter. Using false.\r\n";
								useDtr = false;
							}
							using (ManagementObjectCollection Ports = new ManagementObjectSearcher("root\\CIMV2", SerialPortsQuery).Get())
							{
								foreach (ManagementBaseObject Device in Ports)
								{
									if (new Regex(parsedParams[0]).Match(Device["Name"].ToString()).Success) // port matched by user pattern
									{
										try
										{
											Match M = new Regex(@"COM\d+").Match(Device["Name"].ToString());
											if (!M.Success)
											{
												AR.setError("Port type must be 'COM'.");
												return AR;
											}
											string ComPortName = M.Value;
											if (OpenedPorts.ContainsKey(Device["Name"].ToString()))
											{
												AR.setError($"'{Device["Name"]}' already opened.");
												return AR;
											}
											OpenedPorts.Add(Device["Name"].ToString(), new SerialPort
											{
												PortName = ComPortName,
												BaudRate = baudRate,
												DtrEnable = useDtr
											});
											Info += $"'{Device["Name"]}' opened and selected.";
											SelectPortFromOpenedList(ComPortName);
											AR.setInfo(Info);
											return AR;
										}
										finally
										{
											Device.Dispose();
										}
									}
								}
								AR.setError("Matching port didn't found.");
								return AR;
							}
						}
					case "getports": // (0)- Friendly names, (1) - Show only opened
						{
							bool FriendlyNames;
							bool ShowOnlyOpenedPorts;
							if (parsedParams[0] == null || !bool.TryParse(parsedParams[0], out FriendlyNames))
							{
								FriendlyNames = false;
							}
							if (parsedParams[1] == null || !bool.TryParse(parsedParams[1], out ShowOnlyOpenedPorts))
							{
								ShowOnlyOpenedPorts = false;
							}
							var Names = new List<string>();
							if (FriendlyNames)
							{
								using (ManagementObjectCollection Ports = new ManagementObjectSearcher("root\\CIMV2", SerialPortsQuery).Get())
								{
									foreach (ManagementBaseObject Device in Ports)
									{
										try
										{
											if (ShowOnlyOpenedPorts)
											{
												if (OpenedPorts.Count == 0)
												{
													AR.setError("No opened ports yet.");
													return AR;
												}
												if (OpenedPorts.Keys.Contains(Device["Name"]))
												{
													Names.Add($"{Device["Name"]}\r\n");
												}
											}
											else
											{
												Names.Add($"{Device["Name"]}\r\n");
											}
										}
										finally
										{
											Device.Dispose();
										}
									}
								}
							}
							else
							{
								if (ShowOnlyOpenedPorts)
								{
									if (OpenedPorts.Count == 0)
									{
										AR.setError("No opened ports yet.");
										return AR;
									}
									AR.setSuccess(string.Join("\r\n", OpenedPorts.Keys));
									return AR;
								}
								RegistryKey Key = Registry.LocalMachine.OpenSubKey("hardware\\devicemap\\serialcomm");
								if (Key != null)
									Names.AddRange(Key.GetValueNames().Select(str => Key.GetValue(str).ToString()));
							}
							AR.setSuccess(string.Join("\r\n", Names));
							break;
						}
					case "write": // [0] - string to write on port
						{
							if (parsedParams.Length < 1)
							{
								AR.setError("Expected 1 parameter.");
								return AR;
							}
							bool converted = false;
							byte[] ParsedBytes = ParseHexString(parsedParams[0], out converted);
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
					case "upload": // [0] - Arduino Model, [1] - Path to sketch
						{
							if (parsedParams.Length < 2)
							{
								AR.setError("Expected 2 parameters.");
								return AR;
							}
							AR = UploadSketch(parsedParams[0], parsedParams[1]);
							break;
						}
					case "close":
						{
							string DeletedPortName = CurrentPort.PortName;
							OpenedPorts.Remove(OpenedPorts.First(Port => Port.Value == CurrentPort).Key);
							CurrentPort.Close();
							CurrentPort.Dispose();
							CurrentPort = null;
							if (MainCtl.InvokeRequired) // Updating plugin window ports list.
							{
								MainCtl.Invoke(new Action(delegate { MainCtl.UpdatePortsList(null, null); }));
							}
							else { MainCtl.UpdatePortsList(null, null); }
							AR.setInfo($"Port '{DeletedPortName}' closed.");
							break;
						}
					default:
						{
							AR.setError("Unknown Serial plugin action.");
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

		private static bool IsValidRegEx(string pattern)
		{
			if (string.IsNullOrWhiteSpace(pattern)) return false;
			try
			{
				new Regex(pattern);
				return true;
			}
			catch
			{
				return false;
			}
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
					AR.setError("Invalid Arduino model.");
					return AR;
			}
			if (!File.Exists(file))
			{
				AR.setError("Sketch file doesn't exists.");
				return AR;
			}

			string PortToUpload = CurrentPort.PortName;
			int Bauds = CurrentPort.BaudRate;
			bool Dtr = CurrentPort.DtrEnable;
			CurrentPort.Close();
			CurrentPort.Dispose();

			new ArduinoSketchUploader(new ArduinoSketchUploaderOptions
			{
				ArduinoModel = Model,
				FileName = file,
				PortName = PortToUpload
			}).UploadSketch();

			CurrentPort = new SerialPort
			{
				PortName = PortToUpload,
				BaudRate = Bauds,
				DtrEnable = Dtr
			};
			CurrentPort.DataReceived += CurrentPort_DataReceived;
			CurrentPort.Open();
			AR.setInfo("Sketch uploaded.");
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
				SerialPort Port = OpenedPorts.Values.ElementAt(i);
				Port.Close();
				Port.Dispose();
			}
		}
	}
}