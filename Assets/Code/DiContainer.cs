using System;
using System.Collections.Generic;

namespace Code
{
    public class DiContainer
    {
        private readonly DiContainer _parent;
        private readonly Dictionary<Type, object> _typeToInstanceIndex = new();

        public DiContainer(DiContainer parent = null)
        {
            _parent = parent;
        }

        public void Register<T>(T instance)
        {
            Type instanceType = typeof(T);

            if (_typeToInstanceIndex.ContainsKey(instanceType))
            {
                throw new InvalidOperationException($"Dependency of {instanceType} already registered.");
            }
            
            _typeToInstanceIndex[typeof(T)] = instance;
        }

        public T Resolve<T>()
        {
            Type instanceType = typeof(T);
            
            if (_typeToInstanceIndex.TryGetValue(instanceType, out object instance))
            {
                return (T) instance;
            }

            if (_parent != null)
            {
                return _parent.Resolve<T>();
            }

            throw new InvalidOperationException($"Dependency of {instanceType} not found.");
        }
    }
}