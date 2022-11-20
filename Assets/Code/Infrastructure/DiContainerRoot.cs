namespace Code.Infrastructure
{
    public class DiContainerRoot : DiContainer
    {
        public static DiContainerRoot Instance => _instance ??= new DiContainerRoot();
        private static DiContainerRoot _instance;
        
        private DiContainerRoot()
        {
        }
    }
}