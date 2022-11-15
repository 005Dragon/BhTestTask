using System;
using Code.NetworkMessages;
using Mirror;

namespace Code.Views
{
    public class NetworkManagerView : NetworkManager
    {
        public event EventHandler<NetworkMessageEventArgs<CreatePlayerMessage>> CreatingPlayer;
        public event EventHandler ClientConnected;

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            NetworkServer.RegisterHandler<CreatePlayerMessage>(CreatePlayerHandler);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            
            ClientConnected?.Invoke(this, EventArgs.Empty);
        }

        private void CreatePlayerHandler(NetworkConnectionToClient networkConnection, CreatePlayerMessage message)
        {
            CreatingPlayer?.Invoke(this, new NetworkMessageEventArgs<CreatePlayerMessage>(networkConnection, message));
        }
    }
}