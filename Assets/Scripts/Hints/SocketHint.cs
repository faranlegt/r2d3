using Ld50.Characters;

namespace Ld50.Hints
{
    public class SocketHint : HintOnApproach
    {
        public Ld50.Interactable.Sockets.Socket socket;


        protected override bool InternalCondition() => !socket.owner;
    }
}