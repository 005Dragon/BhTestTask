using System.Linq;
using Code.Infrastructure;
using Code.Services.Contracts;
using Code.Views;
using UnityEngine;

namespace Code.Factories
{
    public class HurdleViewFactory : IHurdleViewFactory
    {
        private readonly IViewService _viewService;

        public HurdleViewFactory()
        {
            _viewService = DiContainerRoot.Instance.Resolve<IViewService>();
        }

        public HurdleView Create(float spawnRadius)
        {
            HurdleView[] hurdleViewTemplates = _viewService.FindTemplates<HurdleView>().ToArray();
            
            HurdleView hurdleView =
                Object.Instantiate(hurdleViewTemplates[Random.Range(0, hurdleViewTemplates.Length)]);

            float anglePosition = GetRandomAngle();
            float distance = Random.Range(0, spawnRadius);

            Transform hurdleViewTransform = hurdleView.transform;

            hurdleViewTransform.position = new Vector3(
                Mathf.Cos(anglePosition) * distance,
                hurdleViewTransform.position.y,
                Mathf.Sin(anglePosition) * distance
            );
            
            hurdleViewTransform.rotation = Quaternion.AngleAxis(GetRandomAngle(), Vector3.up);

            return hurdleView;
        }
        
        private float GetRandomAngle() => Random.Range(0, 2 * Mathf.PI);
    }
}