using System;
using Code.Infrastructure;
using Code.NetworkMessages;
using Code.Services.Contracts;
using Mirror;

namespace Code.Views
{
    public class NetworkManagerView : NetworkManager
    {
        public event EventHandler ClientConnected;

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            NetworkServer.RegisterHandler<CreatePlayerMessage>(CreatePlayerHandler);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            DiContainer.Instance.Resolve<IViewService>().Create<CameraView>();
            DiContainer.Instance.Resolve<IUserInputService>().ChangeCursorLockState();
            
            ClientConnected?.Invoke(this, EventArgs.Empty);
        }
        
        private void CreatePlayerHandler(NetworkConnectionToClient connection, CreatePlayerMessage message)
        {
            var playerView = DiContainer.Instance.Resolve<IViewService>().Create<PlayerView>(false);
            NetworkServer.AddPlayerForConnection(connection, playerView.gameObject);
        }
    }
}