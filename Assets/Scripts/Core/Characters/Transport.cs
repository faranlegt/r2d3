using System;
using Ld50.Animations;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ld50.Core.Characters
{
    public class Transport : MonoBehaviour
    {
        public Transform jumpTarget;

        public SpritesLine launch;

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
            _animator.sprites = launch;
            _animator.animationFrame = 0;
            _animator.loop = false;
            _animator.animate = true;

            return _animator
                .OnAnimationEnd
                .Take(1)
                .Do(
                    _ =>
                    {
                        _animator.animate = true;
                        _animator.loop = true;
                        _animator.animationFrame = 0;

                        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                        
                        _character.SetDirection(Direction.None);
                    }
                );
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