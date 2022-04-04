using System;
using Ld50.Animations;
using UniRx;
using UnityEngine;

namespace Ld50.Interactable
{
    public class Explosion : MonoBehaviour
    {
        public LineAnimator explosionAnim;
        public AudioSource sound;

        void Start()
        {
            sound.Play();
            explosionAnim
                .LaunchOnce(explosionAnim.sprites)
                .DoOnCompleted(
                    () => {
                        Destroy(gameObject);
                    }
                )
                .Subscribe()
                .AddTo(this);
        }
    }
}
