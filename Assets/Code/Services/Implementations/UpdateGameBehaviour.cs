using System.Collections.Generic;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;

namespace Code.Services.Implementations
{
    public class UpdateGameBehaviour : MonoBehaviour, IUpdateService
    {
        public bool Updating { get; set; }

        private readonly List<IUpdatable> _updatableObjects = new();

        public void AddToUpdate(IUpdatable updatableObject)
        {
            _updatableObjects.Add(updatableObject);
        }

        public void RemoveFromUpdate(IUpdatable updatableObject)
        {
            _updatableObjects.Remove(updatableObject);
        }

        private void Update()
        {
            if (!Updating)
            {
                return;
            }
            
            IUpdatable[] cachedUpdatableObjects = _updatableObjects.ToArray();

            foreach (IUpdatable updatable in cachedUpdatableObjects)
            {
                updatable.Update();
            }
        }
    }
}