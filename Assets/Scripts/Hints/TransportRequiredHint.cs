using Ld50.Characters;
using Ld50.Interactable;

namespace Ld50.Hints
{
    public class TransportRequiredHint : HintOnApproach
    {
        public string transportName;

        private TransportController _transportController;

        public void Start()
        {
            _transportController = FindObjectOfType<TransportController>();
        }

        protected override bool InternalCondition() =>
            !_transportController.isInTransport
            || !_transportController.currentTransport
            || _transportController.currentTransport.name != transportName;
    }
}