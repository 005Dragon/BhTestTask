using Code.Controllers;
using Code.Infrastructure;
using Code.Services.Contracts;

namespace Code.Services.Implementations
{
    public class StartGameService : IStartGameService
    {
        private readonly DiContainer _diContainer;

        public StartGameService(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void Start()
        {
            NetworkManagerController networkManagerController =
                _diContainer.Resolve<IFactory<NetworkManagerController>>().Create();
            
            
        }
    }
}