using Ld50.Animations;
using Ld50.Characters;
using UnityEngine;

namespace Ld50.Transports
{
    public class WelderAction : MonoBehaviour, ITransportAction
    {
        public SpritesLine welding;
        private Character _character;
        private LineAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _character = GetComponent<Character>();
        }

        public void StartAction()
        {
            _animator.StartLine(welding, loop: true);
        }

        public void StopAction()
        {
            _animator.StartLine(_character.idle);
        }
    }
}