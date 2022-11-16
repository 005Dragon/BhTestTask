using System;
using Code.Infrastructure;
using UnityEngine;

namespace Code.Controllers
{
    public class ShiftTargetController : IUpdatable
    {
        public event EventHandler<Vector3> CalculatedPositionChanged;
        public Vector3 ShiftPosition { get; set; }
        public Transform OriginTransform { get; set; }

        public void Update(float deltaTime)
        {
            CalculatedPositionChanged?.Invoke(this, CalculatePosition());
        }

        private Vector3 CalculatePosition()
        {
            return OriginTransform == null ? ShiftPosition : OriginTransform.position + ShiftPosition;
        }
    }
}