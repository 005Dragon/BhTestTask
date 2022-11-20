using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(menuName = "GameData/" + nameof(MaterialData), fileName = nameof(MaterialData))]
    public class MaterialData : ScriptableObject
    {
        [SerializeField] private MaterialKey _materialKey;
        [SerializeField] private Material _material;

        public MaterialKey MaterialKey => _materialKey;
        public Material Material => _material;
    }
}