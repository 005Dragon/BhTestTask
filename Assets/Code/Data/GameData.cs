using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(menuName = "GameData/" + nameof(GameData), fileName = nameof(GameData))]
    public class GameData : ScriptableObject
    {
        [SerializeField] private int _scoreToVictory;

        public int ScoreToVictory => _scoreToVictory;
    }
}