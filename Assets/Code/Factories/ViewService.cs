using System.Linq;
using Code.Contracts;
using UnityEngine;

namespace Code.Factories
{
    public class ViewService : IViewService
    {
        private readonly GameObject[] _viewTemplates;

        public ViewService(GameObject[] viewTemplates)
        {
            _viewTemplates = viewTemplates;
        }

        public TInstance Create<TInstance>()
            where TInstance : MonoBehaviour
        {
            GameObject viewTemplate = _viewTemplates.Single(x => x.TryGetComponent(out TInstance _));
            
            GameObject gameObject = Object.Instantiate(viewTemplate);

            return gameObject.GetComponent<TInstance>();
        }
    }
}