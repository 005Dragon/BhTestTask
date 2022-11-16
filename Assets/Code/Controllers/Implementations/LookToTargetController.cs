using System;
using Code.Infrastructure;
using UnityEngine;

namespace Code.Controllers.Implementations
{
    public class LookToTargetController : IUpdatable
    {
        public event EventHandler<Quaternion> CalculatedRotationChanged; 

        public Vector3 TargetPosition { get; set; }

        private readonly Transform _originTransform;

        public LookToTargetController(Transform originTransform)
        {
            _originTransform = originTransform;
        }

        public void Update(float deltaTime)
        {
            CalculatedRotationChanged?.Invoke(this, CalculateRotation());
        }

        private Quaternion CalculateRotation()
        {
            return Quaternion.LookRotation(TargetPosition - _originTransform.position);
        }
    }
}