using Code.Infrastructure;
using Code.Services.Contracts;
using Mirror;
using UnityEngine;

namespace Code.Views
{
    public class PlayerView : NetworkBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        
        private IUserInputService _userInputService;
        private CameraView _cameraView;
        private Transform _cachedTransform;

        private void Start()
        {
            if (isOwned)
            {
                _cachedTransform = transform;
                
                _cameraView = DiContainer.Instance.Resolve<IViewService>().Create<CameraView>();
                _cameraView.Target = transform;

                _userInputService = DiContainer.Instance.Resolve<IUserInputService>();
                _userInputService.ChangeCursorLockState();
            }
        }

        private void FixedUpdate()
        {
            if (isOwned)
            {
                _cameraView.RotateAroundTarget(_userInputService.RotateImpulseInput * Time.deltaTime);
                
                Vector3 moveImpulse =
                    _cachedTransform.forward * _userInputService.MoveInput.y * Time.deltaTime * _moveSpeed +
                    _cachedTransform.right * _userInputService.MoveInput.x * Time.deltaTime * _moveSpeed;

                _cachedTransform.position += moveImpulse;

                float moveImpulseMagnitude = moveImpulse.magnitude;
                
                if (moveImpulseMagnitude > 0.01f)
                {
                    Quaternion targetRotation =
                        Quaternion.AngleAxis(_cameraView.transform.rotation.eulerAngles.y, Vector3.up);

                    _cachedTransform.rotation = Quaternion.Lerp(
                        _cachedTransform.rotation,
                        targetRotation,
                        _rotationSpeed * moveImpulseMagnitude * Time.deltaTime
                    );
                }
            }
        }
    }
}