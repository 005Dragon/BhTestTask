using System;
using Code.Infrastructure;
using UnityEngine;

namespace Code.Controllers
{
    public class PlayerSpurtController : IUpdatable
    {
        public event EventHandler<Vector3> CalculatedPositionChanged;
        public event EventHandler<bool> ActiveChanged;
        
        public float Distance { get; set; }
        public float Speed { get; set; }
        public float Cooldown { get; set; }

        private readonly Transform _playerTransform;

        private bool _isActive;
        private float _currentCooldown;
        private float _progress;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;

        public PlayerSpurtController(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public void Active()
        {
            if (_isActive || _currentCooldown > 0)
            {
                return;
            }
            
            ResetProgress();
            ResetCooldown();
            
            _startPosition = _playerTransform.position;
            _targetPosition = GetTargetPosition();
            
            _isActive = true;
            ActiveChanged?.Invoke(this, _isActive);
        }

        public void Update(float deltaTime)
        {
            Debug.DrawLine(_startPosition, _targetPosition);
            
            if (_isActive)
            {
                UpdateProgress(deltaTime, out bool progressFinished);
                
                CalculatedPositionChanged?.Invoke(this, CalculatePlayerPosition());

                if (progressFinished)
                {
                    _isActive = false;
                    ActiveChanged?.Invoke(this, _isActive);
                }
            }
            else
            {
                UpdateCooldown(deltaTime);
            }
        }

        private Vector3 CalculatePlayerPosition() => Vector3.Lerp(_startPosition, _targetPosition, _progress);
        private void ResetProgress() => _progress = 0.0f;
        private void UpdateProgress(float deltaTime, out bool progressFinished)
        {
            _progress += (Speed * deltaTime) / Distance;

            progressFinished = _progress > 1.0f;

            if (progressFinished)
            {
                _progress = 1.0f;
            }
        }
        private void UpdateCooldown(float deltaTime) => _currentCooldown -= deltaTime;
        private void ResetCooldown() => _currentCooldown = Cooldown;
        private Vector3 GetTargetPosition() => _startPosition + _playerTransform.forward * Distance;
    }
}