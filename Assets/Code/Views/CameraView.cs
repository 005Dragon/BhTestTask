using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _upShift;
        [SerializeField] private float _distanceToTarget;
        [SerializeField] private float _verticalAngleRestrictions;
        [SerializeField] private float _sensitivity;
        
        public Transform Target { get; set; }

        private Transform _cachedTransform;
        private float _horizontalAngle;
        private float _verticalAngle;

        public void RotateAroundTarget(Vector2 siftAngleVector)
        {
            _horizontalAngle -= siftAngleVector.x * _sensitivity;
            _verticalAngle = Mathf.Clamp(
                _verticalAngle + siftAngleVector.y * _sensitivity,
                Mathf.PI / 2 - _verticalAngleRestrictions,
                Mathf.PI / 2 + _verticalAngleRestrictions
            );
        }

        private void Awake()
        {
            _cachedTransform = transform;
        }

        private void FixedUpdate()
        {
            if (Target != null)
            {
                _cachedTransform.position = 
                    Vector3.Lerp(_cachedTransform.position, GetCameraPosition(), Time.deltaTime * _moveSpeed);
                
                _cachedTransform.LookAt(GetTargetPosition());
            }
        }
        
        private Vector3 GetCameraPosition()
        {
            float sinVerticalAngle = Mathf.Sin(_verticalAngle);

            return GetTargetPosition() + new Vector3(
                _distanceToTarget * sinVerticalAngle * Mathf.Cos(_horizontalAngle),
                _distanceToTarget * Mathf.Cos(_verticalAngle),
                _distanceToTarget * sinVerticalAngle * Mathf.Sin(_horizontalAngle)
            );
        }

        private Vector3 GetTargetPosition() => Target.position + new Vector3(0, _upShift);
    }
}