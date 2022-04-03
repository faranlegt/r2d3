using System;
using Ld50.Animations;
using Ld50.Characters;
using UniRx;
using UnityEngine;

namespace Ld50.Transports
{
    public class Transport : MonoBehaviour
    {
        public Transform jumpTarget, exitJumpTarget;

        public SpritesLine launch, stop, waiting;

        public bool isActing;

        private Character _character;
        private LineAnimator _animator;
        private Rigidbody2D _rigidbody;
        private ITransportAction _action;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _character = GetComponent<Character>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _action = GetComponent<ITransportAction>();

            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public IObservable<Unit> Launch()
        {
            _animator.StartLine(launch, loop: false);

            return _animator
                .OnAnimationEnd
                .Take(1)
                .Do(
                    _ =>
                    {
                        _animator.StartLine(launch, loop: true);

                        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                );
        }

        public IObservable<Unit> Stop()
        {
            _animator.StartLine(stop, loop: false);
            _animator.loop = false;

            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

            return _animator
                .OnAnimationEnd
                .Take(1)
                .Do(_ => _animator.StartLine(waiting, loop: true));
        }

        public void SetDirection(Vector2 d)
        {
            if (isActing)
                return;

            _character.SetDirection(d);
        }

        public void Move(Vector2 direction)
        {
            if (isActing)
                return;

            _character.Move(direction);
        }

        public void Action()
        {
            if (isActing) return;
            
            isActing = true;

            _action.StartAction();
        }

        public void NoAction()
        {
            if (!isActing) return;
            
            isActing = false;

            _action.StopAction();
        }
    }
}