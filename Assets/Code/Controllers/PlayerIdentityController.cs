using System.Collections.Generic;

namespace Code.Controllers
{
    public class PlayerIdentityController
    {
        private readonly Dictionary<int, int> _connectionToPlayerIdIndex = new();

        private int _freeId = 1;

        public int GetId(int connectionId)
        {
            if (!_connectionToPlayerIdIndex.TryGetValue(connectionId, out int id))
            {
                id = _freeId++;
                _connectionToPlayerIdIndex[connectionId] = id;
            }

            return id;
        }
    }
}