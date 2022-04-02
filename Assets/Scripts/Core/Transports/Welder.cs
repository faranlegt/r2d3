using Ld50.Animations;

namespace Ld50.Core.Characters.Transports
{
    public class Welder : Transport
    {
        public SpritesLine welding;

        protected override void StartAction()
        {
            Animator.StartLine(welding, loop: true);
        }

        protected override void StopAction()
        {
            Animator.StartLine(Character.idle);
        }
    }
}