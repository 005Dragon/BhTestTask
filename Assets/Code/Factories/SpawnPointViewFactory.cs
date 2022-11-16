using Code.Infrastructure;
using Code.Services.Contracts;
using Code.Views;
using UnityEngine;

namespace Code.Factories
{
    public class SpawnPointViewFactory : ISpawnPointViewFactory
    {
        private readonly IViewService _viewService;

        public SpawnPointViewFactory()
        {
            _viewService = DiContainer.Instance.Resolve<IViewService>();
        }

        public SpawnPointView Create(float spawnRadius)
        {
            var spawnPointView = _viewService.Create<SpawnPointView>(false);

            float anglePosition = GetRandomAngle();
            float distance = Random.Range(0, spawnRadius);

            Transform spawnPointViewTransform = spawnPointView.transform;

            spawnPointViewTransform.position = new Vector3(
                Mathf.Cos(anglePosition) * distance,
                spawnPointViewTransform.position.y,
                Mathf.Sin(anglePosition) * distance
            );
            
            return spawnPointView;
        }
        
        private float GetRandomAngle() => Random.Range(0, 2 * Mathf.PI);
    }
}