using FanControl.Plugins;
using Microsoft.Win32;
using System.Globalization;

namespace FanControl.HWInfoReg
{
    class HWInfoRegPluginSensor : IPluginSensor
    {
        public HWInfoRegPluginSensor(string name, string id, string registryValueName)
        {
            Name = name;
            Id = id;
            RegistryValueName = registryValueName;
        }

        public string Name { get; }

        public float? Value { get; private set; }

        public string Id { get; }

        public string RegistryValueName { get; set; }
        public void Update()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\HWiNFO64\\VSB"))
            {
                if (key != null)
                {
                    if (key.GetValue(RegistryValueName) != null)
                    {
                        Value = float.Parse(key.GetValue(RegistryValueName).ToString(), CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        Value = -999;
                    }
                }
            }
        }
    }
}
