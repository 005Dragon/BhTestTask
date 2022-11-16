using System;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;

namespace Code.Controllers
{
    public class RotateAroundTargetController : IUpdatable
    {
        public event EventHandler<Vector3> CalculatedPositionChanged;

        public Vector3 TargetPosition { set; get; }
        public float DistanceToTarget { get; set; }
        public float VerticalAngleRestrictions { get; set; }
        public float VerticalAngleAnchor { get; set; }
        public float Sensitivity { get; set; }
        
        public float VerticalAngle { get; private set; }
        public float HorizontalAngle { get; private set; }

        private readonly IUserInputService _userInputService;

        public RotateAroundTargetController(float verticalAngle = 0, float horizontalAngle = 0)
        {
            _userInputService = DiContainer.Instance.Resolve<IUserInputService>();

            VerticalAngle = verticalAngle;
            HorizontalAngle = horizontalAngle;
        }

        public void Update(float deltaTime)
        {
            Vector2 userInputImpulse = GetUserInputImpulse(deltaTime);

            UpdateHorizontalAngle(userInputImpulse.x);
            UpdateVerticalAngle(userInputImpulse.y);
            
            CalculatedPositionChanged?.Invoke(this, CalculatePosition());
        }

        private Vector3 CalculatePosition()
        {
            float sinVerticalAngle = Mathf.Sin(VerticalAngle);
            
            var relativePosition = new Vector3(
                DistanceToTarget * sinVerticalAngle * Mathf.Cos(HorizontalAngle),
                DistanceToTarget * Mathf.Cos(VerticalAngle),
                DistanceToTarget * sinVerticalAngle * Mathf.Sin(HorizontalAngle)
            );

            return TargetPosition + relativePosition;
        }

        private void UpdateHorizontalAngle(float horizontalUserInputImpulse)
        {
            HorizontalAngle -= horizontalUserInputImpulse * Sensitivity;
        }

        private void UpdateVerticalAngle(float verticalUserInputImpulse)
        {
            VerticalAngle = Mathf.Clamp(
                VerticalAngle + verticalUserInputImpulse * Sensitivity,
                VerticalAngleAnchor - VerticalAngleRestrictions,
                VerticalAngleAnchor + VerticalAngleRestrictions
            );
        }

        private Vector2 GetUserInputImpulse(float deltaTime) => _userInputService.RotateImpulseInput * deltaTime;
    }
}