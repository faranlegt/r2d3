using System;
using Ld50.Animations;
using UniRx;
using UnityEngine;

namespace Ld50.Core.Characters
{
    public class TransportController : MonoBehaviour
    {
        public bool isInTransport;
        public bool canMove;
        
        public float jumpDuration = 1f;
        public float jumpHeight = 0.5f;

        public SpritesLine jump;

        public Transport currentTransport;

        private LineAnimator _animator;
        private SpriteRenderer _renderer;
        private Collider2D _collider;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        public void Enter(Transport transport)
        {
            isInTransport = true;

            _collider.enabled = false;
            
            _animator.sprites = jump;
            _animator.loop = false;
            _animator.animationFrame = 0;

            var startPos = transform.position;
            var endPos = transport.jumpTarget.position;
            var t = 0f;

            Observable
                .EveryUpdate()
                .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(jumpDuration)))
                .Do(
                    _ =>
                    {
                        t += Time.deltaTime / jumpDuration;
                        transform.position = JumpInterpolate(startPos, endPos, t);
                    }
                )
                .DoOnCompleted(
                    () =>
                    {
                        _renderer.enabled = false;
                        transport
                            .Launch()
                            .Do(
                                _ =>
                                {
                                    canMove = true;
                                    currentTransport = transport;
                                })
                            .Subscribe()
                            .AddTo(this);

                    }
                )
                .Subscribe()
                .AddTo(this);
        }

        private void Update()
        {
            if (canMove)
            {
                transform.position = currentTransport.transform.position;
            }
        }

        private Vector3 JumpInterpolate(Vector3 a, Vector3 b, float t) =>
            Vector3.Lerp(a, b, t) + Vector3.up * jumpHeight * Mathf.Sin(t * Mathf.PI);
    }
}