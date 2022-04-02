using System;
using Ld50.Animations;
using UniRx;
using UnityEngine;

namespace Ld50.Core.Characters
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

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetDirection(Vector2 d)
        {
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

        public void Move(Vector2 offset)
        {
            _rigidbody.MovePosition(_rigidbody.position + offset.normalized * speed * Time.fixedDeltaTime);
        }

        public IObservable<Unit> MoveToPoint(Vector3 endPos)
        {
            isAutoMoving = true;
            
            var start = transform.position;
            var direction = endPos - start;
            SetDirection(direction);

            return Observable
                .EveryUpdate()
                .TakeWhile(_ => Vector2.Distance(transform.position, endPos) > 0.1f)
                .Do(_ => Move(direction))
                .DoOnCompleted(
                    () =>
                    {
                        transform.position = endPos;
                        isAutoMoving = false;
                    })
                .AsUnitObservable();
        }

        public IObservable<Unit> Jump(Vector3 endPos)
        {
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
                .DoOnCompleted(() => isAutoMoving = false)
                .AsUnitObservable();
        }

        private Vector3 JumpInterpolate(Vector3 a, Vector3 b, float t) =>
            Vector3.Lerp(a, b, t) + Vector3.up * jumpHeight * Mathf.Sin(t * Mathf.PI);
    }
}