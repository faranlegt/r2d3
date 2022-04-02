using System;
using Ld50.Animations;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ld50.Core.Characters
{
    public class Transport : MonoBehaviour
    {
        public Transform jumpTarget, exitJumpTarget;

        public SpritesLine launch, stop, waiting;

        public float speed = 2.5f;

        private Character _character;
        private LineAnimator _animator;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _character = GetComponent<Character>();
            _rigidbody = GetComponent<Rigidbody2D>();

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

        public void SetDirection(Direction d)
        {
            _character.SetDirection(d);
        }

        public void Move(Vector2 direction)
        {
            _rigidbody.MovePosition(_rigidbody.position + direction.normalized * speed * Time.fixedDeltaTime);
        }
    }
}