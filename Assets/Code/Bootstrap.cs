using System.Collections.Generic;
using System.Linq;
using Code.Controllers;
using Code.Data;
using Code.Factories;
using Code.Infrastructure;
using Code.Services.Contracts;
using Code.Services.Implementations;
using UnityEngine;

namespace Code
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> _storages;
        [SerializeField] private List<GameObject> _viewTemplates;

        private GameStartController _gameStartController;
        
        private void Awake()
        {
            DiContainer.Instance.Register<IMaterialService>(
                new MaterialService(_storages.OfType<MaterialStorage>().First())
            );
            
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