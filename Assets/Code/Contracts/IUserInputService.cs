using System;
using UnityEngine;

namespace Code.Contracts
{
    public interface IUserInputService
    {
        event EventHandler MainAction;
        Vector2 RotateImpulseInput { get; }
        Vector2 MoveInput { get; }
    }
}