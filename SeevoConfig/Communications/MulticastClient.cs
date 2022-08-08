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
            udp.JoinMulticastGroup(groupAddress);
        }

        public void Listen(CancellationToken cancellationToken)
        {
            if (this.cancellationToken != null && !IsCancellationRequested())
            {
                return;
            }

            this.cancellationToken = cancellationToken;
            cancellationToken.Register(() => Logger.LogDebug("Timeout"));

            Logger.LogDebug("Listen");
            BeginReceive();
        }

        private void BeginReceive()
        {
            udp.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            if (IsCancellationRequested()) { return; }
            if (udp?.Client == null) { return; }

            var remoteEndpoint = (IPEndPoint)udp.Client.RemoteEndPoint;
            var receiveBytes = udp.EndReceive(result, ref remoteEndpoint);
            string receiveString = Encoding.UTF8.GetString(receiveBytes);

            DataReceived?.Invoke(new DataReceivedEventArgs(receiveString));
            Logger.LogDebug(string.Concat(receiveString.AsSpan(0, 110), " ..."));

            if (IsCancellationRequested()) { return; }
            BeginReceive();
        }

        private bool IsCancellationRequested()
        {
            if (cancellationToken == null || !cancellationToken.HasValue) { throw new Exception("Invalid CancellationToken."); }
            if (cancellationToken.Value.IsCancellationRequested)
            {
                return true;
            };

            return false;

        }

        private static UdpClient GetConfiguredClient()
        {
            var localPort = 11000;
            var endPoint = new IPEndPoint(IPAddress.Any, localPort);
            var udp = new UdpClient(endPoint)
            {
                MulticastLoopback = true
            };

            udp.AllowNatTraversal(false);
            return udp;
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
