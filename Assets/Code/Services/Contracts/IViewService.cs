using UnityEngine;

namespace Code.Services.Contracts
{
    public interface IViewService
    {
        TInstance FindTemplate<TInstance>()
            where TInstance : MonoBehaviour;
        
        TInstance Create<TInstance>(bool registerInDiContainer = true)
            where TInstance : MonoBehaviour;
    }
}