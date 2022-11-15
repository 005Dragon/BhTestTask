using System;
using Mirror;

namespace Code.NetworkMessages
{
    public class NetworkMessageEventArgs<TMessage> : EventArgs
        where TMessage : NetworkMessage
    {
        public NetworkConnectionToClient NetworkConnectionToClient { get; }
        public TMessage Message { get; }

        public NetworkMessageEventArgs(NetworkConnectionToClient networkConnectionToClient, TMessage message)
        {
            NetworkConnectionToClient = networkConnectionToClient;
            Message = message;
        }
    }
}