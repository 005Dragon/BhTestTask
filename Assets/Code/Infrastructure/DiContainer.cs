using System;
using System.Collections.Generic;

namespace Code.Infrastructure
{
    public class DiContainer
    {
        private readonly Dictionary<Type, object> _typeToInstanceIndex = new();
        private readonly DiContainer _parent;

        public DiContainer(DiContainer parent = null)
        {
            _parent = parent;
        }

        public void Register<TInstance>(TInstance instance)
        {
            Type instanceType = typeof(TInstance);

            if (_typeToInstanceIndex.ContainsKey(instanceType))
            {
                throw new InvalidOperationException($"Dependency of {instanceType} already registered.");
            }
            
            _typeToInstanceIndex[typeof(TInstance)] = instance;
        }

        public TInstance Resolve<TInstance>()
        {
            Type instanceType = typeof(TInstance);
            
            if (_typeToInstanceIndex.TryGetValue(instanceType, out object instance))
            {
                return (TInstance) instance;
            }

            if (_parent != null)
            {
                return _parent.Resolve<TInstance>();
            }

            throw new InvalidOperationException($"Dependency of {instanceType} not found.");
        }

        public void Clear() => _typeToInstanceIndex.Clear();
    }
}