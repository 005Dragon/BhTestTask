using Code.Controllers;
using Code.Data;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private CameraData _cameraData;

        public Transform Target
        {
            get => _diContainer.Resolve<ShiftTargetController>().OriginTransform;
            set => _diContainer.Resolve<ShiftTargetController>().OriginTransform = value;
        }

        private DiContainer _diContainer;

        private void Awake()
        {
            _diContainer = new DiContainer(DiContainerRoot.Instance);
            _diContainer.Register(transform);
            _diContainer.Register(_cameraData);

            CreateControllers(_diContainer);
        }

        private static void CreateControllers(DiContainer diContainer)
        {
            CreateLookToTargetController(diContainer);
            CreateFollowToPositionController(diContainer);
            CreateRotateAroundTargetController(diContainer);
            CreateShitTargetController(diContainer);
        }
        
        private static void CreateLookToTargetController(DiContainer diContainer)
        {
            var transform = diContainer.Resolve<Transform>();

            var lookToTargetController = new LookToTargetController(transform);

            lookToTargetController.CalculatedRotationChanged += (_, rotation) => transform.rotation = rotation;
            
            diContainer.Resolve<IUpdateService>().AddToUpdate(lookToTargetController);
            
            diContainer.Register(lookToTargetController);
        }
        private static void CreateFollowToPositionController(DiContainer diDiContainer)
        {
            var transform = diDiContainer.Resolve<Transform>();
            var cameraData = diDiContainer.Resolve<CameraData>();

            var followToPositionController = new FollowToPositionController(transform)
            {
                TargetPosition = transform.position,
                Acceleration = cameraData.Acceleration
            };

            followToPositionController.CalculatedPositionChanged +=
                (_, position) => transform.position = position;

            diDiContainer.Resolve<IUpdateService>().AddToUpdate(followToPositionController);
            
            diDiContainer.Register(followToPositionController);
        }
        private static void CreateRotateAroundTargetController(DiContainer diContainer)
        {
            var cameraData = diContainer.Resolve<CameraData>();

            var rotateAroundTargetController = new RotateAroundTargetController(cameraData.VerticalAngleAnchor)
            {
                DistanceToTarget = cameraData.DistanceToTarget,
                Sensitivity = cameraData.Sensitivity,
                VerticalAngleRestrictions = cameraData.VerticalAngleRestrictions * Mathf.Deg2Rad,
                VerticalAngleAnchor = cameraData.VerticalAngleAnchor
            };

            var followToPositionController = diContainer.Resolve<FollowToPositionController>();

            rotateAroundTargetController.CalculatedPositionChanged += (_, position) =>
                followToPositionController.TargetPosition = position;
            
            diContainer.Resolve<IUpdateService>().AddToUpdate(rotateAroundTargetController);

            diContainer.Register(rotateAroundTargetController);
        }
        private static void CreateShitTargetController(DiContainer diContainer)
        {
            var shiftTargetController = new ShiftTargetController
            {
                ShiftPosition = new Vector3(0, diContainer.Resolve<CameraData>().UpShift)
            };

            var lookToTargetController = diContainer.Resolve<LookToTargetController>();
            var rotateAroundTargetController = diContainer.Resolve<RotateAroundTargetController>();

            shiftTargetController.CalculatedPositionChanged += (_, position) =>
            {
                lookToTargetController.TargetPosition = position;
                rotateAroundTargetController.TargetPosition = position;
            };

            diContainer.Resolve<IUpdateService>().AddToUpdate(shiftTargetController);

            diContainer.Register(shiftTargetController);
        }
    }
}