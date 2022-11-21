using Code.Infrastructure;

namespace Code.Services.Contracts
{
    public interface IUpdateService
    {
        bool Updating { get; set; }
        
        void AddToUpdate(IFixedUpdatable fixedUpdatableObject);
        void AddToUpdate(IUpdatable updatableObject);
        void RemoveFromUpdate(IFixedUpdatable fixedUpdatableObject);
        void RemoveFromUpdate(IUpdatable updatableObject);
        void Clear();
    }
}