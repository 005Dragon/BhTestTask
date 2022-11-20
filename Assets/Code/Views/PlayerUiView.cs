using System;
using UnityEngine;

namespace Code.Views
{
    public class PlayerUiView : MonoBehaviour
    {
        [Header("Inner references")] 
        [SerializeField] private PlayerScoreUiView _playerScore;

        public void Initialize(PlayerView playerView)
        {
            playerView.EnemyDamaged += PlayerViewOnEnemyDamaged;
        }

        private void PlayerViewOnEnemyDamaged(object sender, int countDamagedPlayers)
        {
            _playerScore.SetScore(_playerScore.Score + countDamagedPlayers);
        }
    }
}