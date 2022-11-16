using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Code.Services.Contracts;
using UnityEngine;

namespace Code.Services.Implementations
{
    public class MaterialService : IMaterialService
    {
        private readonly Dictionary<MaterialKey, Material> _keyToMaterialIndex;

        public MaterialService(MaterialStorage materialStorage)
        {
            _keyToMaterialIndex = materialStorage.Materials.ToDictionary(x => x.MaterialKey, x => x.Material);
        }

        public Material GetMaterial(MaterialKey materialKey) => _keyToMaterialIndex[materialKey];
    }
}