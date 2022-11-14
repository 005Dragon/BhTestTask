using UnityEngine;

namespace Code.Contracts
{
    public interface IViewService
    {
        TInstance Create<TInstance>()
            where TInstance : MonoBehaviour;
    }
}