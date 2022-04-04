using System;
using Ld50.Animations;
using UnityEngine;

namespace Ld50.Interactable.Shields
{
    public class Shield : MonoBehaviour, IBreakable
    {
        public float brakePower = 1;
        
        public bool isBroken;

        public SpritesLine working, broken;

        public Teleporter teleporter;

        public SpriteRenderer shieldAura;

        public bool IsBroken => isBroken;

        private LineAnimator _animator;
        private Ship.Ship _ship;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
            _ship = FindObjectOfType<Ship.Ship>();
        }

        public void Brake()
        {
            if (isBroken)
                return;
            
            isBroken = true;
            teleporter.canTeleport = false;
            shieldAura.enabled = false;

            _animator.StartLine(broken, true);
        }

        public void Fix()
        {
            if (!isBroken)
                return;
            
            isBroken = false;
            teleporter.canTeleport = true;
            shieldAura.enabled = true;

            _animator.StartLine(working, true);
        }

        private void Update()
        {
            if (isBroken)
            {
                _ship.Brake(brakePower * Time.deltaTime);
            }
        }
    }
}