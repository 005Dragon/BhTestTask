using System;
using System.Collections.Generic;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code.Services.Implementations
{
    public class ViewService : IViewService
    {
        private readonly GameObject[] _viewTemplates;

        public ViewService(GameObject[] viewTemplates)
        {
            _viewTemplates = viewTemplates;
        }

        public TInstance FindSingleTemplate<TInstance>() 
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
        
        public IEnumerable<TInstance> FindTemplates<TInstance>() 
            where TInstance : MonoBehaviour
        {
            foreach (GameObject viewTemplate in _viewTemplates)
            {
                if (viewTemplate.TryGetComponent(out TInstance instance))
                {
                    yield return instance;
                }
            }
        }

        public TInstance Create<TInstance>(bool registerInDiContainer)
            where TInstance : MonoBehaviour
        {
            TInstance instance = Object.Instantiate(FindSingleTemplate<TInstance>());

            if (registerInDiContainer)
            {
                DiContainerRoot.Instance.Register(instance);
            }

            return instance;
        }

        public TInstance CreateRandomAnyOfType<TInstance>() 
            where TInstance : MonoBehaviour
        {
            List<TInstance> templates = new();

            foreach (GameObject viewTemplate in _viewTemplates)
            {
                if (viewTemplate.TryGetComponent(out TInstance instance))
                {
                    templates.Add(instance);
                }
            }

            int templateIndex = Random.Range(0, templates.Count);
            
            return Object.Instantiate(templates[templateIndex]);
        }
    }
}