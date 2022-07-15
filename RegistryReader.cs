using Microsoft.Win32;
using System;

namespace overwolf.plugins
{
    public class RegistryKeyReadResult
    {
        public string status { get; set; }
        public bool exists { get; set; }
        public object value { get; set; }
        public string error { get; set; }
    }

    public class RegistryReader
    {
        public RegistryReader()
        {

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Checked")]
        public RegistryKeyReadResult getValueFromLocalMachineSync(string subKeyName, string keyName)
        {
            try
            {
                RegistryKey localKey;
                if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                    return new RegistryKeyReadResult()
                    {
                        status = "error",
                        error = "The OS isn't windows"
                    };
                if (Environment.Is64BitOperatingSystem)
                    localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                else
                    localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                object value = localKey.OpenSubKey(subKeyName).GetValue(keyName);
                return new RegistryKeyReadResult()
                {
                    status = "success",
                    exists = value != null,
                    value = value
                };
            } catch (Exception ex)
            {
                return new RegistryKeyReadResult()
                {
                    status = "error",
                    error = ex.Message
                };
            }
        }

        public void getValueFromLocalMachine(string subKeyName, string keyName, Action<object> callback)
        {
            callback(getValueFromLocalMachineSync(subKeyName, keyName));
        }
    }
}
