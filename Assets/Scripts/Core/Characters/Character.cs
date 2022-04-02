using Ld50.Animations;
using UnityEngine;

namespace Ld50.Core.Characters
{
    public class Character : MonoBehaviour
    {
        public SpritesLine idle, up, left, right, down;

        private LineAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<LineAnimator>();
        }

        public void SetDirection(Direction d) =>
            _animator.sprites = d switch {
                Direction.Up => up,
                Direction.Left => left,
                Direction.Right => right,
                Direction.Down => down,
                _ => idle
            };
    }
}