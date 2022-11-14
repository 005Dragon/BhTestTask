namespace Code
{
    public interface IUpdateService
    {
        bool Updating { get; set; }
        
        void AddToUpdate(IUpdatable updatableObject);
        void RemoveFromUpdate(IUpdatable updatableObject);
    }
}