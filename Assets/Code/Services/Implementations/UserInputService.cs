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
        
        public void Update()
        {
            if (Input.GetKey(0))
            {
                MainAction?.Invoke(this, EventArgs.Empty);
            }

            RotateImpulseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}