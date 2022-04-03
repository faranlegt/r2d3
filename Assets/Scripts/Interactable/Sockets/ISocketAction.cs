using Ld50.Characters;

namespace Ld50.Interactable.Sockets
{
    public interface ISocketAction
    {
        void OnEntered(SocketController socketController);
        
        void OnExited(SocketController socketController);
        
        void Do(SocketController socketController);
    }
}