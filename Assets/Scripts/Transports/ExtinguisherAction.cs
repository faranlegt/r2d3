using Ld50.Animations;
using Ld50.Characters;
using UnityEngine;

namespace Ld50.Transports
{
    public class ExtinguisherAction : MonoBehaviour, ITransportAction
    {
        public SpritesLine extinguish;
        private Character _character;
        private LineAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _character = GetComponent<Character>();
        }

        public void StartAction()
        {
            _animator.StartLine(extinguish, loop: true);
        }

        public void StopAction()
        {
            _animator.StartLine(_character.idle);
        }
    }
}