using Code.Controllers;
using Code.Infrastructure;
using Code.Services.Contracts;
using Code.Views;

namespace Code.Factories
{
    public class NetworkManagerFactory : IFactory<NetworkManagerController>
    {
        private readonly DiContainer _diContainer;

        public NetworkManagerFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public NetworkManagerController Create()
        {
            var view = _diContainer.Resolve<IViewService>().Create<NetworkManagerView>();
            view.playerPrefab = _diContainer.Resolve<IViewService>().FindTemplate<PlayerView>().gameObject;

            return new NetworkManagerController(view, _diContainer);
        }
    }
}