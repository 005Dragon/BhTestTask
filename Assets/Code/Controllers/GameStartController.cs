using System.Linq;
using Code.Infrastructure;
using Code.NetworkMessages;
using Code.Services.Contracts;
using Code.Views;
using Mirror;

namespace Code.Controllers
{
    public class GameStartController
    {
        private readonly IViewService _viewService;

        public GameStartController()
        {
            _viewService = DiContainerRoot.Instance.Resolve<IViewService>();
        }

        public void Start()
        {
            CreateNetworkManager();
        }

        private void CreateNetworkManager()
        {
            var networkManagerView = _viewService.Create<NetworkManagerView>();
            
            networkManagerView.spawnPrefabs.AddRange(
                _viewService.FindTemplates<NetworkBehaviour>().Select(x => x.gameObject)
            );
            
            networkManagerView.playerPrefab = _viewService.FindSingleTemplate<PlayerView>().gameObject;
            networkManagerView.spawnPrefabs.Remove(networkManagerView.playerPrefab);
            
            networkManagerView.ClientConnected += (_, _) => NetworkClient.Send(new CreatePlayerMessage());
        }
    }
}