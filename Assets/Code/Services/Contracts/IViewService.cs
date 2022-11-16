using System.Collections.Generic;
using UnityEngine;

namespace Code.Services.Contracts
{
    public interface IViewService
    {
        TInstance FindSingleTemplate<TInstance>()
            where TInstance : MonoBehaviour;

        IEnumerable<TInstance> FindTemplates<TInstance>()
            where TInstance : MonoBehaviour;
        
        TInstance Create<TInstance>(bool registerInDiContainer = true)
            where TInstance : MonoBehaviour;
    }
}