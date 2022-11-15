using System;
using Code.Infrastructure;
using Code.NetworkMessages;
using Code.Services.Contracts;
using Code.Views;
using Mirror;

namespace Code.Controllers
{
    public class NetworkManagerController
    {
        private readonly NetworkManagerView _view;
        private readonly DiContainer _diContainer;

        public NetworkManagerController(NetworkManagerView view, DiContainer diContainer)
        {
            _view = view;
            _diContainer = diContainer;
            
            _view.ClientConnected += ViewOnClientConnected;
            _view.CreatingPlayer += ViewOnCreatingPlayer;
        }

        private void ViewOnClientConnected(object sender, EventArgs eventArgs)
        {
            NetworkClient.Send(new CreatePlayerMessage());
        }

        private void ViewOnCreatingPlayer(object sender, NetworkMessageEventArgs<CreatePlayerMessage> eventArgs)
        {
            PlayerController playerController = _diContainer.Resolve<IFactory<PlayerController>>().Create();
            NetworkServer.AddPlayerForConnection(eventArgs.NetworkConnectionToClient, playerController.View.gameObject);
        }
    }
}