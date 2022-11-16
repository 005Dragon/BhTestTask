using System;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Services.Implementations
{
    public class ViewService : IViewService
    {
        private readonly GameObject[] _viewTemplates;

        public ViewService(GameObject[] viewTemplates)
        {
            _viewTemplates = viewTemplates;
        }

        public TInstance FindTemplate<TInstance>() 
            where TInstance : MonoBehaviour
        {
            TInstance result = null;
            
            foreach (GameObject gameObject in _viewTemplates)
            {
                if (gameObject.TryGetComponent(out TInstance instance))
                {
                    if (result != null)
                    {
                        throw new InvalidOperationException("Found more than one element of type.");
                    }
                    
                    result = instance;
                }
            }

            if (result == null)
            {
                throw new InvalidOperationException("Not found element of type.");
            }

            return result;
        }

        public TInstance Create<TInstance>(bool registerInDiContainer)
            where TInstance : MonoBehaviour
        {
            TInstance instance = Object.Instantiate(FindTemplate<TInstance>());

            if (registerInDiContainer)
            {
                DiContainer.Instance.Register(instance);
            }

            return instance;
        }
    }
}