using Code.Infrastructure;
using Code.Services.Contracts;
using Code.Views;
using Mirror;

namespace Code.Factories
{
    public class MapViewFactory : IMapViewFactory
    {
        private readonly IViewService _viewService;
        private readonly IHurdleViewFactory _hurdleViewFactory;

        public MapViewFactory()
        {
            _viewService = DiContainer.Instance.Resolve<IViewService>();
            _hurdleViewFactory = DiContainer.Instance.Resolve<IHurdleViewFactory>();
        }

        public MapView Create()
        {
            var mapView = _viewService.Create<MapView>();
            NetworkServer.Spawn(mapView.gameObject);

            for (int i = 0; i < mapView.CountHurdles; i++)
            {
                HurdleView hurdleView = _hurdleViewFactory.Create(mapView.SpawnHurdleRadius);
                
                NetworkServer.Spawn(hurdleView.gameObject);
            }

            return mapView;
        }
    }
}