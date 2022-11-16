using Code.Views;

namespace Code.Services.Contracts
{
    public interface IHurdleViewFactory
    {
        HurdleView Create(float spawnRadius);
    }
}