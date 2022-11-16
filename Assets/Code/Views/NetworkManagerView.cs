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

        private IViewService _viewService;
        private IUserInputService _userInputService;
        private IMapViewFactory _mapViewFactory;

        public override void Awake()
        {
            base.Awake();

            _viewService = DiContainer.Instance.Resolve<IViewService>();
            _userInputService = DiContainer.Instance.Resolve<IUserInputService>();
            _mapViewFactory = DiContainer.Instance.Resolve<IMapViewFactory>();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            _mapViewFactory.Create();
            
            NetworkServer.RegisterHandler<CreatePlayerMessage>(CreatePlayerHandler);
        }
        
        public override void OnClientConnect()
        {
            base.OnClientConnect();
            
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            _viewService.Create<CameraView>();
            _userInputService.ChangeCursorLockState();
            
            ClientConnected?.Invoke(this, EventArgs.Empty);
        }

        private void CreatePlayerHandler(NetworkConnectionToClient connection, CreatePlayerMessage message)
        {
            SpawnPointView spawnPoint = DiContainer.Instance.Resolve<MapView>().GetRandomSpawnPoint();
            var playerView = _viewService.Create<PlayerView>(false);
            playerView.transform.position = spawnPoint.transform.position;
            
            NetworkServer.AddPlayerForConnection(connection, playerView.gameObject);
        }
    }
}