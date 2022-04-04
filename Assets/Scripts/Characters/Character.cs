using System;
using Ld50.Animations;
using UniRx;
using UnityEngine;

namespace Ld50.Characters
{
    public class Character : MonoBehaviour
    {
        public float speed = 1f;

        public float jumpDuration = 0.5f;
        public float jumpHeight = 0.5f;

        public bool isAutoMoving = false;

        public SpritesLine idle, up, left, right, down, jump;

        private LineAnimator _animator;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;

        public bool isSliding = false;
        private Vector2 _slideDirection = Vector2.zero;
        private Vector2 _lastPos;
        public float slideFriction = 0.99f;
        public float slideSpeed = 0f;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();

            _lastPos = _rigidbody.position;
        }

        public void SetDirection(Vector2 d)
        {
            if (d.sqrMagnitude < 0.1)
            {
                SetDirection(Direction.None);
                return;
            }

            var angle = Mathf.Atan2(d.x, d.y) * Mathf.Rad2Deg;

            if (angle < 0)
                angle += 360f;


            SetDirection((Direction)Mathf.FloorToInt(angle / 90f));
        }

        public void SetDirection(Direction d) =>
            _animator.sprites = d switch {
                Direction.Up => up,
                Direction.Left => left,
                Direction.Right => right,
                Direction.Down => down,
                _ => idle
            };

        public void Slide()
        {
            if (!isSliding) return;

            _slideDirection = (_rigidbody.position - _lastPos).normalized;

            _rigidbody.MovePosition(
                _rigidbody.position + _slideDirection * slideSpeed * Time.fixedDeltaTime
            );
            slideSpeed *= slideFriction;
        }

        public void Move(Vector2 offset, float speedFactor = 1)
        {
            _lastPos = _rigidbody.position;

            _rigidbody.MovePosition(
                _rigidbody.position + offset.normalized * speedFactor * speed * Time.fixedDeltaTime
            );

            slideSpeed = speed;
        }

        public IObservable<Unit> MoveToPoint(Vector3 endPos)
        {
            isAutoMoving = true;

            var start = transform.position;
            var direction = endPos - start;

            SetDirection(direction);

            return Observable
                .EveryFixedUpdate()
                .TakeWhile(_ => Vector2.Distance(transform.position, endPos) > 0.1f)
                .Do(_ => Move(direction, 0.5f))
                .DoOnCompleted(
                    () =>
                    {
                        transform.position = endPos;
                        isAutoMoving = false;
                    }
                )
                .AsUnitObservable();
        }

        public IObservable<Unit> Jump(Vector3 endPos)
        {
            var oldSortLayer = _renderer.sortingOrder;

            _renderer.sortingOrder = oldSortLayer + 1;

            isAutoMoving = true;
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
                .DoOnCompleted(
                    () =>
                    {
                        isAutoMoving = false;
                        _renderer.sortingOrder = oldSortLayer; 
                    }
                )
                .AsUnitObservable();
        }

        private Vector3 JumpInterpolate(Vector3 a, Vector3 b, float t) =>
            Vector3.Lerp(a, b, t) + Vector3.up * jumpHeight * Mathf.Sin(t * Mathf.PI);
    }
}