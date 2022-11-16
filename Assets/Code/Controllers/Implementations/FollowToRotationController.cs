using System;
using Code.Infrastructure;
using UnityEngine;

namespace Code.Controllers.Implementations
{
    public class FollowToRotationController : IUpdatable
    {
        public event EventHandler<Quaternion> CalculatedRotationChanged;
        
        public float Acceleration { get; set; }
        
        public Quaternion TargetRotation { get; set; }
        
        private readonly Transform _originTransform;

        public FollowToRotationController(Transform originTransform)
        {
            _originTransform = originTransform;
        }

        public void Update(float deltaTime)
        {
            CalculatedRotationChanged?.Invoke(this, CalculateRotation(deltaTime));
        }

        private Quaternion CalculateRotation(float deltaTime)
        {
            return Quaternion.Lerp(_originTransform.rotation, TargetRotation, Acceleration * deltaTime);
        }
    }
}