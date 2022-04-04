using Ld50.Animations;
using Ld50.Characters;
using Ld50.Interactable.Sockets;
using UniRx;
using UnityEngine;

namespace Ld50.Interactable.Shields
{
    public class Teleporter : MonoBehaviour, ISocketAction
    {
        public bool canTeleport = true;
        public bool isAnimating = false;
        public Socket nextSocket;

        public SpritesLine teleportIn, teleportOut;
        public AudioSource teleportInSound, teleportOutSound;

        public void OnEntered(SocketController socketController) { }

        public void OnExited(SocketController socketController) { }

        public void Do(SocketController socketController)
        {
            if (isAnimating || !canTeleport)
                return;

            socketController.canPowerUp = false;
            isAnimating = true;
            var animator = socketController.GetComponent<LineAnimator>();

            teleportOutSound.Play();
            animator
                .LaunchOnce(teleportIn)
                .DoOnCompleted(
                    () =>
                    {
                        socketController.transform.position = nextSocket.transform.position;
                        socketController.currentSocket.PluggedOut(socketController);

                        teleportInSound.Play();
                        animator
                            .LaunchOnce(teleportOut)
                            .DoOnCompleted(
                                () =>
                                {
                                    socketController.currentSocket = nextSocket;
                                    socketController.canPowerUp = true;

                                    nextSocket.PluggedIn(socketController);

                                    isAnimating = false;
                                }
                            )
                            .Subscribe()
                            .AddTo(this);
                    }
                )
                .Subscribe()
                .AddTo(this);
        }
    }
}