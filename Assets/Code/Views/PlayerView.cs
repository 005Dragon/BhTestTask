using Code.Controllers;
using Code.Data;
using Code.Infrastructure;
using Code.Services.Contracts;
using Mirror;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerView : NetworkBehaviour
    {
        [Header("Inner references")] 
        [SerializeField] private MeshRenderer _bodyMeshRenderer;
        
        [Header("Simple movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _moveAcceleration;
        [SerializeField] private float _rotationAcceleration;
        
        [Header("Spurt movement")]
        [SerializeField] private float _spurtDistance;
        [SerializeField] private float _spurtSpeed;
        [SerializeField] private float _spurtCooldown;

        [SyncVar(hook = nameof(SyncBodyMaterial))] 
        // ReSharper disable once NotAccessedField.Local
        private MaterialKey _currentBodyMaterialKey;

        private IUpdateService _updateService;
        private IUserInputService _userInputService;
        private IMaterialService _materialService;

        private void Awake()
        {
            _updateService = DiContainer.Instance.Resolve<IUpdateService>();
            _userInputService = DiContainer.Instance.Resolve<IUserInputService>();
            _materialService = DiContainer.Instance.Resolve<IMaterialService>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if (!isOwned)
            {
                return;
            }
            
            Transform cachedTransform = transform;

            InitializeCamera(cachedTransform);
            CreateControllers(cachedTransform, GetComponent<Rigidbody>());
        }

        private void InitializeCamera(Transform cachedTransform)
        {
            var cameraView = DiContainer.Instance.Resolve<CameraView>();
            cameraView.Target = cachedTransform;
        }

        private void CreateControllers(Transform cachedTransform, Rigidbody cachedRigidbody)
        {
            FollowToRotationController followToRotationController = 
                CreateFollowToRotationController(cachedTransform);

            PlayerRotateController playerRotateController =
                CreatePlayerRotateController(followToRotationController);

            FollowToPositionController followToPositionController =
                CreateFollowToPositionController(cachedTransform, cachedRigidbody, _moveAcceleration);

            PlayerMoveController playerMoveController = CreatePlayerMoveController(
                cachedTransform,
                followToPositionController,
                followToRotationController
            );

            CreatePlayerSpurtController(
                cachedTransform,
                followToPositionController,
                playerRotateController,
                playerMoveController
            );
        }

        private void CreatePlayerSpurtController(
            Transform cachedTransform,
            FollowToPositionController followToPositionController,
            PlayerRotateController playerRotateController,
            PlayerMoveController playerMoveController)
        {
            var playerSpurtController = new PlayerSpurtController(cachedTransform)
            {
                Distance = _spurtDistance,
                Speed = _spurtSpeed,
                Cooldown = _spurtCooldown
            };

            playerSpurtController.ReadyChanged += (_, isReady) =>
            {
                CmdChangeBodyMaterial(isReady ? MaterialKey.ActivePlayer : MaterialKey.DefaultPlayer);
            };

            playerSpurtController.ActiveChanged += (_, isActive) =>
            {
                playerMoveController.IsActive = !isActive;
                playerRotateController.IsActive = !isActive;
            };

            playerSpurtController.CalculatedPositionChanged += (_, position) =>
            {
                cachedTransform.position = position;
                followToPositionController.TargetPosition = position;
            };
            
            _updateService.AddToUpdate(playerSpurtController);
            _userInputService.MainAction += (_, _) => playerSpurtController.Active();
        }

        private FollowToRotationController CreateFollowToRotationController(Transform cachedTransform)
        {
            var followToRotationController = new FollowToRotationController(cachedTransform);

            followToRotationController.CalculatedRotationChanged +=
                (_, rotation) => cachedTransform.rotation = rotation;
            
            _updateService.AddToUpdate(followToRotationController);

            return followToRotationController;
        }

        private PlayerRotateController CreatePlayerRotateController(
            FollowToRotationController followToRotationController)
        {
            var playerRotateController = new PlayerRotateController()
            {
                IsActive = true
            };

            playerRotateController.CalculatedRotationChanged +=
                (_, rotation) => followToRotationController.TargetRotation = rotation;
            
            _updateService.AddToUpdate(playerRotateController);

            return playerRotateController;
        }

        private PlayerMoveController CreatePlayerMoveController(
            Transform cachedTransform,
            FollowToPositionController followToPositionController,
            FollowToRotationController followToRotationController)
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
            
            _updateService.AddToUpdate(playerMoveController);

            return playerMoveController;
        }

        private static FollowToPositionController CreateFollowToPositionController(
            Transform cachedTransform,
            Rigidbody rigidbody,
            float acceleration)
        {
            FollowToPositionController followToPositionController = DiContainer.Instance
                .Resolve<IFollowToPositionControllerFactory>()
                .Create(cachedTransform, acceleration);

            followToPositionController.CalculatedPositionChanged +=
                (_, position) => rigidbody.MovePosition(position);

            return followToPositionController;
        }

        [Command]
        private void CmdChangeBodyMaterial(MaterialKey newValue) => _currentBodyMaterialKey = newValue;
        
        // ReSharper disable once UnusedParameter.Local
        private void SyncBodyMaterial(MaterialKey oldValue, MaterialKey newValue)
        {
            _bodyMeshRenderer.material = _materialService.GetMaterial(newValue);
        }
    }
}