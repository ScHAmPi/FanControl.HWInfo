using FanControl.Plugins;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FanControl.HWInfoReg
{
    public class HWInfoPluginReg : IPlugin
    {

        string GenerateId(string sensorName, string sensorLabel)
        {
            return sensorName + sensorLabel;
        }
        string IPlugin.Name => "HWInfoReg";

        void IPlugin.Close()
        {
        }

        void IPlugin.Initialize()
        {
        }

        void IPlugin.Load(IPluginSensorsContainer _container)
        {
            Dictionary<int, Sensor> sensors = new Dictionary<int, Sensor>();

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\HWiNFO64\\VSB"))
            {
                if (key != null)
                {

                    foreach (string valueName in key.GetValueNames())
                    {
                        string resultString = Regex.Match(valueName, @"\d+").Value;
                        int number = int.Parse(resultString);

                        if (!sensors.ContainsKey(number))
                        {
                            sensors[number] = new Sensor();
                        }

                        if (valueName.StartsWith("Sensor"))
                        {
                            sensors[number].SensorName = key.GetValue(valueName).ToString();
                        }
                        else if (valueName.StartsWith("Label"))
                        {
                            sensors[number].SensorLabel = key.GetValue(valueName).ToString();
                        }
                        else if (valueName.StartsWith("ValueRaw"))
                        {
                            sensors[number].RegistryValueName = valueName;
                        }
                        else if (valueName.StartsWith("Value"))
                        {
                            sensors[number].Unit = Regex.Match(key.GetValue(valueName).ToString(), @"[^0-9. ]+").Value;
                        }
                    }
                }
            }
            foreach (int sensornumber in sensors.Keys)
            {
                if (sensors[sensornumber].Unit.Equals("°C"))
                {
                    _container.TempSensors.Add(new HWInfoRegPluginSensor(sensors[sensornumber].SensorName + sensors[sensornumber].SensorLabel, GenerateId(sensors[sensornumber].SensorName, sensors[sensornumber].SensorLabel), sensors[sensornumber].RegistryValueName));
                }
                else if (sensors[sensornumber].Unit.Equals("RPM"))
                {
                    _container.FanSensors.Add(new HWInfoRegPluginSensor(sensors[sensornumber].SensorName + sensors[sensornumber].SensorLabel, GenerateId(sensors[sensornumber].SensorName, sensors[sensornumber].SensorLabel), sensors[sensornumber].RegistryValueName));
                }
            }
        }
    }

    class Sensor
    {
        public string SensorName { get; set; }
        public string SensorLabel { get; set; }
        public string RegistryValueName { get; set; }
        public string Unit { get; set; }

        public Sensor()
        {
        }

        public override string ToString()
        {
            return SensorName + SensorLabel + " -> " + RegistryValueName + "(" + Unit + ")";
        }

    }
}
