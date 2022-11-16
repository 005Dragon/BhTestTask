using Code.Controllers;
using UnityEngine;

namespace Code.Services.Contracts
{
    public interface IFollowToRotationControllerFactory
    {
        FollowToRotationController Create(Transform cachedTransform, float acceleration);
    }
}