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
    public class ExtinguisherAction : MonoBehaviour, ITransportAction
    {
        public float extinguishTime = 3f;
        public SpritesLine extinguish;

        public List<Fire> fires = new();
        
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
                .Where(c => c.gameObject.CompareTag("Fire"))
                .Select(c => c.gameObject.GetComponent<Fire>())
                .Do(fires.Add)
                .Subscribe()
                .AddTo(this);
            
            
            this.OnTriggerExit2DAsObservable()
                .Where(c => c.gameObject.CompareTag("Fire"))
                .Select(c => c.gameObject.GetComponent<Fire>())
                .Do(h => fires.Remove(h))
                .Subscribe()
                .AddTo(this);
        }

        public void StartAction()
        {
            _animator.StartLine(extinguish, loop: true);
        }

        public void StopAction()
        {
            _animator.StartLine(_character.idle);
        }

        private void Update()
        {
            if (!_transport.isActing) return;

            foreach (var fire in fires)
            {
                fire.Extinguish(Time.deltaTime / extinguishTime);
            }
        }
    }
}