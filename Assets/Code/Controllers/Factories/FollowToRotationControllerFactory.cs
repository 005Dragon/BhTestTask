using Code.Controllers.Implementations;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;

namespace Code.Controllers.Factories
{
    public class FollowToRotationControllerFactory : IFollowToRotationControllerFactory
    {
        private readonly IUpdateService _updateService;

        public FollowToRotationControllerFactory()
        {
            _updateService = DiContainer.Instance.Resolve<IUpdateService>();
        }

        public FollowToRotationController Create(Transform cachedTransform, float acceleration)
        {
            var followToRotationController = new FollowToRotationController(cachedTransform)
            {
                Acceleration = acceleration
            };
            
            _updateService.AddToUpdate(followToRotationController);

            return followToRotationController;
        }
    }
}