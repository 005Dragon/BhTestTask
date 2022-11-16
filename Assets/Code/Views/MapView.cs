using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Code.Views
{
    public class MapView : NetworkBehaviour
    {
        [field: SerializeField] public float SpawnPlayerRadius { get; set; }
        [field: SerializeField] public int CountSpawnPoints { get; set; }
        
        [field: SerializeField] public float SpawnHurdleRadius { get; set; }
        [field: SerializeField] public int CountHurdles { get; set; }

        private readonly List<SpawnPointView> _spawnPoints = new();

        public void AddSpawnPoint(SpawnPointView spawnPointView) => _spawnPoints.Add(spawnPointView);
        public SpawnPointView GetRandomSpawnPoint() => _spawnPoints[Random.Range(0, _spawnPoints.Count)];
    }
}