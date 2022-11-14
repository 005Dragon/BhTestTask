using System;
using UnityEngine;

namespace Code.Models
{
    public class TransformModel
    {
        public event EventHandler PositionChanged;
        public event EventHandler RotationChanged;
        public event EventHandler ScaleChanged;

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                PositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                RotationChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                ScaleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private Vector3 _position;
        private Quaternion _rotation;
        private Vector3 _scale;
    }
}