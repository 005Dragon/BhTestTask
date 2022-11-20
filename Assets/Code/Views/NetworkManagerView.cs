using System;
using Code.Controllers;
using Code.Data;
using Code.Infrastructure;
using Code.NetworkMessages;
using Code.Services.Contracts;
using Mirror;
using UnityEngine.SceneManagement;

namespace Code.Views
{
    public class NetworkManagerView : NetworkManager
    {
        public event EventHandler ClientConnected;

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            NetworkServer.RegisterHandler<CreatePlayerMessage>(CreatePlayerHandler);
            NetworkServer.RegisterHandler<WinnerPlayerMessage>(WinnerPlayerHandler);
            NetworkServer.RegisterHandler<ReloadGameMessage>(ReloadGameHandler);
            
            DiContainerRoot.Instance.Resolve<IMapViewFactory>().Create();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            
            NetworkClient.RegisterHandler<ClearUpdatingMessage>(ClearUpdatingHandler);
            
            InitializePlayer();
            
            DiContainerRoot.Instance.Resolve<IUserInputService>().ChangeCursorLockState();
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);
            
            DiContainerRoot.Instance.Resolve<IMapViewFactory>().Create();
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();
            
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            DiContainerRoot.Instance.Resolve<IViewService>().Create<CameraView>();
            DiContainerRoot.Instance.Resolve<IViewService>().Create<PlayerUiView>();

            ClientConnected?.Invoke(this, EventArgs.Empty);
        }

        private void CreateReloadSceneCooldownController()
        {
            var cooldownController = new CooldownController
            {
                Cooldown = DiContainerRoot.Instance.Resolve<GameData>().ReloadSceneCooldown
            };

            cooldownController.CooldownExpired += (_, _) =>
            {
                NetworkServer.SendToAll(new ClearUpdatingMessage());
                NetworkClient.Send(new ReloadGameMessage());
            };
            
            DiContainerRoot.Instance.Resolve<IUpdateService>().AddToUpdate(cooldownController);
            
            cooldownController.Start();
        }

        private void WinnerPlayerHandler(NetworkConnectionToClient connection, WinnerPlayerMessage message)
        {
            var playerWinnerUi = DiContainerRoot.Instance.Resolve<IViewService>().Create<PlayerWinnerUi>();
            NetworkServer.Spawn(playerWinnerUi.gameObject);
            playerWinnerUi.PlayerName = message.PlayerName;
            CreateReloadSceneCooldownController();
        }

        private static void CreatePlayerHandler(NetworkConnectionToClient connection, CreatePlayerMessage message)
        {
            SpawnPointView spawnPoint = DiContainerRoot.Instance.Resolve<MapView>().GetRandomSpawnPoint();
            var playerView = DiContainerRoot.Instance.Resolve<IViewService>().Create<PlayerView>(false);
            playerView.transform.position = spawnPoint.transform.position;
            
            NetworkServer.AddPlayerForConnection(connection, playerView.gameObject);
        }

        private static void ClearUpdatingHandler(ClearUpdatingMessage message)
        {
            DiContainerRoot.Instance.Resolve<IUpdateService>().Clear();
        }

        private void ReloadGameHandler(NetworkConnectionToClient connection, ReloadGameMessage message)
        {
            foreach (NetworkIdentity networkIdentity in FindObjectsOfType<NetworkIdentity>())
            {
                NetworkServer.Destroy(networkIdentity.gameObject);
            }

            ServerChangeScene(SceneManager.GetActiveScene().name);
        }
    }
}