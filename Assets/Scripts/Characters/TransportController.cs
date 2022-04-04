using Ld50.Animations;
using Ld50.Transports;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ld50.Characters
{
    public class TransportController : MonoBehaviour
    {
        public bool isInTransport;
        public bool canMove;

        public Transport currentTransport;

        private LineAnimator _animator;
        private SpriteRenderer _renderer;
        private Collider2D _collider;
        private Character _character;
        private SocketController _socket;

        public AudioSource plugInSound, plugOutSound;

        public SpriteRenderer hintWeld, hintExtinguish, hintExit;

        private void Awake()
        {
            _socket = GetComponent<SocketController>();
            _character = GetComponent<Character>();
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
                         && !_socket.isInSocket
                         && !_character.isAutoMoving
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
            currentTransport.GetCharacter().Slide();

            if (Input.GetKey(KeyCode.X))
            {
                Exit();
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                currentTransport.Action();
            }
            else
            {
                currentTransport.NoAction();
            }
        }

        public void Enter(Transport transport)
        {
            isInTransport = true;
            _collider.enabled = false;

            hintExit.enabled = true;

            if (transport.gameObject.name == "Welder")
                hintWeld.enabled = true;
            else
                hintExtinguish.enabled = true;

            _character
                .Jump(transport.jumpTarget.position)
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
                                    plugInSound.Play();
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

            hintExit.enabled = false;

            hintWeld.enabled = false;
            hintExtinguish.enabled = false;

            currentTransport
                .Stop()
                .Do(
                    _ =>
                    {
                        _collider.enabled = true;
                        _renderer.enabled = true;

                        _character
                            .Jump(targetPos)
                            .DoOnCompleted(
                                () =>
                                {
                                    _animator.animate = true;
                                    _animator.loop = true;

                                    isInTransport = false;
                                    plugOutSound.Play();
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
    }
}