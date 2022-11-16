using Code.Data;
using UnityEngine;

namespace Code.Services.Contracts
{
    public interface IMaterialService
    {
        Material GetMaterial(MaterialKey materialKey);
    }
}