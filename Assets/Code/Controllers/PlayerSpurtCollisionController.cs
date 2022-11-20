using System;
using Code.Views;
using UnityEngine;

namespace Code.Controllers
{
    public class PlayerSpurtCollisionController
    {
        public event EventHandler Damaged;
        public event EventHandler Intersected;

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
                    Damaged?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (selfState == PlayerState.Spurt)
            {
                Intersected?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}