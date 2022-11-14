using Code.Contracts;
using Code.Controllers;
using Code.Infrastructure;

namespace Code
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
            _diContainer.Resolve<IFactory<PlayerController>>().Create();
        }
    }
}