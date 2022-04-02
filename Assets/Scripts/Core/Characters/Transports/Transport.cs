using System;
using Ld50.Animations;
using UniRx;
using UnityEngine;

namespace Ld50.Core.Characters.Transports
{
    public abstract class Transport : MonoBehaviour
    {
        public Transform jumpTarget, exitJumpTarget;

        public SpritesLine launch, stop, waiting;

        public bool isActing;

        protected Character Character;
        protected LineAnimator Animator;
        protected Rigidbody2D Rigidbody;

        private void Awake()
        {
            Animator = GetComponent<LineAnimator>();
            Character = GetComponent<Character>();
            Rigidbody = GetComponent<Rigidbody2D>();

            Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public IObservable<Unit> Launch()
        {
            Animator.StartLine(launch, loop: false);

            return Animator
                .OnAnimationEnd
                .Take(1)
                .Do(
                    _ =>
                    {
                        Animator.StartLine(launch, loop: true);

                        Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                );
        }

        public IObservable<Unit> Stop()
        {
            Animator.StartLine(stop, loop: false);
            Animator.loop = false;

            Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

            return Animator
                .OnAnimationEnd
                .Take(1)
                .Do(_ => Animator.StartLine(waiting, loop: true));
        }

        public void SetDirection(Vector2 d)
        {
            if (isActing)
                return;

            Character.SetDirection(d);
        }

        public void Move(Vector2 direction)
        {
            if (isActing)
                return;

            Character.Move(direction);
        }

        public void Action()
        {
            if (isActing) return;
            
            isActing = true;

            StartAction();
        }

        public void NoAction()
        {
            if (!isActing) return;
            
            isActing = false;

            StopAction();
        }

        protected abstract void StopAction();

        protected abstract void StartAction();
    }
}