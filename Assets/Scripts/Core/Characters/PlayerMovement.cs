using Ld50.Animations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ld50.Core.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(TransportController))]
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 1f;

        private Vector2 _direction;

        private Character _player;
        private Rigidbody2D _rigidbody;
        private TransportController _transportController;


        private void Awake()
        {
            _player = GetComponent<Character>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _transportController = GetComponent<TransportController>();
        }

        private void Update()
        {
            if (_player.isAutoMoving || (_transportController.isInTransport && !_transportController.canMove))
                return;

            _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (_direction.sqrMagnitude <= 0.1)
            {
                _direction = Vector2.zero;
            }

            if (_transportController.isInTransport)
            {
                _transportController.currentTransport.SetDirection(_direction);
            }
            else
            {
                _player.SetDirection(_direction);
            }
        }

        private void FixedUpdate()
        {
            if ((_transportController.isInTransport && !_transportController.canMove) || _direction.sqrMagnitude < 0.1f)
                return;

            if (_transportController.isInTransport)
            {
                _transportController.currentTransport.Move(_direction);
            }
            else
            {
                _player.Move(_direction);
            }
        }
    }
}