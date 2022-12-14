using Code.Data;
using Code.Infrastructure;
using Code.NetworkMessages;
using Mirror;
using TMPro;
using UnityEngine;

namespace Code.Views
{
    public class PlayerUiView : MonoBehaviour
    {
        [Header("Inner references")] 
        [SerializeField] private TextMeshProUGUI _playerNameTextMeshPro;
        [SerializeField] private PlayerScoreUiView _playerScore;

        private string _playerName;

        public void Initialize(PlayerView playerView)
        {
            playerView.EnemyDamaged += PlayerViewOnEnemyDamaged;
            _playerName = playerView.name;
            _playerNameTextMeshPro.text = _playerName;
        }

        private void PlayerViewOnEnemyDamaged(object sender, int countDamagedPlayers)
        {
            _playerScore.SetScore(_playerScore.Score + countDamagedPlayers);

            if (_playerScore.Score >= DiContainerRoot.Instance.Resolve<GameData>().ScoreToVictory)
            {
                NetworkClient.Send(new WinnerPlayerMessage { PlayerName = _playerName});
            }
        }
    }
}