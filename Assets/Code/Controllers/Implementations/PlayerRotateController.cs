using System;
using Code.Infrastructure;
using Code.Views;
using UnityEngine;

namespace Code.Controllers.Implementations
{
    public class PlayerRotateController : IUpdatable
    {
        public event EventHandler<Quaternion> CalculatedRotationChanged;

        public bool IsActive { get; set; }
        
        private readonly CameraView _cameraView;

        public PlayerRotateController()
        {
            _cameraView = DiContainer.Instance.Resolve<CameraView>();
        }

        public void Update(float deltaTime)
        {
            if (!IsActive)
            {
                return;
            }
            
            CalculatedRotationChanged?.Invoke(this, CalculateRotation());
        }

        private Quaternion CalculateRotation()
        {
            return Quaternion.AngleAxis(_cameraView.transform.rotation.eulerAngles.y, Vector3.up);
        }
    }
}