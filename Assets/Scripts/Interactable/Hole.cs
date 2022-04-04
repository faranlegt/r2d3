using System;
using Ld50.Animations;
using Ld50.Hints;
using UnityEngine;

namespace Ld50.Interactable
{
    public class Hole : MonoBehaviour
    {
        public float brakePower = 1f;
        
        public SpritesLine repaired, welding, waiting;

        public bool isBroken;
        
        private Ship.Ship _ship;
        private LineAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _ship = FindObjectOfType<Ship.Ship>();

            isBroken = true;
        }

        public void StartWelding()
        {
            _animator.StartLine(welding);
        }

        public void StopWelding()
        {
            if (!isBroken) return;
            
            _animator.StartLine(waiting);
        }

        private void Update()
        {
            if (isBroken)
            {
                _ship.Brake(brakePower * Time.deltaTime);
            }
        }

        public void Repair()
        {
            if (!isBroken) return;
            
            isBroken = false;
            _animator.StartLine(repaired);

            GetComponentInChildren<TransportRequiredHint>().show = false;
        }
    }
}