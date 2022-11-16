using Code.Controllers.Implementations;
using Code.Infrastructure;
using Code.Services.Contracts;
using Mirror;
using UnityEngine;

namespace Code.Views
{
    public class PlayerView : NetworkBehaviour
    {
        [Header("Simple movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _moveAcceleration;
        [SerializeField] private float _rotationAcceleration;
        
        [Header("Spurt movement")]
        [SerializeField] private float _spurtDistance;
        [SerializeField] private float _spurtSpeed;
        [SerializeField] private float _spurtCooldown;
        
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

            PlayerRotateController playerRotateController =
                CreatePlayerRotateController(followToRotationController, updateService);

            FollowToPositionController followToPositionController =
                CreateFollowToPositionController(cachedTransform, _moveAcceleration);

            PlayerMoveController playerMoveController = CreatePlayerMoveController(
                cachedTransform,
                followToPositionController,
                followToRotationController,
                updateService
            );

            var userInputService = DiContainer.Instance.Resolve<IUserInputService>();

            CreatePlayerSpurtController(
                cachedTransform,
                followToPositionController,
                playerRotateController,
                playerMoveController,
                updateService,
                userInputService
            );
        }

        private void CreatePlayerSpurtController(
            Transform cachedTransform,
            FollowToPositionController followToPositionController,
            PlayerRotateController playerRotateController,
            PlayerMoveController playerMoveController,
            IUpdateService updateService,
            IUserInputService userInputService)
        {
            var playerSpurtController = new PlayerSpurtController(cachedTransform)
            {
                Distance = _spurtDistance,
                Speed = _spurtSpeed,
                Cooldown = _spurtCooldown
            };

            playerSpurtController.ActiveChanged += (_, active) =>
            {
                playerMoveController.IsActive = !active;
                playerRotateController.IsActive = !active;
            };

            playerSpurtController.CalculatedPositionChanged += (_, position) =>
            {
                cachedTransform.position = position;
                followToPositionController.TargetPosition = position;
            };
            
            updateService.AddToUpdate(playerSpurtController);
            userInputService.MainAction += (_, _) => playerSpurtController.Active();
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

        private static PlayerRotateController CreatePlayerRotateController(
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

            return playerRotateController;
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

        private PlayerMoveController CreatePlayerMoveController(
            Transform cachedTransform,
            FollowToPositionController followToPositionController,
            FollowToRotationController followToRotationController,
            IUpdateService updateService)
        {
            var playerMoveController = new PlayerMoveController(cachedTransform)
            {
                IsActive = true,
                Speed = _moveSpeed
            };

            playerMoveController.CalculatedPositionChanged +=
                (_, position) => followToPositionController.TargetPosition = position;

            playerMoveController.MoveImpulseMagnitudeChanged += (_, moveImpulseMagnitude) =>
                followToRotationController.Acceleration = _rotationAcceleration * moveImpulseMagnitude;
            
            updateService.AddToUpdate(playerMoveController);

            return playerMoveController;
        }
    }
}