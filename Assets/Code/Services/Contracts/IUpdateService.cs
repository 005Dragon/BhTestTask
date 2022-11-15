using Code.Infrastructure;

namespace Code.Services.Contracts
{
    public interface IUpdateService
    {
        bool Updating { get; set; }
        
        void AddToUpdate(IUpdatable updatableObject);
        void RemoveFromUpdate(IUpdatable updatableObject);
    }
}