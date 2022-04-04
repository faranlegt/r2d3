using System;
using System.Collections.Generic;
using Ld50.Animations;
using Ld50.Characters;
using Ld50.Interactable;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ld50.Transports
{
    public class WelderAction : MonoBehaviour, ITransportAction
    {
        public SpritesLine welding;

        public List<Hole> holes = new();

        public float actionTime;

        public float repairTime = 2f;

        private Character _character;
        private LineAnimator _animator;
        private Transport _transport;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _character = GetComponent<Character>();
            _transport = GetComponent<Transport>();
        }

        private void Start()
        {
            this.OnTriggerEnter2DAsObservable()
                .Where(c => c.gameObject.CompareTag("Hole"))
                .Select(c => c.gameObject.GetComponent<Hole>())
                .Do(holes.Add)
                .Subscribe()
                .AddTo(this);


            this.OnTriggerExit2DAsObservable()
                .Where(c => c.gameObject.CompareTag("Hole"))
                .Select(c => c.gameObject.GetComponent<Hole>())
                .Do(
                    h =>
                    {
                        h.StopWelding();
                        holes.Remove(h);
                    }
                )
                .Subscribe()
                .AddTo(this);
        }

        private void Update()
        {
            if (!_transport.isActing)
                return;

            actionTime += Time.deltaTime;

            if (actionTime >= repairTime)
            {
                foreach (var hole in holes)
                {
                    hole.Repair();
                }

                holes.Clear();
            }
        }

        public AudioSource weldSound;

        public void StartAction()
        {
            weldSound.Play();

            actionTime = 0;
            _animator.StartLine(welding, loop: true);
            _transport.GetCharacter().slideSpeed = 0f;

            foreach (var hole in holes)
            {
                hole.StartWelding();
            }
        }

        public void StopAction()
        {
            weldSound.Pause();

            _animator.StartLine(_character.idle);

            foreach (var hole in holes)
            {
                hole.StopWelding();
            }
        }
    }
}