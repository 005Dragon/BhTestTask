using System;
using Code.Infrastructure;

namespace Code.Controllers
{
    public class CooldownController : IFixedUpdatable
    {
        public event EventHandler CooldownExpired;

        public float Cooldown
        {
            get => _maxCooldown;
            set
            {
                _maxCooldown = value;
                _currentCooldown = _maxCooldown;
            }
        }

        private bool _cooldownExpired;
        private float _maxCooldown;
        private float _currentCooldown;

        public void Start() => _cooldownExpired = false;

        public void Reset()
        {
            _currentCooldown = _maxCooldown;
            Start();
        }
        
        public void FixedUpdate(float deltaTime)
        {
            if (!_cooldownExpired)
            {
                UpdateCooldown(deltaTime);
            }
        }

        private void UpdateCooldown(float deltaTime)
        {
            _currentCooldown -= deltaTime;

            if (_currentCooldown <= 0.0f)
            {
                _currentCooldown = 0.0f;
                _cooldownExpired = true;
                CooldownExpired?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}