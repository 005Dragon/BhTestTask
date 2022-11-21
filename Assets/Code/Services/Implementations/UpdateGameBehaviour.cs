using System.Collections.Generic;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;

namespace Code.Services.Implementations
{
    public class UpdateGameBehaviour : MonoBehaviour, IUpdateService
    {
        public bool Updating { get; set; } = true;

        private readonly List<IFixedUpdatable> _fixedUpdatableObjects = new();
        private readonly List<IUpdatable> _updatableObjects = new();

        public void AddToUpdate(IFixedUpdatable fixedUpdatableObject)
        {
            _fixedUpdatableObjects.Add(fixedUpdatableObject);
        }
        public void AddToUpdate(IUpdatable updatableObject)
        {
            _updatableObjects.Add(updatableObject);
        }
        public void RemoveFromUpdate(IFixedUpdatable fixedUpdatableObject)
        {
            _fixedUpdatableObjects.Remove(fixedUpdatableObject);
        }
        public void RemoveFromUpdate(IUpdatable updatableObject)
        {
            _updatableObjects.Remove(updatableObject);
        }

        public void Clear()
        {
            _fixedUpdatableObjects.Clear();
            _updatableObjects.Clear();
        }

        public void Update()
        {
            if (!Updating)
            {
                return;
            }

            IUpdatable[] updatableObjects = _updatableObjects.ToArray();

            float deltaTime = Time.deltaTime;

            foreach (IUpdatable updatableObject in updatableObjects)
            {
                updatableObject.Update(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (!Updating)
            {
                return;
            }
            
            IFixedUpdatable[] cachedUpdatableObjects = _fixedUpdatableObjects.ToArray();

            float deltaTime = Time.deltaTime;

            foreach (IFixedUpdatable updatable in cachedUpdatableObjects)
            {
                updatable.FixedUpdate(deltaTime);
            }
        }
    }
}