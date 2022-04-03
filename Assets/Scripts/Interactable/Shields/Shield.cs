using Ld50.Animations;
using UnityEngine;

namespace Ld50.Interactable.Shields
{
    public class Shield : MonoBehaviour, IBreakable
    {
        public bool isBroken;

        public SpritesLine working, broken;

        public Teleporter teleporter;

        public SpriteRenderer shieldAura;

        public bool IsBroken => isBroken;

        private LineAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
        }

        public void Brake()
        {
            isBroken = true;
            teleporter.canTeleport = true;
            shieldAura.enabled = false;

            _animator.StartLine(broken, true);
        }

        public void Fix()
        {
            isBroken = false;
            teleporter.canTeleport = false;
            shieldAura.enabled = true;

            _animator.StartLine(working, false);
        }
    }
}