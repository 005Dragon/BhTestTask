using Code.Views;

namespace Code.Services.Contracts
{
    public interface ISpawnPointViewFactory
    {
        SpawnPointView Create(float spawnRadius);
    }
}