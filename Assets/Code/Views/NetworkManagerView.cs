using Code.Controllers;
using Code.Data;
using Code.Infrastructure;
using Code.NetworkMessages;
using Code.Services.Contracts;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Views
{
    public class NetworkManagerView : NetworkRoomManager
    {
        private readonly PlayerIdentityController _playerIdentityController = new();

        public override void OnStartServer()
        {
            base.OnStartServer();

            NetworkServer.RegisterHandler<WinnerPlayerMessage>(WinnerPlayerHandler);
            NetworkServer.RegisterHandler<ReloadGameMessage>(ReloadGameHandler);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            
            NetworkClient.RegisterHandler<ClearUpdatingMessage>(ClearUpdatingHandler);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);

            if (IsSceneActive(GameplayScene))
            {
                DiContainerRoot.Instance.Resolve<IMapViewFactory>().Create();
            }
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();

            if (IsSceneActive(GameplayScene))
            {
                DiContainerRoot.Instance.Resolve<IViewService>().Create<CameraView>();
                DiContainerRoot.Instance.Resolve<IViewService>().Create<PlayerUiView>();
                
                DiContainerRoot.Instance.Resolve<IUserInputService>().ChangeCursorLockState(CursorLockMode.Locked);
            }
            else
            {
                DiContainerRoot.Instance.Resolve<IUserInputService>().ChangeCursorLockState(CursorLockMode.Confined);
            }
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient connection)
        {
            if (IsSceneActive(GameplayScene))
            {
                GameObject playerGameObject = CreatePlayerGameObject(connection);
                NetworkServer.AddPlayerForConnection(connection, playerGameObject);
            }
            else
            {
                base.OnServerAddPlayer(connection);    
            }
        }

        public override GameObject OnRoomServerCreateGamePlayer(
            NetworkConnectionToClient connection,
            GameObject roomPlayer)
        {
            return CreatePlayerGameObject(connection);
        }

        private GameObject CreatePlayerGameObject(NetworkConnectionToClient connection)
        {
            var playerView = DiContainerRoot.Instance.Resolve<IViewService>().Create<PlayerView>(false);
            SpawnPointView spawnPoint = DiContainerRoot.Instance.Resolve<MapView>().GetRandomSpawnPoint();

            playerView.PlayerName = "Player" + _playerIdentityController.GetId(connection.connectionId);
            playerView.transform.position = spawnPoint.transform.position;
            
            return playerView.gameObject;
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

            ServerChangeScene(SceneManager.GetActiveScene().path);
        }
    }
}