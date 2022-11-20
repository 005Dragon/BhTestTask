using System;
using System.Collections.Generic;
using Code.Views;
using UnityEngine;

namespace Code.Controllers
{
    public class PlayerSpurtCollisionController
    {
        public event EventHandler SelfDamaged;
        public event EventHandler Intersected;

        public int DamagedPlayers => _damagedPlayers.Count;

        private readonly HashSet<PlayerView> _damagedPlayers = new();

        public void CheckCollision(PlayerState selfState, Collision collision)
        {
            if (selfState == PlayerState.Disable)
            {
                return;
            }
            
            if (collision.gameObject.TryGetComponent(out PlayerView playerView))
            {
                if (playerView.State == PlayerState.Spurt)
                {
                    SelfDamaged?.Invoke(this, EventArgs.Empty);
                }

                if (selfState == PlayerState.Spurt && playerView.State != PlayerState.Disable)
                {
                    _damagedPlayers.Add(playerView);
                }
            }
            else if (selfState == PlayerState.Spurt)
            {
                Intersected?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ResetDamagedPlayers() => _damagedPlayers.Clear();
    }
}