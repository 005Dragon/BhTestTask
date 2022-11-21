using System.Diagnostics.CodeAnalysis;
using Mirror;
using TMPro;
using UnityEngine;

namespace Code.Views
{
    public class PlayerWinnerUi : NetworkBehaviour
    {
        [SerializeField] private string _textTemplate;        
        
        [Header("Inner references")] 
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        [field: SyncVar(hook = nameof(SyncPlayerName))]
        public string PlayerName { get; set; }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void SyncPlayerName(string oldValue, string newValue)
        {
            _textMeshPro.text = string.Format(_textTemplate, PlayerName);
        }
    }
}