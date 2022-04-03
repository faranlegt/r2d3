using System;
using Ld50.Animations;
using UniRx;
using UnityEngine;

namespace Ld50.Interactable
{
    public class Turret : MonoBehaviour, IBreakable
    {
        public Laser laserPrefab;

        public float period = 3f;

        public bool isWorking;

        public SpritesLine blueLed, redLed, shoot, idle;

        public LineAnimator ledAnimator, towerAnimator;

        public Transform laserStart, tower;

        public bool IsBroken => !isWorking;

        public void Start()
        {
            isWorking = true;
            
            Observable
                .Interval(TimeSpan.FromSeconds(period))
                .Delay(TimeSpan.FromSeconds(period / 2f))
                .Where(_ => isWorking)
                .Do(_ => Shoot())
                .Subscribe()
                .AddTo(this);
        }

        private void Shoot()
        {
            towerAnimator
                .LaunchOnce(shoot)
                .DoOnCompleted(
                    () => { }
                )
                .Subscribe(
                    _ =>
                    {
                        Instantiate(laserPrefab, laserStart.position, laserStart.rotation);

                        towerAnimator.StartLine(idle, true);
                    }
                )
                .AddTo(this);
            ;
        }

        public void Update()
        {
            if (isWorking)
            {
                tower.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * period) * 30f);
            }
        }

        public void Brake()
        {
            isWorking = false;
            ledAnimator.StartLine(redLed);
        }

        public void Fix()
        {
            isWorking = true;
            ledAnimator.StartLine(blueLed);
        }
    }
}