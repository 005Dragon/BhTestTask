using Code.Infrastructure;

namespace Code.Contracts
{
    public interface IUpdateService
    {
        bool Updating { get; set; }
        
        void AddToUpdate(IUpdatable updatableObject);
        void RemoveFromUpdate(IUpdatable updatableObject);
    }
}