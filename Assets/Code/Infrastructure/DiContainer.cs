using System;
using System.Collections.Generic;

namespace Code.Infrastructure
{
    public class DiContainer
    {
        public static DiContainer Instance => _instance ??= new DiContainer();
        private static DiContainer _instance;
        
        private readonly Dictionary<Type, object> _typeToInstanceIndex = new();

        private DiContainer()
        {
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

            throw new InvalidOperationException($"Dependency of {instanceType} not found.");
        }
    }
}