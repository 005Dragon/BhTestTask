using Mirror;

namespace Code.NetworkMessages
{
    public struct PlayerDamageMessage : NetworkMessage
    {
        public uint[] PlayerNetIds;
    }
}