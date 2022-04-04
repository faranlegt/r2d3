using Ld50.Characters;

namespace Ld50.Hints
{
    public class TransportHint : HintOnApproach
    {
        private TransportController _transportController;

        public void Start()
        {
            _transportController = FindObjectOfType<TransportController>();
        }
        
        protected override bool InternalCondition() => !_transportController.isInTransport;
    }
}