using System;
using Code.Infrastructure;
using UnityEngine;

namespace Code.Controllers
{
    public class ShiftTargetController : IFixedUpdatable
    {
        public event EventHandler<Vector3> CalculatedPositionChanged;
        public Vector3 ShiftPosition { get; set; }
        public Transform OriginTransform { get; set; }

        public void FixedUpdate(float deltaTime)
        {
            CalculatedPositionChanged?.Invoke(this, CalculatePosition());
        }

        private Vector3 CalculatePosition()
        {
            return OriginTransform == null ? ShiftPosition : OriginTransform.position + ShiftPosition;
        }
    }
}