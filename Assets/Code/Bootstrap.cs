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
        [SerializeField] private GameData _gameData;
        [SerializeField] private List<ScriptableObject> _storages;
        [SerializeField] private List<GameObject> _viewTemplates;

        private GameStartController _gameStartController;
        
        private void Awake()
        {
            DiContainerRoot.Instance.Register(_gameData);
            DiContainerRoot.Instance.Register<IMaterialService>(
                new MaterialService(_storages.OfType<MaterialStorage>().First())
            );
            
            DiContainerRoot.Instance.Register<IUpdateService>(gameObject.AddComponent<UpdateGameBehaviour>());
            DiContainerRoot.Instance.Register<IUserInputService>(new UserInputService());
            DiContainerRoot.Instance.Register<IViewService>(new ViewService(_viewTemplates.ToArray()));
            
            DiContainerRoot.Instance.Register<IHurdleViewFactory>(new HurdleViewFactory());
            DiContainerRoot.Instance.Register<ISpawnPointViewFactory>(new SpawnPointViewFactory());
            DiContainerRoot.Instance.Register<IMapViewFactory>(new MapViewFactory());

            _gameStartController = new GameStartController();
        }

        private void Start() => _gameStartController.Start();
    }
}