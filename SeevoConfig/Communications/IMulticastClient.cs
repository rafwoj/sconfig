using System;
using System.Threading;

namespace SeevoConfig.Communications
{
    public interface IMulticastClient : IDisposable
    {
        event MulticastClient.DataReceivedEvent DataReceived;

        void Listen(CancellationToken cancellationToken);
    }
}