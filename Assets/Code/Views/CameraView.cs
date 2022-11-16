using Code.Controllers;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private float _upShift;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _distanceToTarget;
        [SerializeField] private float _verticalAngleAnchor;
        [SerializeField] private float _verticalAngleRestrictions;
        [SerializeField] private float _sensitivity;

        public Transform Target
        {
            get => _shiftTargetController.OriginTransform;
            set => _shiftTargetController.OriginTransform = value;
        }

        private ShiftTargetController _shiftTargetController;

        private void Awake()
        {
            CreateControllers(transform);
        }

        private void CreateControllers(Transform cachedTransform)
        {
            var updateService = DiContainer.Instance.Resolve<IUpdateService>();

            LookToTargetController lookToTargetController =
                CreateLookToTargetController(cachedTransform, updateService);

            FollowToPositionController followToPositionController =
                CreateFollowToPositionController(cachedTransform, _acceleration);

            RotateAroundTargetController rotateAroundTargetController =
                CreateRotateAroundTargetController(followToPositionController, updateService);

            _shiftTargetController = 
                CreateShitTargetController(lookToTargetController, rotateAroundTargetController, updateService);
        }
        
        private static LookToTargetController CreateLookToTargetController(
            Transform cachedTransform,
            IUpdateService updateService)
        {
            var lookToTargetController = new LookToTargetController(cachedTransform);

            lookToTargetController.CalculatedRotationChanged += (_, rotation) => cachedTransform.rotation = rotation;
            
            updateService.AddToUpdate(lookToTargetController);

            return lookToTargetController;
        }

        private ShiftTargetController CreateShitTargetController(
            LookToTargetController lookToTargetController,
            RotateAroundTargetController rotateAroundTargetController,
            IUpdateService updateService)
        {
            var shiftTargetController = new ShiftTargetController
            {
                ShiftPosition = new Vector3(0, _upShift)
            };

            shiftTargetController.CalculatedPositionChanged += (_, position) =>
            {
                lookToTargetController.TargetPosition = position;
                rotateAroundTargetController.TargetPosition = position;
            };
            
            updateService.AddToUpdate(shiftTargetController);

            return shiftTargetController;
        }

        private RotateAroundTargetController CreateRotateAroundTargetController(
            FollowToPositionController followToPositionController,
            IUpdateService updateService)
        {
            float verticalAngleAnchor = _verticalAngleAnchor * Mathf.Deg2Rad;

            var rotateAroundTargetController = new RotateAroundTargetController(verticalAngleAnchor)
            {
                DistanceToTarget = _distanceToTarget,
                Sensitivity = _sensitivity,
                VerticalAngleRestrictions = _verticalAngleRestrictions * Mathf.Deg2Rad,
                VerticalAngleAnchor = verticalAngleAnchor
            };

            rotateAroundTargetController.CalculatedPositionChanged += (_, position) =>
                followToPositionController.TargetPosition = position;
            
            updateService.AddToUpdate(rotateAroundTargetController);

            return rotateAroundTargetController;
        }

        private static FollowToPositionController CreateFollowToPositionController(
            Transform cachedTransform,
            float acceleration)
        {
            FollowToPositionController followToPositionController = 
                DiContainer.Instance
                    .Resolve<IFollowToPositionControllerFactory>()
                    .Create(cachedTransform, acceleration);

            followToPositionController.CalculatedPositionChanged +=
                (_, position) => cachedTransform.position = position;

            return followToPositionController;
        }
    }
}