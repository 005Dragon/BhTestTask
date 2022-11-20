using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(menuName = "GameData/" + nameof(PlayerData), fileName = nameof(PlayerData))]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private float _disableCooldown;
        
        [Header("Simple movement")] 
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _moveAcceleration;
        [SerializeField] private float _rotationAcceleration;

        [Header("Spurt movement")] 
        [SerializeField] private float _spurtDistance;
        [SerializeField] private float _spurtSpeed;
        [SerializeField] private float _spurtCooldown;

        public float DisableCooldown => _disableCooldown;
        public float MoveSpeed => _moveSpeed;
        public float MoveAcceleration => _moveAcceleration;
        public float RotationAcceleration => _rotationAcceleration;
        public float SpurtDistance => _spurtDistance;
        public float SpurtSpeed => _spurtSpeed;
        public float SpurtCooldown => _spurtCooldown;
    }
}