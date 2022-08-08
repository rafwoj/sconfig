using System;
using SeevoConfig.Devices;

namespace SeevoConfig.Communications
{
    public class SeevoConfigReceivedEventArgs : EventArgs

    {
        public SeevoModel DeviceConfig { get; private set; }

        public SeevoConfigReceivedEventArgs(SeevoModel config)
        {
            DeviceConfig = config;
        }
    }
}
