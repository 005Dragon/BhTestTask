using Code.Controllers;
using UnityEngine;

namespace Code.Services.Contracts
{
    public interface IFollowToPositionControllerFactory
    {
        FollowToPositionController Create(Transform cachedTransform, float acceleration);
    }
}