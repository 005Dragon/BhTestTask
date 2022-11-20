using System;
using Code.Infrastructure;
using Code.Services.Contracts;
using UnityEngine;

namespace Code.Services.Implementations
{
    public class UserInputService : IUserInputService, IUpdatable
    {
        public event EventHandler MainAction;
        public Vector2 RotateImpulseInput { get; private set; }
        public Vector2 MoveInput { get; private set; }

        public UserInputService()
        {
            DiContainerRoot.Instance.Resolve<IUpdateService>().AddToUpdate(this);
        }

        public void ChangeCursorLockState()
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked
                ? CursorLockMode.Confined
                : CursorLockMode.Locked;
        }

        public void Update(float deltaTime)
        {
            if (Input.GetMouseButton(0))
            {
                MainAction?.Invoke(this, EventArgs.Empty);
            }

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                RotateImpulseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
            
            MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            if (Input.GetKey(KeyCode.Escape))
            {
                ChangeCursorLockState();
            }
        }
    }
}