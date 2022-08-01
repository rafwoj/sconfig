using System;
using SeevoConfig.Devices;

namespace SeevoConfig.Communications
{
    public class DeviceJsonReceivedEventArgs : EventArgs

    {
        public SeevoModel DeviceConfig { get; private set; }

        public DeviceJsonReceivedEventArgs(SeevoModel Config)
        {
            this.DeviceConfig = Config;
        }
    }
}
