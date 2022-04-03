using Ld50.Characters;
using UnityEngine;

namespace Ld50.Interactable.Sockets
{
    public class Socket : MonoBehaviour
    {
        private ISocketAction _action;

        public void Awake() => _action = GetComponent<ISocketAction>();

        public void PowerUp(SocketController socketController) => _action.Do(socketController);

        public void PluggedIn(SocketController socketController) => _action.OnEntered(socketController);

        public void PluggedOut(SocketController socketController) => _action.OnExited(socketController);
    }
}