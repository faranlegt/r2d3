using Ld50.Animations;

namespace Ld50.Core.Characters.Transports
{
    public class Extinguisher : Transport
    {
        public SpritesLine extinguish;

        protected override void StartAction()
        {
            Animator.StartLine(extinguish, loop: true);
        }

        protected override void StopAction()
        {
            Animator.StartLine(Character.idle);
        }
    }
}