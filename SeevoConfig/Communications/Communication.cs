using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SeevoConfig.Devices;
using SeevoConfig.Errors;

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
            StartMulticast();
            // Receive broadcast messages.
            ReceiveBroadcastMessages();


/*            var json = "";
            var device = JsonSerializer.Deserialize<SeevoModel>(json);
            DeviceJsonReceived.Invoke(new DeviceJsonReceivedEventArgs(device));*/
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
                    Logger.LogError(ex);
                }
            }
        }

        public void SendToDevice(SeevoModel model)
        {
            CancelDiscovering();
            //send to device
        }

#region multicast_receive
        private static IPAddress mcastAddress;
        private static int mcastPort;
        private static Socket mcastSocket;
        private static MulticastOption mcastOption;

        public void StartMulticast()
        {

            try
            {
                mcastAddress = IPAddress.Parse("224.168.100.2");
                mcastPort = 11000;

                mcastSocket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Dgram,
                                         ProtocolType.Udp);

                IPAddress localIPAddr = IPAddress.Parse("192.168.1.109");
                Logger.LogDebug("MULTICAST START ......");


                //IPAddress localIP = IPAddress.Any;
                EndPoint localEP = (EndPoint)new IPEndPoint(localIPAddr, mcastPort);

                mcastSocket.Bind(localEP);


                // Define a MulticastOption object specifying the multicast group
                // address and the local IPAddress.
                // The multicast group address is the same as the address used by the server.
                mcastOption = new MulticastOption(mcastAddress, localIPAddr);

                mcastSocket.SetSocketOption(SocketOptionLevel.IP,
                                            SocketOptionName.AddMembership,
                                            mcastOption);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public void ReceiveBroadcastMessages()
        {
            bool done = false;
            byte[] bytes = new Byte[500];
            IPEndPoint groupEP = new IPEndPoint(mcastAddress, mcastPort);
            EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

            try
            {
                //while (!done)
                //{
                    Logger.LogDebug("Waiting for multicast packets.......");
                    Logger.LogDebug("Enter ^C to terminate.");

                    mcastSocket.ReceiveFrom(bytes, ref remoteEP);

                    var msg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

                    Logger.LogDebug($"Received broadcast from {groupEP} :\n {msg}\n");
                    done = true;
                //}

                mcastSocket.Close();

                var device = JsonSerializer.Deserialize<SeevoModel>(msg);
                DeviceJsonReceived.Invoke(new DeviceJsonReceivedEventArgs(device));

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

        }

#endregion
    }
}
