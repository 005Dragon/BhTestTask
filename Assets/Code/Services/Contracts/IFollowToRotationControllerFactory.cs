using Code.Controllers.Implementations;
using UnityEngine;

namespace Code.Services.Contracts
{
    public interface IFollowToRotationControllerFactory
    {
        FollowToRotationController Create(Transform cachedTransform, float acceleration);
    }
}