using System;
using Ld50.Characters;
using Ld50.Interactable.MiniGames;
using UnityEngine;

namespace Ld50.Interactable.Sockets
{
    public class Socket : MonoBehaviour
    {
        public GameObject breakable;

        public SocketController owner;

        private MiniGame _miniGame;
        private ISocketAction _action;
        private IBreakable _breakable;

        public void Awake()
        {
            _breakable = null;
            
            _action = GetComponent<ISocketAction>();
            breakable.TryGetComponent(out _breakable);

            TryGetComponent(out _miniGame);
        }

        public void PowerUp(SocketController socketController)
        {
            if (_miniGame && breakable && _breakable.IsBroken && owner)
                return;

            _action.Do(socketController);
        }

        public void PluggedIn(SocketController socketController)
        {
            owner = socketController;
            _action.OnEntered(socketController);
        }

        public void PluggedOut(SocketController socketController)
        {
            owner = null;
            _action.OnExited(socketController);

            if (_miniGame)
            {
                _miniGame.Cancel();
            }
        }

        private void Update()
        {
            if (_miniGame && breakable && _breakable.IsBroken && owner)
            {
                _miniGame.Launch(_breakable);
            }
        }
    }
}