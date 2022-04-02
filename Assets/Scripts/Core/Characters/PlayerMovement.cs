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

        private void Start()
        {
            this.OnTriggerStay2DAsObservable()
                .Where(
                    c => c.gameObject.CompareTag("Transport")
                         && Input.GetKey(KeyCode.Z)
                         && !_transportController.isInTransport
                )
                .Do(c => _transportController.Enter(c.gameObject.GetComponent<Transport>()))
                .Subscribe()
                .AddTo(this);
        }

        private void Update()
        {
            if (_transportController.isInTransport && !_transportController.canMove)
                return;

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

            if (_transportController.isInTransport)
            {
                _transportController.currentTransport.SetDirection(directionRounded);
            }
            else
            {
                _player.SetDirection(directionRounded);
            }
        }

        private void FixedUpdate()
        {
            if ((_transportController.isInTransport && !_transportController.canMove) || _direction.sqrMagnitude < 0.1f)
                return;

            if (_transportController.isInTransport)
            {
                _transportController.currentTransport.Move(_direction.normalized);
            }
            else
            {
                _rigidbody.MovePosition(_rigidbody.position + _direction.normalized * speed * Time.fixedDeltaTime);
            }
        }
    }
}