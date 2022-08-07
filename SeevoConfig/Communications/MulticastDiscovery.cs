using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SeevoConfig.Errors;

namespace SeevoConfig.Communications
{
    public class MulticastDiscovery
    {
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
            byte[] bytes = new Byte[100];
            IPEndPoint groupEP = new IPEndPoint(mcastAddress, mcastPort);
            EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

            try
            {
                while (!done)
                {
                    Logger.LogDebug("Waiting for multicast packets.......");
                    Logger.LogDebug("Enter ^C to terminate.");

                    mcastSocket.ReceiveFrom(bytes, ref remoteEP);

                    var msg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    Logger.LogDebug($"Received broadcast from {groupEP} :\n {msg}\n");
                }

                mcastSocket.Close();
            }

            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

    }
}
