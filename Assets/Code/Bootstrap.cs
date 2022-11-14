using UnityEngine;

namespace Code
{
    public class Bootstrap : MonoBehaviour
    {
        private void Awake()
        {
            var diContainer = new DiContainer();

            diContainer.Register<IStartGameService>(new StartGameService(diContainer));
            diContainer.Register<IUpdateService>(gameObject.AddComponent<UpdateGameBehaviour>());
        }
    }
}