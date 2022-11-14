using System.Collections.Generic;
using Code.Contracts;
using Code.Controllers;
using Code.Factories;
using Code.Infrastructure;
using UnityEngine;

namespace Code
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _viewTemplates;
        
        private DiContainer _diContainer;

        private void Awake()
        {
            _diContainer = new DiContainer();

            _diContainer.Register<IStartGameService>(new StartGameService(_diContainer));
            _diContainer.Register<IUpdateService>(gameObject.AddComponent<UpdateGameBehaviour>());
            _diContainer.Register<IUserInputService>(new UserInput());
            _diContainer.Register<IViewService>(new ViewService(_viewTemplates.ToArray()));
            _diContainer.Register<IFactory<PlayerController>>(new PlayerFactory(_diContainer));
        }

        private void Start()
        {
            _diContainer.Resolve<IStartGameService>().Start();
        }
    }
}