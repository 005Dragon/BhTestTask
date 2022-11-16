using Code.Controllers;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;

namespace Code.Factories
{
    public class FollowToPositionControllerFactory : IFollowToPositionControllerFactory
    {
        private readonly IUpdateService _updateService;

        public FollowToPositionControllerFactory()
        {
            _updateService = DiContainer.Instance.Resolve<IUpdateService>();
        }

        public FollowToPositionController Create(Transform cachedTransform, float acceleration)
        {
            var followToPositionController = new FollowToPositionController(cachedTransform)
            {
                Acceleration = acceleration
            };
            
            _updateService.AddToUpdate(followToPositionController);

            return followToPositionController;
        }
    }
}