using Ld50.Core;
using UnityEngine;

namespace Ld50.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(TransportController))]
    public class PlayerMovement : MonoBehaviour
    {
        private Vector2 _direction;

        private Character _player;
        private TransportController _transportController;
        private SocketController _socket;

        private GameTimeManager _gameTimeManager;

        private void Awake()
        {
            _socket = GetComponent<SocketController>();
            _player = GetComponent<Character>();
            _transportController = GetComponent<TransportController>();

            _gameTimeManager = FindObjectOfType<GameTimeManager>();
        }

        private void Update()
        {
            if (!_gameTimeManager.isPlaying)
                return;

            if (_player.isAutoMoving
                || _socket.isInSocket
                || (_transportController.isInTransport && !_transportController.canMove))
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
            if ((_transportController.isInTransport && !_transportController.canMove)
                || _socket.isInSocket
                || _direction.sqrMagnitude < 0.1f)
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