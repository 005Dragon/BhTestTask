using System;
using System.Collections.Generic;
using System.Linq;
using Code.Views;
using Mirror;
using UnityEngine;

namespace Code.Controllers
{
    public class PlayerSpurtCollisionController
    {
        public event EventHandler Intersected;

        private readonly HashSet<uint> _damagedPlayerNetIds = new();

        public uint[] GetDamagedPlayerIds() => _damagedPlayerNetIds.ToArray();
        
        public void CheckCollision(PlayerState selfState, Collision collision)
        {
            if (selfState == PlayerState.Disable)
            {
                return;
            }

            if (selfState == PlayerState.Spurt)
            {
                if (collision.gameObject.TryGetComponent(out PlayerView playerView))
                {
                    if (playerView.State != PlayerState.Disable)
                    {
                        _damagedPlayerNetIds.Add(playerView.GetComponent<NetworkIdentity>().netId);
                    }
                }
                else
                {
                    Intersected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void ResetDamagedPlayers() => _damagedPlayerNetIds.Clear();
    }
}