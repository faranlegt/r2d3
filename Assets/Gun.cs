using System;
using Ld50.Animations;
using UniRx;
using UnityEngine;

namespace Ld50.Interactable
{
    public class Gun : MonoBehaviour
    {
        public Laser laserPrefab;
        public LineAnimator gunAnimator;
        public AudioSource laserSound;
        public Transform laserStart;

        public SpritesLine shoot, idle;

        // Start is called before the first frame update
        void Start()
        {
        }

        private void Shoot()
        {
            Observable
                .Timer(TimeSpan.FromSeconds(5 / 60f))
                .DoOnCompleted(() => {
                    var laserInstance = Instantiate(laserPrefab, laserStart.position, laserStart.rotation);
                    laserSound.Play();
                })
                .Subscribe()
                .AddTo(this);

            gunAnimator
                .LaunchOnce(shoot)
                .DoOnCompleted(
                    () => { }
                )
                .Subscribe(
                    _ =>
                    {
                        gunAnimator.StartLine(idle, true);
                    }
                )
                .AddTo(this);
            ;
        }

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Random.Range(1, 1000) < 10)
                Shoot();
        }
    }
}
