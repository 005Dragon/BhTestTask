using System;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;

namespace Code.Controllers.Implementations
{
    public class PlayerMoveController : IUpdatable
    {
        public event EventHandler<Vector3> CalculatedPositionChanged;
        public event EventHandler<float> MoveImpulseMagnitudeChanged; 

        public bool IsActive { get; set; }
        public float Speed { get; set; }
        
        private readonly Transform _playerTransform;
        private readonly IUserInputService _userInputService;

        public PlayerMoveController(Transform playerTransform)
        {
            _playerTransform = playerTransform;
            _userInputService = DiContainer.Instance.Resolve<IUserInputService>();
        }

        public void Update(float deltaTime)
        {
            if (!IsActive)
            {
                return;
            }

            Vector3 moveImpulse = CalculateMoveImpulse(deltaTime);
            
            MoveImpulseMagnitudeChanged?.Invoke(this, moveImpulse.magnitude);

            CalculatedPositionChanged?.Invoke(this, CalculatePosition(moveImpulse));
        }

        private Vector3 CalculateMoveImpulse(float deltaTime)
        {
            return
                _playerTransform.forward * (_userInputService.MoveInput.y * deltaTime * Speed) +
                _playerTransform.right * (_userInputService.MoveInput.x * deltaTime * Speed);
        }
        private Vector3 CalculatePosition(Vector3 moveImpulse)
        {
            return _playerTransform.position + moveImpulse;
        }
    }
}