using Ld50.Characters;
using Ld50.Interactable;

namespace Ld50.Hints
{
    public class TransportRequiredHint : HintOnApproach
    {
        public string transportName;

        private TransportController _transportController;
        private IBreakable _breakable;

        public void Start()
        {
            _transportController = FindObjectOfType<TransportController>();
            _breakable = GetComponentInParent<IBreakable>();
        }

        protected override bool InternalCondition() =>
            _breakable.IsBroken
            && (!_transportController.isInTransport || _transportController.currentTransport.name != transportName);
    }
}