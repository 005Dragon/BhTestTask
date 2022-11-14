using Code.Contracts;
using Code.Controllers;
using Code.Infrastructure;
using Code.Models;
using Code.Views;

namespace Code.Factories
{
    public class PlayerFactory : IFactory<PlayerController>
    {
        private readonly DiContainer _diContainer;

        public PlayerFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public PlayerController Create()
        {
            var playerView = _diContainer.Resolve<IViewService>().Create<PlayerView>();
            var playerModel = new PlayerModel();

            return new PlayerController(playerModel, playerView);
        }
    }
}