using System;
using Code.Infrastructure;

namespace Code.Controllers
{
    public class PlayerStateController : IUpdatable
    {
        public event EventHandler<PlayerState> StateChanged;

        public float DisableCooldown
        {
            get => _disableCooldownController.Cooldown;
            set => _disableCooldownController.Cooldown = value;
        }
        public float SpurtCooldown
        {
            get => _spurtCooldownController.Cooldown;
            set => _spurtCooldownController.Cooldown = value;
        }

        private bool _isActive = true;
        private readonly CooldownController _disableCooldownController = new();
        
        private bool _isSpurtActive;
        private bool _isSpurtReady;
        private readonly CooldownController _spurtCooldownController = new();

        public PlayerStateController()
        {
            _disableCooldownController.CooldownExpired += DisableCooldownControllerOnCooldownExpired;
            _spurtCooldownController.CooldownExpired += SpurtCooldownControllerOnCooldownExpired;
        }

        public bool TryChangeStateToDisableSpurt()
        {
            if (!_isSpurtActive)
            {
                return false;
            }

            _isSpurtActive = false;
            _isSpurtReady = false;
            _spurtCooldownController.Reset();
            
            StateChanged?.Invoke(this, _isActive ? PlayerState.Default : PlayerState.Disable);

            return true;
        }

        public bool TryChangeStateToActiveSpurt()
        {
            if (!_isActive || _isSpurtActive || !_isSpurtReady)
            {
                return false;
            }
            
            _isSpurtActive = true;
            StateChanged?.Invoke(this, PlayerState.Spurt);
            
            return true;
        }

        public bool TryChangeStateToDisable()
        {
            if (_isSpurtActive && !_isActive)
            {
                return false;
            }

            _disableCooldownController.Reset();
            _isActive = false;
            
            StateChanged?.Invoke(this, PlayerState.Disable);
            
            return true;
        }

        public void Update(float deltaTime)
        {
            _disableCooldownController.Update(deltaTime);
            _spurtCooldownController.Update(deltaTime);
        }

        private void DisableCooldownControllerOnCooldownExpired(object sender, EventArgs eventArgs)
        {
            _isActive = true;
            StateChanged?.Invoke(this, _isSpurtReady ? PlayerState.SpurtReady : PlayerState.Default);
        }

        private void SpurtCooldownControllerOnCooldownExpired(object sender, EventArgs eventArgs)
        {
            _isSpurtReady = true;
            StateChanged?.Invoke(this, _isActive ? PlayerState.SpurtReady : PlayerState.Disable);
        }
    }
}