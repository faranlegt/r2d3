using Ld50.Animations;
using Ld50.Interactable;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ld50.Characters
{
    public class SocketController : MonoBehaviour
    {
        public SpritesLine plugIn, plugOut;

        public bool isInSocket, canPowerUp;

        public Socket currentSocket;

        private TransportController _transportController;
        private Character _character;
        private LineAnimator _animator;

        private void Awake()
        {
            _character = GetComponent<Character>();
            _animator = GetComponent<LineAnimator>();
            _transportController = GetComponent<TransportController>();
        }

        private void Start()
        {
            this.OnTriggerStay2DAsObservable()
                .Where(
                    c => c.gameObject.CompareTag("Socket")
                         && Input.GetKey(KeyCode.Z)
                         && !isInSocket
                         && !_transportController.isInTransport
                         && !_character.isAutoMoving
                )
                .Do(c => PlugIn(c.gameObject.GetComponent<Socket>()))
                .Subscribe()
                .AddTo(this);
        }

        private void Update()
        {
            if (!canPowerUp)
                return;

            if (Input.GetKey(KeyCode.Z))
            {
                currentSocket.PowerUp();
            }
            else if (Input.GetKey(KeyCode.X))
            {
                PlugOut();
            }
        }

        private void PlugIn(Socket socket)
        {
            isInSocket = true;

            _character
                .MoveToPoint(socket.transform.position)
                .DoOnCompleted(
                    () =>
                    {
                        _animator.StartLine(plugIn, loop: false);

                        _animator
                            .OnAnimationEnd
                            .Take(1)
                            .DoOnCompleted(
                                () =>
                                {
                                    currentSocket = socket;
                                    canPowerUp = true;
                                }
                            )
                            .Subscribe()
                            .AddTo(this);
                    }
                )
                .Subscribe()
                .AddTo(this);
        }

        private void PlugOut()
        {
            canPowerUp = false;
            currentSocket = null;

            _animator.StartLine(plugOut, loop: false);

            _animator
                .OnAnimationEnd
                .Take(1)
                .DoOnCompleted(
                    () =>
                    {
                        isInSocket = false;
                        _animator.StartLine(_character.idle, loop: true);
                    }
                )
                .Subscribe()
                .AddTo(this);
        }
    }
}