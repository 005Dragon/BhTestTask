using System;
using Code.Infrastructure;
using UnityEngine;

namespace Code.Controllers
{
    public class FollowToPositionController : IUpdatable
    {
        public event EventHandler<Vector3> CalculatedPositionChanged;

        public float Acceleration { get; set; }
        public Vector3 TargetPosition { get; set; }

        private readonly Transform _originTransform;

        public FollowToPositionController(Transform originTransform)
        {
            _originTransform = originTransform;
        }

        public void Update(float deltaTime)
        {
            CalculatedPositionChanged?.Invoke(this, CalculatePosition(deltaTime));
        }

        private Vector3 CalculatePosition(float deltaTime)
        {
            return Vector3.Lerp(_originTransform.position, TargetPosition, Acceleration * deltaTime);
        }
    }
}