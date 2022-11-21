using System;
using UnityEngine;

namespace Code.Services.Contracts
{
    public interface IUserInputService
    {
        event EventHandler MainAction;
        Vector2 RotateImpulseInput { get; }
        Vector2 MoveInput { get; }

        void ChangeCursorLockState();
        void ChangeCursorLockState(CursorLockMode cursorLockMode);
    }
}