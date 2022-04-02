using System;
using UniRx;
using UnityEngine;

namespace Ld50.Core
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 1f;

        private Player _player;
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        private void Awake()
        {
            _player = GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            var directionRounded = Direction.None;
            _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (_direction.sqrMagnitude > 0.1)
            {
                var angle = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg;

                if (angle < 0)
                    angle += 360f;


                directionRounded = (Direction)Mathf.FloorToInt(angle / 90f);
            }
            else
            {
                _direction = Vector2.zero;
            }

            _player.SetDirection(directionRounded);
        }

        private void FixedUpdate()
        {
            if (_direction.sqrMagnitude < 0.1f) return;
            
            _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * speed * Time.fixedDeltaTime); 
        }
    }
}