using Code.Controllers.Implementations;
using UnityEngine;

namespace Code.Services.Contracts
{
    public interface IFollowToPositionControllerFactory
    {
        FollowToPositionController Create(Transform cachedTransform, float acceleration);
    }
}