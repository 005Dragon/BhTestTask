using System;
using System.Diagnostics.CodeAnalysis;
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

        [Header("Data")] 
        [SerializeField] private PlayerData _playerData;

        public event EventHandler<int> EnemyDamaged;

        [field: SyncVar(hook = nameof(UpdateBodyMaterial))]
        public PlayerState State { get; private set; }
        
        private DiContainer _diContainer;

        private void Awake()
        {
            _diContainer = new DiContainer(DiContainerRoot.Instance);
            _diContainer.Register(transform);
            _diContainer.Register(GetComponent<Rigidbody>());
            _diContainer.Register(_playerData);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (!isOwned)
            {
                return;
            }

            CreateControllers(_diContainer);
            InitializePlayerUi(_diContainer);
            InitializeCamera(_diContainer);
            InitializeApplyingDataFromControllers(_diContainer);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isOwned)
            {
                _diContainer.Resolve<PlayerSpurtCollisionController>().CheckCollision(State, collision);
            }
        }

        private void InitializePlayerUi(DiContainer diContainer)
        {
            var playerUiView = diContainer.Resolve<PlayerUiView>();
            playerUiView.Initialize(this);
        }

        private static void InitializeCamera(DiContainer diContainer)
        {
            var cameraView = diContainer.Resolve<CameraView>();
            cameraView.Target = diContainer.Resolve<Transform>();
        }

        private static void InitializeApplyingDataFromControllers(DiContainer diContainer)
        {
            var transform = diContainer.Resolve<Transform>();

            diContainer.Resolve<FollowToRotationController>().CalculatedRotationChanged  += 
                (_, rotation) => transform.rotation = rotation;
            
            var rigidbody = diContainer.Resolve<Rigidbody>();

            diContainer.Resolve<FollowToPositionController>().CalculatedPositionChanged +=
                (_, position) => rigidbody.MovePosition(position);
            
            diContainer.Resolve<IUserInputService>().MainAction +=
                (_, _) => diContainer.Resolve<PlayerStateController>().TryChangeStateToActiveSpurt();
        }

        private void CreateControllers(DiContainer diContainer)
        {
            CreatePlayerSpurtCollisionController(diContainer);
            CreateFollowToRotationController(diContainer);
            CreatePlayerRotateController(diContainer);
            CreateFollowToPositionController(diContainer);
            CreatePlayerMoveController(diContainer);
            CreatePlayerSpurtController(diContainer);
            CreatePlayerStateController(diContainer);
        }

        private void CreatePlayerSpurtCollisionController(DiContainer diContainer)
        {
            var playerSpurtCollisionController = new PlayerSpurtCollisionController();

            playerSpurtCollisionController.SelfDamaged +=
                (_, _) => diContainer.Resolve<PlayerStateController>().TryChangeStateToDisable();

            playerSpurtCollisionController.Intersected += 
                (_, _) => diContainer.Resolve<PlayerSpurtController>().Stop();
            
            diContainer.Register(playerSpurtCollisionController);
        }

        private static void CreateFollowToRotationController(DiContainer diContainer)
        {
            var transform = diContainer.Resolve<Transform>();
            
            var followToRotationController = new FollowToRotationController(transform);

            diContainer.Resolve<IUpdateService>().AddToUpdate(followToRotationController);

            diContainer.Register(followToRotationController);
        }

        private static void CreatePlayerRotateController(DiContainer diContainer)
        {
            var playerRotateController = new PlayerRotateController()
            {
                IsActive = true
            };

            playerRotateController.CalculatedRotationChanged +=
                (_, rotation) => diContainer.Resolve<FollowToRotationController>().TargetRotation = rotation;

            diContainer.Resolve<IUpdateService>().AddToUpdate(playerRotateController);

            diContainer.Register(playerRotateController);
        }

        private static void CreateFollowToPositionController(DiContainer diContainer)
        {
            var transform = diContainer.Resolve<Transform>();
            
            var followToPositionController = new FollowToPositionController(transform)
            {
                TargetPosition = transform.position,
                Acceleration = diContainer.Resolve<PlayerData>().MoveAcceleration
            };

            diContainer.Resolve<IUpdateService>().AddToUpdate(followToPositionController);
            
            diContainer.Register(followToPositionController);
        }

        private static void CreatePlayerMoveController(DiContainer diContainer)
        {
            var playerData = diContainer.Resolve<PlayerData>();

            var playerMoveController = new PlayerMoveController(diContainer.Resolve<Transform>())
            {
                IsActive = true,
                Speed = playerData.MoveSpeed
            };

            var followToPositionController = diContainer.Resolve<FollowToPositionController>();

            playerMoveController.CalculatedPositionChanged +=
                (_, position) => followToPositionController.TargetPosition = position;

            var followToRotationController = diContainer.Resolve<FollowToRotationController>();

            playerMoveController.MoveImpulseMagnitudeChanged += (_, moveImpulseMagnitude) =>
                followToRotationController.Acceleration = playerData.RotationAcceleration * moveImpulseMagnitude;

            diContainer.Resolve<IUpdateService>().AddToUpdate(playerMoveController);

            diContainer.Register(playerMoveController);
        }

        private void CreatePlayerSpurtController(DiContainer diContainer)
        {
            var cachedTransform = diContainer.Resolve<Transform>();
            var playerData = diContainer.Resolve<PlayerData>();

            var playerSpurtController = new PlayerSpurtController(cachedTransform)
            {
                Distance = playerData.SpurtDistance,
                Speed = playerData.SpurtSpeed
            };

            var playerMoveController = diContainer.Resolve<PlayerMoveController>();
            var playerRotateController = diContainer.Resolve<PlayerRotateController>();
            var playerSpurtCollisionController = diContainer.Resolve<PlayerSpurtCollisionController>();

            playerSpurtController.ActiveChanged += (_, isActive) =>
            {
                playerMoveController.IsActive = !isActive;
                playerRotateController.IsActive = !isActive;

                if (!isActive)
                {
                    diContainer.Resolve<PlayerStateController>().TryChangeStateToDisableSpurt();
                    
                    if (playerSpurtCollisionController.DamagedPlayers > 0)
                    {
                        EnemyDamaged?.Invoke(this, playerSpurtCollisionController.DamagedPlayers);
                        playerSpurtCollisionController.ResetDamagedPlayers();
                    }
                }
            };

            var followToPositionController = diContainer.Resolve<FollowToPositionController>();

            playerSpurtController.CalculatedPositionChanged += (_, position) =>
            {
                cachedTransform.position = position;
                followToPositionController.TargetPosition = position;
            };
            
            diContainer.Resolve<IUpdateService>().AddToUpdate(playerSpurtController);
            
            diContainer.Register(playerSpurtController);
        }

        private void CreatePlayerStateController(DiContainer diContainer)
        {
            var playerData = diContainer.Resolve<PlayerData>();

            var playerStateController = new PlayerStateController
            {
                DisableCooldown = playerData.DisableCooldown,
                SpurtCooldown = playerData.SpurtCooldown
            };

            var playerSpurtController = diContainer.Resolve<PlayerSpurtController>();

            playerStateController.StateChanged += (_, state) =>
            {
                if (state == PlayerState.Spurt)
                {
                    playerSpurtController.Active();
                }
                
                PlayerStateControllerOnStateChanged(state);
            };

            diContainer.Resolve<IUpdateService>().AddToUpdate(playerStateController);
            
            diContainer.Register(playerStateController);
        }

        [Command]
        private void PlayerStateControllerOnStateChanged(PlayerState playerState)
        {
            State = playerState;
        }
        
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void UpdateBodyMaterial(PlayerState oldValue, PlayerState newValue)
        {
            MaterialKey bodyMaterialKey = State switch
            {
                PlayerState.Default => MaterialKey.DefaultPlayer,
                PlayerState.Disable => MaterialKey.DisablePlayer,
                PlayerState.SpurtReady => MaterialKey.SpurtReadyPlayer,
                PlayerState.Spurt => MaterialKey.SpurtPlayer,
                _ => throw new ArgumentOutOfRangeException(nameof(State), State, null)
            };

            _bodyMeshRenderer.material = _diContainer.Resolve<IMaterialService>().GetMaterial(bodyMaterialKey);
        }
    }
}