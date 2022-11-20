using TMPro;
using UnityEngine;

namespace Code.Views
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PlayerScoreUiView : MonoBehaviour
    {
        [SerializeField] private string _prefix;

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                _textMeshPro.text = _prefix + Score;
            }
        }

        private int _score;
        private TextMeshProUGUI _textMeshPro;

        public void SetScore(int score)
        {
            Score = score;
        }
        
        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }
    }
}