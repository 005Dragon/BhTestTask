using Mirror;

namespace Code.NetworkMessages
{
    public struct WinnerPlayerMessage : NetworkMessage
    {
        public string PlayerName;
    }
}