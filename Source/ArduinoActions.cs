using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ArduinoController;
using ArduinoController.Hardware;
using ArduinoController.SerialProtocol;
using PluginInterface;

namespace Serial {
    internal static class ArduinoActions {
        internal static actionResult DoDriverAction(string[] actionNameArray, string[] actionParameters) {
            var ar = new actionResult();

            if (!Plugin.SelectedPort.IsAlive()) {
                ar.setError("Current port isn't open.");
                return ar;
            }
            if (actionParameters.Length < 1) {
                ar.setError("Arduino model not specified.");
                return ar;
            }
            ArduinoModel arduinoModel;
            if (!Enum.TryParse(actionParameters[0], true, out arduinoModel)) {
                ar.setError("Invalid Arduino model.");
                return ar;
            }

            switch (actionNameArray[2].ToUpper()) {
                case "UPLOAD": { // [Arduino Model], [Path to sketch]
                    // TODO connect avrdude.exe to compile & upload sketches without Arduino IDE.
                    if (actionParameters.Length < 2) {
                        ar.setError("2 parameters expected.");
                        return ar;
                    }
                    // Uploading sketch from another thread,
                    // Result is returned by Serial.Upload.{Status} event.
                    Task.Run(() => UploadSketch(arduinoModel, actionParameters[1], Plugin.SelectedPort.PortName));
                    ar.setInfo("Uploading...");
                    return ar;
                }
#if DEBUG
                // BETA ACTIONS, BUT MOST OF THEM ALREADY TESTED ON MY PC W/ ARDUINO UNO.
                case "MARIO": { // [Arduino model], [Buzzer pin]
                    if (actionParameters.Length < 2) {
                        ar.setError("2 parameters expected.");
                        return ar;
                    }
                    byte pin;
                    if (!byte.TryParse(actionParameters[1], out pin)) {
                        ar.setError("Invalid buzzer pin.");
                        return ar;
                    }

                    Task.Run(
                        delegate {
                            using (var driver = new ArduinoDriver(arduinoModel, Plugin.SelectedPort.PortName, true)) {
                                for (var i = 0; i < melody.Length; i++) {
                                    int noteDuration = 1000 / tempo[i];
                                    driver.Send(new ToneRequest(pin, (ushort) melody[i], (uint) noteDuration));
                                    Thread.Sleep((int) (noteDuration * 1.40));
                                    driver.Send(new NoToneRequest(pin));
                                }
                            }
                            Plugin.HostInstance.triggerEvent(
                                "Serial.Micro.Mario.Success",
                                new List<string>()
                            );
                        }
                    );
                    ar.setInfo("Uploading...");
                    break;
                }

                case "TONE": { // [Arduino model], [Buzzer pin], [Tone], [Duration in ms]
                    if (actionParameters.Length < 4) {
                        ar.setError("4 parameters expected.");
                        return ar;
                    }
                    byte pin;
                    if (!byte.TryParse(actionParameters[1], out pin)) {
                        ar.setError("Invalid pin.");
                        return ar;
                    }
                    ushort tone;
                    if (!ushort.TryParse(actionParameters[2], out tone)) {
                        ar.setError("Invalid tone.");
                        return ar;
                    }
                    uint duration;
                    if (!uint.TryParse(actionParameters[2], out duration)) {
                        ar.setError("Invalid duration.");
                        return ar;
                    }

                    Task.Run(
                        delegate {
                            using (var driver = new ArduinoDriver(arduinoModel, Plugin.SelectedPort.PortName, true)) {
                                driver.Send(new ToneRequest(pin, tone, duration));
                                Thread.Sleep((int) (duration * 1.40));
                                driver.Send(new NoToneRequest(pin));
                            }
                        }
                    );
                    ar.setInfo("OK.");
                    break;
                }

                case "ANALOGREAD": { // [Arduino model], [Pin to read]
                    if (actionParameters.Length < 2) {
                        ar.setError("2 parameters expected.");
                        return ar;
                    }
                    byte pin;
                    if (!byte.TryParse(actionParameters[1], out pin)) {
                        ar.setError("Invalid pin.");
                        return ar;
                    }
                    using (var driver = new ArduinoDriver(arduinoModel, Plugin.SelectedPort.PortName, true)) {
                        AnalogReadResponse response = driver.Send(new AnalogReadRequest(pin));
                        ar.setSuccess(response.PinValue.ToString());
                    }
                    break;
                }

                case "DIGITALREAD": { // [Arduino model], [Pin to read]
                    if (actionParameters.Length < 2) {
                        ar.setError("2 parameters expected.");
                        return ar;
                    }
                    byte pin;
                    if (!byte.TryParse(actionParameters[1], out pin)) {
                        ar.setError("Invalid pin.");
                        return ar;
                    }
                    using (var driver = new ArduinoDriver(arduinoModel, Plugin.SelectedPort.PortName, true)) {
                        DigitalReadResponse response = driver.Send(new DigitalReadRequest(pin));
                        ar.setSuccess(response.PinValue.ToString("G").ToUpper());
                    }
                    break;
                }

                // all other actions need at least 3 parameters.
                default: {
                    if (actionParameters.Length < 3) {
                        ar.setError("3 parameters expected.");
                        return ar;
                    }
                    byte pin;
                    if (!byte.TryParse(actionParameters[1], out pin)) {
                        ar.setError("Invalid pin.");
                        return ar;
                    }
                    switch (actionNameArray[2].ToUpper()) {
                        case "ANALOGWRITE": { // [Arduino model], [Pin to write], [Value to write]
                            byte val;
                            if (!byte.TryParse(actionParameters[2], out val)) {
                                ar.setError("Invalid pin value.");
                                return ar;
                            }
                            using (var driver = new ArduinoDriver(arduinoModel, Plugin.SelectedPort.PortName, true)) {
                                AnalogWriteResponse response = driver.Send(new AnalogWriteRequest(pin, val));
                                ar.setInfo($"OK. pin {response.PinWritten}, value: {val}");
                            }
                            break;
                        }

                        case "DIGITALWRITE": { // [Arduino model], [Pin to write], [Value to write]
                            byte val;
                            if (!byte.TryParse(actionParameters[2], out val)) {
                                ar.setError("Invalid pin value.");
                                return ar;
                            }
                            using (var driver = new ArduinoDriver(arduinoModel, Plugin.SelectedPort.PortName, true)) {
                                DigitalWriteReponse response = driver.Send(new DigitalWriteRequest(pin, (DigitalValue) val));
                                ar.setInfo($"OK. pin {response.PinWritten}, value: {val}");
                            }
                            break;
                        }

                        case "PINMODE": { // [Arduino model], [Pin to set], [Pin mode]
                            PinMode pinMode;
                            switch (actionParameters[2].ToUpper()) {
                                case "IN":
                                    pinMode = PinMode.Input;
                                    break;
                                case "INPULL":
                                    pinMode = PinMode.InputPullup;
                                    break;
                                case "OUT":
                                    pinMode = PinMode.Output;
                                    break;
                                default:
                                    ar.setError("Invalid pin mode.");
                                    return ar;
                            }
                            using (var driver = new ArduinoDriver(arduinoModel, Plugin.SelectedPort.PortName, true)) {
                                PinModeResponse response = driver.Send(new PinModeRequest(pin, pinMode));
                                ar.setInfo($"OK. pin {response.Pin}: {response.Mode:G}.");
                            }
                            break;
                        }
                    }
                    break;
                }
#endif
            }
            return ar;
        }

        private static void UploadSketch(ArduinoModel arduinoModel, string hexFilePath, string comPortName) {
            if (!File.Exists(hexFilePath)) {
                Plugin.HostInstance.triggerEvent(
                    "Serial.Upload.Error",
                    new List<string> { ".hex sketch file doesn't exists." }
                );
                return;
            }

            // upload sketch
            try {
                new ArduinoSketchUploader(
                    new ArduinoSketchUploaderOptions {
                        ArduinoModel = arduinoModel,
                        FileName     = hexFilePath,
                        PortName     = comPortName
                    }
                ).UploadSketch();
            }
            catch (Exception ex) {
                Plugin.HostInstance.triggerEvent(
                    "Serial.Upload.Error",
                    new List<string> { ex.ToString() }
                );
                return;
            }

            Plugin.HostInstance.triggerEvent(
                "Serial.Upload.Success",
                new List<string>()
            );
        }

        #region Mario melody properties

        private const int d3  = 147;
        private const int ds3 = 156;
        private const int e3  = 165;
        private const int f3  = 175;
        private const int g3  = 196;
        private const int gs3 = 208;
        private const int a3  = 220;
        private const int as3 = 233;
        private const int b3  = 247;
        private const int c4  = 262;
        private const int cs4 = 277;
        private const int d4  = 294;
        private const int ds4 = 311;
        private const int f4  = 349;
        private const int fs4 = 370;
        private const int gs4 = 415;
        private const int a4  = 440;
        private const int as4 = 466;
        private const int e5  = 523;

        private static readonly int[] melody = {
            c4, e5, a3, a4, as3, as4, 0, 0, c4, e5, a3, a4, as3, as4, 0, 0, f3, f4, d3, d4, ds3, ds4, 0, 0, f3, f4, d3, d4, ds3, ds4, 0,
            0, ds4, d4, cs4, c4, ds4, d4, gs3, g3, cs4, c4, fs4, f4, e3, as4, a4, gs4, ds4, b3, as3, a3, gs3, 0, 0, 0
        };

        private static readonly int[] tempo = {
            12, 12, 12, 12, 12, 12, 6, 3, 12, 12, 12, 12, 12, 12, 6, 3, 12, 12, 12, 12, 12, 12, 6, 3, 12, 12, 12, 12, 12, 12, 6,
            6, 18, 18, 18, 6, 6, 6, 6, 6, 6, 18, 18, 18, 18, 18, 18, 10, 10, 10, 10, 10, 10, 3, 3, 3
        };

        #endregion
    }
}