using System;

namespace Code
{
    public class StartGameService : IStartGameService
    {
        private readonly DiContainer _diContainer;

        public StartGameService(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}