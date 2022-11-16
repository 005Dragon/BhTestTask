using Code.Infrastructure;
using Code.Services.Contracts;
using Mirror;
using UnityEngine;

namespace Code.Views
{
    public class MapView : NetworkBehaviour
    {
        [field: SerializeField] public float SpawnHurdleRadius { get; set; }
        [field: SerializeField] public int CountHurdles { get; set; }
    }
}