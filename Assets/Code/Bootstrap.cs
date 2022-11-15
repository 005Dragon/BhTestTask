using System;
using System.Collections.Generic;
using Code.Infrastructure;
using Code.NetworkMessages;
using Code.Services.Contracts;
using Code.Services.Implementations;
using Code.Views;
using Mirror;
using UnityEngine;

namespace Code
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _viewTemplates;
        
        private void Awake()
        {
            DiContainer.Instance.Register<IUpdateService>(gameObject.AddComponent<UpdateGameBehaviour>());
            DiContainer.Instance.Register<IUserInputService>(new UserInputService());
            DiContainer.Instance.Register<IViewService>(new ViewService(_viewTemplates.ToArray()));
        }

        private void Start()
        {
            var networkManagerView = DiContainer.Instance.Resolve<IViewService>().Create<NetworkManagerView>();
            
            networkManagerView.playerPrefab =
                DiContainer.Instance.Resolve<IViewService>().FindTemplate<PlayerView>().gameObject;
            networkManagerView.ClientConnected += NetworkManagerViewOnClientConnected;
        }

        private void NetworkManagerViewOnClientConnected(object sender, EventArgs eventArgs)
        {
            NetworkClient.Send(new CreatePlayerMessage());
        }
    }
}