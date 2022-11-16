using System;
using Code.Infrastructure;
using UnityEngine;

namespace Code.Controllers
{
    public class PlayerSpurtController : IUpdatable
    {
        public event EventHandler<Vector3> CalculatedPositionChanged;
        public event EventHandler<bool> ActiveChanged;
        public event EventHandler<bool> ReadyChanged;
        
        public float Distance { get; set; }
        public float Speed { get; set; }

        public float Cooldown
        {
            get => _cooldown;
            set
            {
                _cooldown = value;
                _currentCooldown = _cooldown;
            }
        }

        private readonly Transform _playerTransform;

        private bool _isReady;
        private bool _isActive;
        private float _cooldown;
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
            if (_isActive || !_isReady)
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
                if (!_isReady)
                {
                    UpdateCooldown(deltaTime);
                }
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
        private void UpdateCooldown(float deltaTime) 
        {
            _currentCooldown -= deltaTime;

            if (_currentCooldown <= 0.0f)
            {
                _isReady = true;
                ReadyChanged?.Invoke(this, _isReady);
            }
        }
        private void ResetCooldown()
        {
            _isReady = false;
            ReadyChanged?.Invoke(this, _isReady);
            _currentCooldown = Cooldown;
        }
        private Vector3 GetTargetPosition() => _startPosition + _playerTransform.forward * Distance;
    }
}