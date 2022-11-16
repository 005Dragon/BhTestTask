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
        private readonly ISpawnPointViewFactory _spawnPointViewFactory;

        public MapViewFactory()
        {
            _viewService = DiContainer.Instance.Resolve<IViewService>();
            _hurdleViewFactory = DiContainer.Instance.Resolve<IHurdleViewFactory>();
            _spawnPointViewFactory = DiContainer.Instance.Resolve<ISpawnPointViewFactory>();
        }

        public MapView Create()
        {
            var mapView = _viewService.Create<MapView>();
            NetworkServer.Spawn(mapView.gameObject);

            for (int i = 0; i < mapView.CountSpawnPoints; i++)
            {
                SpawnPointView spawnPointView = _spawnPointViewFactory.Create(mapView.SpawnPlayerRadius);

                mapView.AddSpawnPoint(spawnPointView);
            }

            for (int i = 0; i < mapView.CountHurdles; i++)
            {
                HurdleView hurdleView = _hurdleViewFactory.Create(mapView.SpawnHurdleRadius);
                
                NetworkServer.Spawn(hurdleView.gameObject);
            }

            return mapView;
        }
    }
}