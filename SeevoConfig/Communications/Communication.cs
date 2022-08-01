using System;
using System.Collections.Generic;
using System.Text.Json;
using SeevoConfig.Devices;
using SeevoConfig.Other;

namespace SeevoConfig.Communications
{
    public class Communication : IDisposable
    {
        public delegate void DeviceJsonReceivedEventHandler(DeviceJsonReceivedEventArgs e);
        public event DeviceJsonReceivedEventHandler DeviceJsonReceived;

        #region IDisposable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public void LoadExampleData()
        {
            CancelDiscovering();

            var rnd = new Random();

            for (int i = 0; i < 9; i++)
            {
                var device = new SeevoModel
                {
                    Id = $"Device {i}",
                    IP = $"192.168.0.{i + 10}",
                    Port = 12345,
                    Mac = rnd.Next(10000, 99999).ToString(),
                    AvailableEvents = new List<string> { "none", "on", "off", "click", "group1", "group2", "group3" }
                };

                DeviceJsonReceived.Invoke(new DeviceJsonReceivedEventArgs(device));
            }
        }

        public void Discovery()
        {
            // open port/communication here

            var json = "";
            var device = JsonSerializer.Deserialize<SeevoModel>(json);
            DeviceJsonReceived.Invoke(new DeviceJsonReceivedEventArgs(device));
        }

        public void CancelDiscovering()
        {
            // close port/communication here
        }

        public void SendToDevices(IList<SeevoModel> list)
        {
            foreach (var device in list)
            {
                try
                {
                    SendToDevice(device);
                }
                catch (Exception ex)
                {
                    Logger.LogAndDisplayError(ex);
                }
            }
        }

        public void SendToDevice(SeevoModel model)
        {
            CancelDiscovering();
            //send to device
        }

    }
}
