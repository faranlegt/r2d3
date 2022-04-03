using Ld50.Characters;
using UnityEngine;

namespace Ld50.Interactable.Sockets
{
    public class EmptySocketAction : MonoBehaviour, ISocketAction
    {
        public void OnEntered(SocketController socketController) { }

        public void OnExited(SocketController socketController) { }

        public void Do(SocketController socketController) { }
    }
}