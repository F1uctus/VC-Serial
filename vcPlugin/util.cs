using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Serial
{
    //  ====    Sample port names for different types of devices.    ====

    //	[COM1 nothing attached]	   Communications Port (COM1) : Communications Port

    //	[NANO] 		   Prolific USB-to-Serial Comm Port (COM6) : Prolific USB-to-Serial Comm Port
    //	[NodeMCU 0.9]  USB-SERIAL CH340 (COM5) : USB-SERIAL CH340
    //	[Wemos]        USB-SERIAL CH340 (COM5) : USB-SERIAL CH340
    //	[Mega2560]     Arduino Mega 2560 (COM7) : Arduino Mega 2560

    class util
    {
        //public bool AutoOpenArduinoPort(int bauds, bool dtr)
        //{
        //    using (ManagementObjectCollection Ports = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\"").Get())
        //    {
        //        myHost.log("# com ports: " + Ports.Count);
        //        foreach (ManagementBaseObject Dev in Ports)
        //        {
        //            try
        //            {
        //                String strPortInfo = Dev["Name"] + " : " + Dev["Description"];
        //                myHost.log("found port: " + strPortInfo);

        //                if (!strPortInfo.Contains("Communications Port"))
        //                {
        //                    OpenedPorts.Add(new SerialPort
        //                    {
        //                        PortName = ArduinoPort = Dev["DeviceID"].ToString(),
        //                        BaudRate = ArduinoBauds = bauds,
        //                        DtrEnable = ArduinoDTR = dtr
        //                    });
        //                    MainCtl.OpenedPortsNames.Add(ArduinoPort);
        //                    OpenPortFromList(ArduinoPort);
        //                    return true;
        //                }
        //            }
        //            catch (Exception err) { myHost.log("Serial: Unable to open port: " + Dev["Name"]); }
        //            finally
        //            {
        //                Dev.Dispose();
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}
