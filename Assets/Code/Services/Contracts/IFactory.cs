namespace Code.Services.Contracts
{
    public interface IFactory<out TInstance>
    {
        TInstance Create();
    }
}