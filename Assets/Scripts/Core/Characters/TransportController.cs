using System;
using Ld50.Animations;
using UniRx;
using UniRx.Triggers;
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

        private void Start()
        {
            this.OnTriggerStay2DAsObservable()
                .Where(
                    c => c.gameObject.CompareTag("Transport")
                         && Input.GetKey(KeyCode.Z)
                         && !isInTransport
                )
                .Do(c => Enter(c.gameObject.GetComponent<Transport>()))
                .Subscribe()
                .AddTo(this);
        }

        private void Update()
        {
            if (!canMove)
                return;

            transform.position = currentTransport.transform.position;

            if (Input.GetKey(KeyCode.X))
            {
                Exit();
            }
        }

        public void Enter(Transport transport)
        {
            isInTransport = true;
            _collider.enabled = false;

            Jump(transport.jumpTarget.position)
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
                                }
                            )
                            .Subscribe()
                            .AddTo(this);
                    }
                )
                .Subscribe()
                .AddTo(this);
        }

        public void Exit()
        {
            var targetPos = currentTransport.exitJumpTarget.position;

            currentTransport
                .Stop()
                .Do(
                    _ =>
                    {
                        _collider.enabled = true;
                        _renderer.enabled = true;

                        Jump(targetPos)
                            .DoOnCompleted(
                                () =>
                                {
                                    _animator.animate = true;
                                    _animator.loop = true;
                                    
                                    isInTransport = false;
                                }
                            )
                            .Subscribe()
                            .AddTo(this);
                    }
                )
                .Subscribe()
                .AddTo(this);

            currentTransport = null;
            canMove = false;
        }

        private IObservable<Unit> Jump(Vector3 endPos)
        {
            _animator.StartLine(jump, loop: false);

            var startPos = transform.position;
            var t = 0f;

            return Observable
                .EveryUpdate()
                .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(jumpDuration)))
                .Do(
                    _ =>
                    {
                        t += Time.deltaTime / jumpDuration;
                        transform.position = JumpInterpolate(startPos, endPos, t);
                    }
                )
                .AsUnitObservable();
        }

        private Vector3 JumpInterpolate(Vector3 a, Vector3 b, float t) =>
            Vector3.Lerp(a, b, t) + Vector3.up * jumpHeight * Mathf.Sin(t * Mathf.PI);
    }
}