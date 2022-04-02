using System;
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

        private void Awake()
        {
            _player = GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            var directionRounded = Direction.None;
            var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (direction.sqrMagnitude > 0.1)
            {
                var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

                if (angle < 0)
                    angle += 360f;


                directionRounded = (Direction)Mathf.FloorToInt(angle / 90f);

                _rigidbody.velocity = direction.normalized * speed;
            }
            else
            {
                _rigidbody.velocity = Vector2.zero;
            }

            _player.SetDirection(directionRounded);
        }

        private void OnDrawGizmos()
        {
            var pos = transform.position;
            var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            Gizmos.DrawLine(pos, pos + (Vector3)direction.normalized);
        }
    }
}