using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SeevoConfig.Errors;

namespace SeevoConfig.Communications
{
    public class MulticastClient : IMulticastClient
    {
        public delegate void DataReceivedEvent(DataReceivedEventArgs e);
        public event DataReceivedEvent DataReceived;

        private readonly UdpClient udp;
        private readonly IPAddress groupAddress;
        private CancellationToken? cancellationToken;

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    udp?.Dispose();
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

        public MulticastClient() : this(GetConfiguredClient(), GetMulticastGroupAddress())
        {
        }

        public MulticastClient(UdpClient udp, IPAddress groupAddress)
        {
            this.udp = udp;
            this.groupAddress = groupAddress;

            udp.AllowNatTraversal(true);
            udp.MulticastLoopback = true;
        }

        public void Listen(CancellationToken cancellationToken)
        {
            if (this.cancellationToken != null) { Disconnect(); }

            this.cancellationToken = cancellationToken;
            if (DisconnectOnCancellationRequested()) { return; }

            Logger.LogDebug("Listen");
            udp.JoinMulticastGroup(groupAddress);
            BeginReceive();
        }

        private void BeginReceive()
        {
            udp.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private bool DisconnectOnCancellationRequested()
        {
            if (cancellationToken == null || !cancellationToken.HasValue) { throw new Exception("Invalid CancellationToken."); }
            if (cancellationToken.Value.IsCancellationRequested)
            {
                Disconnect();
            };

            return cancellationToken.Value.IsCancellationRequested;
        }

        private void Disconnect()
        {
            if (udp?.Client == null) { return; }
            Logger.LogDebug("Disconnect");
            cancellationToken = null;
            udp.DropMulticastGroup(groupAddress);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            if (DisconnectOnCancellationRequested()) { return; }
            if (udp?.Client == null) { return; }

            var remoteEndpoint = (IPEndPoint)udp.Client.RemoteEndPoint;
            var receiveBytes = udp.EndReceive(result, ref remoteEndpoint);
            string receiveString = Encoding.UTF8.GetString(receiveBytes);

            DataReceived?.Invoke(new DataReceivedEventArgs(receiveString));
            Logger.LogDebug(string.Concat(receiveString.AsSpan(0, 100), " ..."));

            if (DisconnectOnCancellationRequested()) { return; }
            BeginReceive();
        }

        private static UdpClient GetConfiguredClient()
        {
            var localPort = 11000;
            var endPoint = new IPEndPoint(IPAddress.Any, localPort);
            return new UdpClient(endPoint);
        }

        private static IPAddress GetMulticastGroupAddress()
        {
            var address = "224.168.100.2";
            if (!IPAddress.TryParse(address, out IPAddress ipAddress))
            {
                throw new InvalidOperationException("Invalid Multicast Group Address.");
            }
            return ipAddress;
        }
    }
}
