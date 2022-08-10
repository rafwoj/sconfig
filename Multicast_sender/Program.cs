using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

// This sender example must be used in conjunction with the listener program.
// You must run this program as follows:
// Open a console window and run the listener from the command line.
// In another console window run the sender. In both cases you must specify
// the local IPAddress to use. To obtain this address,  run the ipconfig command
// from the command line.
//
namespace Mssc.TransportProtocols.Utilities
{
    class TestMulticastOption
    {

        static IPAddress mcastAddress;
        static int mcastPort;
        static Socket mcastSocket;


        static int thread_count = 3; //1...99 --->  ilość symulowanych urządzeń
        static int delay_ms_between_json = 0; // opóźnienie w ms pomiedzy wysyłaniem kolejnych pakietów JSON
        static int json_addidtional_chars = 100; //długość json w znakach
        static int json_repeat_counter = 10; // ilosc powtórzeń wysyłki


        static void JoinMulticastGroup()
        {
            try
            {
                // Create a multicast socket.
                mcastSocket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Dgram,
                                         ProtocolType.Udp);

                // Get the local IP address used by the listener and the sender to
                // exchange multicast messages.
                IPAddress localIPAddr = IPAddress.Any;

                // Create an IPEndPoint object.
                IPEndPoint IPlocal = new IPEndPoint(localIPAddr, 0);

                // Bind this endpoint to the multicast socket.
                mcastSocket.Bind(IPlocal);

                // Define a MulticastOption object specifying the multicast group
                // address and the local IP address.
                // The multicast group address is the same as the address used by the listener.
                MulticastOption mcastOption;
                mcastOption = new MulticastOption(mcastAddress, localIPAddr);

                mcastSocket.SetSocketOption(SocketOptionLevel.IP,
                                            SocketOptionName.AddMembership,
                                            mcastOption);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }
        }

        static void BroadcastMessage(string message, int count)
        {
            IPEndPoint endPoint;

            try
            {
                //Send multicast packets to the listener.
                endPoint = new IPEndPoint(mcastAddress, mcastPort);
                for (int i = 0; i < count; i++)
                {
                    mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes(message), endPoint);
                    Console.WriteLine("Multicast #{0} thread: #{2} data sent.....   [{1}]", i, message, Thread.CurrentThread.ManagedThreadId);

                    Thread.Sleep(delay_ms_between_json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }

        }

        static void MulticastSocketClose()
        {
            mcastSocket.Close();
        }

        public static void ThreadProc()
        {

            string s = "\"device-port\": 11000 , \"device-selected-event\": \"off\" ,\"device-available-events\": [\"on\", \"off\", \"short-click\", \"long-click\", \"very-long-click\", \"group01\", \"group02\", \"group03\", \"group04\", \"group01\"]";

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("{");
            sb.AppendFormat(" \"thread-id\": \"{0}\", ", Thread.CurrentThread.ManagedThreadId);
            sb.AppendFormat(" \"device-id\": \"module_{0}\", ", Thread.CurrentThread.ManagedThreadId);
            sb.AppendFormat(" \"device-ip\": \"192.168.12.{0}\", ", Thread.CurrentThread.ManagedThreadId);
            sb.AppendFormat(" \"device-mac\": \"AA:BB:CC:01:02:{0}\", ", Thread.CurrentThread.ManagedThreadId);
            sb.Append(s);
            sb.AppendLine("}");

            BroadcastMessage(sb.ToString(), json_repeat_counter);
        }



        static void Main(string[] args)
        {
            //------ PARAMETRY -----------
            
            thread_count = 10;
            delay_ms_between_json = 500;
            json_addidtional_chars = 100;
            json_repeat_counter = 3;

            //------ PARAMETRY -----------

            mcastAddress = IPAddress.Parse("224.168.100.2");
            mcastPort = 11000;
            JoinMulticastGroup();


            Thread[] t = new Thread[thread_count];

            Console.WriteLine("START ...");

            for ( int i = 0; i< thread_count; i++)
            {
                t[i] = new Thread(new ThreadStart(ThreadProc));
                t[i].Start();
            }

            for (int i = 0; i < thread_count; i++) t[i].Join();


            MulticastSocketClose();

            Console.WriteLine("End ... Press ENTER key ...");
            Console.ReadLine();

        }
    }
}