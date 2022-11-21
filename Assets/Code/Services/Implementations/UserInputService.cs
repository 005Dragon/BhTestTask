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

        private bool _changingLookState;

        public UserInputService()
        {
            DiContainerRoot.Instance.Resolve<IUpdateService>().AddToUpdate(this);
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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _changingLookState = true;
                Debug.Log("Esc down");
            }

            if (Input.GetKeyUp(KeyCode.Escape) && _changingLookState)
            {
                _changingLookState = false;
                ChangeCursorLockState();
                Debug.Log("Esc up");
            }
        }

        private static void ChangeCursorLockState()
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked
                ? CursorLockMode.Confined
                : CursorLockMode.Locked;
        }
    }
}