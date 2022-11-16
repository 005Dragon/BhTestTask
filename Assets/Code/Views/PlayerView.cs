using Code.Controllers.Implementations;
using Code.Infrastructure;
using Code.Services.Contracts;
using Mirror;
using UnityEngine;

namespace Code.Views
{
    public class PlayerView : NetworkBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _moveAcceleration;
        [SerializeField] private float _rotationAcceleration;
        
        public override void OnStartClient()
        {
            base.OnStartClient();

            if (isOwned)
            {
                Transform cachedTransform = transform;
                
                InitializeCamera(cachedTransform);
                CreateControllers(cachedTransform);
            }
        }

        private void InitializeCamera(Transform cachedTransform)
        {
            var cameraView = DiContainer.Instance.Resolve<CameraView>();
            cameraView.Target = cachedTransform;
        }

        private void CreateControllers(Transform cachedTransform)
        {
            var updateService = DiContainer.Instance.Resolve<IUpdateService>();
            
            FollowToRotationController followToRotationController = 
                CreateFollowToRotationController(cachedTransform, updateService);

            CreatePlayerRotateController(followToRotationController, updateService);

            FollowToPositionController followToPositionController =
                CreateFollowToPositionController(cachedTransform, _moveAcceleration);

            CreatePlayerMoveController(
                cachedTransform,
                _moveSpeed,
                _rotationAcceleration,
                followToPositionController,
                followToRotationController,
                updateService
            );
        }

        private static FollowToRotationController CreateFollowToRotationController(
            Transform cachedTransform,
            IUpdateService updateService)
        {
            var followToRotationController = new FollowToRotationController(cachedTransform);

            followToRotationController.CalculatedRotationChanged +=
                (_, rotation) => cachedTransform.rotation = rotation;
            
            updateService.AddToUpdate(followToRotationController);

            return followToRotationController;
        }

        private static void CreatePlayerRotateController(
            FollowToRotationController followToRotationController,
            IUpdateService updateService)
        {
            var playerRotateController = new PlayerRotateController()
            {
                IsActive = true
            };

            playerRotateController.CalculatedRotationChanged +=
                (_, rotation) => followToRotationController.TargetRotation = rotation;
            
            updateService.AddToUpdate(playerRotateController);
        }

        private static FollowToPositionController CreateFollowToPositionController(
            Transform cachedTransform,
            float acceleration)
        {
            FollowToPositionController followToPositionController = DiContainer.Instance
                .Resolve<IFollowToPositionControllerFactory>()
                .Create(cachedTransform, acceleration);

            followToPositionController.CalculatedPositionChanged +=
                (_, position) => cachedTransform.position = position;
            
            return followToPositionController;
        }

        private static void CreatePlayerMoveController(
            Transform cachedTransform,
            float speed,
            float rotationAcceleration,
            FollowToPositionController followToPositionController,
            FollowToRotationController followToRotationController,
            IUpdateService updateService)
        {
            var playerMoveController = new PlayerMoveController(cachedTransform)
            {
                IsActive = true,
                Speed = speed
            };

            playerMoveController.CalculatedPositionChanged +=
                (_, position) => followToPositionController.TargetPosition = position;

            playerMoveController.MoveImpulseMagnitudeChanged += (_, moveImpulseMagnitude) =>
                followToRotationController.Acceleration = rotationAcceleration * moveImpulseMagnitude;
            
            updateService.AddToUpdate(playerMoveController);
        }
    }
}