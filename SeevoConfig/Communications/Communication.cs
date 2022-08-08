using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using SeevoConfig.Devices;
using SeevoConfig.Errors;

namespace SeevoConfig.Communications
{
    public class Communication : IDisposable
    {
        public delegate void SeevoConfigReceivedEvent(SeevoConfigReceivedEventArgs e);
        public event SeevoConfigReceivedEvent SeevoConfigReceived;
        public readonly IMulticastClient multicastClient;

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    multicastClient?.Dispose();
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

        public Communication()
        {
            multicastClient = new MulticastClient();
            multicastClient.DataReceived += MulticastClient_DataReceived;
        }

        public void LoadExampleData()
        {
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

                SeevoConfigReceived?.Invoke(new SeevoConfigReceivedEventArgs(device));
            }
        }

        public void Discovery()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            multicastClient.Listen(cts.Token);
        }

        private void MulticastClient_DataReceived(DataReceivedEventArgs e)
        {
            var device = JsonSerializer.Deserialize<SeevoModel>(e.Data);
            SeevoConfigReceived?.Invoke(new SeevoConfigReceivedEventArgs(device));
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
                    Logger.LogError(ex);
                }
            }
        }

        public void SendToDevice(SeevoModel model)
        {
            //send to device
        }
    }
}
