using System.Collections.Generic;
using Code.Controllers;
using Code.Factories;
using Code.Infrastructure;
using Code.Services.Contracts;
using Code.Services.Implementations;
using UnityEngine;

namespace Code
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _viewTemplates;

        private GameStartController _gameStartController;
        
        private void Awake()
        {
            DiContainer.Instance.Register<IUpdateService>(gameObject.AddComponent<UpdateGameBehaviour>());
            DiContainer.Instance.Register<IUserInputService>(new UserInputService());
            DiContainer.Instance.Register<IViewService>(new ViewService(_viewTemplates.ToArray()));
            
            DiContainer.Instance.Register<IFollowToPositionControllerFactory>(new FollowToPositionControllerFactory());
            DiContainer.Instance.Register<IFollowToRotationControllerFactory>(new FollowToRotationControllerFactory());
            DiContainer.Instance.Register<IHurdleViewFactory>(new HurdleViewFactory());
            DiContainer.Instance.Register<ISpawnPointViewFactory>(new SpawnPointViewFactory());
            DiContainer.Instance.Register<IMapViewFactory>(new MapViewFactory());

            _gameStartController = new GameStartController();
        }

        private void Start() => _gameStartController.Start();
    }
}