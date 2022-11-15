using UnityEngine;

namespace Code.Services.Contracts
{
    public interface IViewService
    {
        TInstance FindTemplate<TInstance>()
            where TInstance : MonoBehaviour;
        
        TInstance Create<TInstance>()
            where TInstance : MonoBehaviour;
    }
}