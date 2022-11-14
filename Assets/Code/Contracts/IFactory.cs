namespace Code.Contracts
{
    public interface IFactory<out TInstance>
    {
        TInstance Create();
    }
}