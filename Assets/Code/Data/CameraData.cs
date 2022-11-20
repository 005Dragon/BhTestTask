using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(menuName = "GameData/" + nameof(CameraData), fileName = nameof(CameraData))]
    public class CameraData : ScriptableObject
    {
        [SerializeField] private float _upShift;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _distanceToTarget;
        [SerializeField] private float _verticalAngleAnchor;
        [SerializeField] private float _verticalAngleRestrictions;
        [SerializeField] private float _sensitivity;
        
        public float UpShift => _upShift;
        public float Acceleration => _acceleration;
        public float DistanceToTarget => _distanceToTarget;
        public float VerticalAngleAnchor => _verticalAngleAnchor * Mathf.Deg2Rad;
        public float VerticalAngleRestrictions => _verticalAngleRestrictions * Mathf.Deg2Rad;
        public float Sensitivity => _sensitivity;
    }
}