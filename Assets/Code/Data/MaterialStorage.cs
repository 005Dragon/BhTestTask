using System.Collections.Generic;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(menuName = "GameData/"  + nameof(MaterialStorage), fileName = nameof(MaterialStorage))]
    public class MaterialStorage : ScriptableObject
    {
        [SerializeField] private List<MaterialData> _materials;

        public IReadOnlyList<MaterialData> Materials => _materials;
    }
}