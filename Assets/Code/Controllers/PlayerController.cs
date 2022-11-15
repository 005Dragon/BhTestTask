using Code.Infrastructure;
using Code.Models;
using Code.Views;
using UnityEngine;

namespace Code.Controllers
{
    public class PlayerController : IUpdatable
    {
        public PlayerView View { get; }
        
        private readonly PlayerModel _model;
        private readonly Transform _cachedViewTransform;

        public PlayerController(PlayerModel model, PlayerView view)
        {
            _model = model;
            View = view;
            _cachedViewTransform = view.transform;
            
            UpdateModelFromView();
        }

        public void Update()
        {
            UpdateViewFromModel();
        }

        private void UpdateModelFromView()
        {
            Vector3 viewTransformPosition = _cachedViewTransform.position;
            _model.Position = new Vector2(viewTransformPosition.x, viewTransformPosition.z);

            _model.DirectionAngle = _cachedViewTransform.rotation.eulerAngles.y;
        }

        private void UpdateViewFromModel()
        {
            _cachedViewTransform.position =
                new Vector3(_model.Position.x, _cachedViewTransform.position.y, _model.Position.y);
            
            _cachedViewTransform.rotation = Quaternion.AngleAxis(_model.DirectionAngle, Vector3.up);
        }
    }
}