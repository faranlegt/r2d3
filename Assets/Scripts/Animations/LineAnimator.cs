using System;
using UniRx;
using UnityEngine;

namespace Ld50.Animations
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LineAnimator : MonoBehaviour
    {
        public SpritesLine sprites;
        public int animationFrame = 0;
        public bool loop;
        public bool animate;
        public float frameLength = 1f;

        public IObservable<Unit> OnAnimationEnd => _animationEnd;

        private SpriteRenderer _renderer;
        private SpritesLine _oldSprites;

        private bool _revalidate;
        private float _frameTime;
        private int _oldFrame;

        private Subject<Unit> _animationEnd;

        private void Awake()
        {
            _animationEnd = new Subject<Unit>();
            _renderer = GetComponent<SpriteRenderer>();

            SyncSprite();
        }

        private void Start()
        {
            _revalidate = true;
        }

        public void StartLine(SpritesLine line, bool? loop = null)
        {
            sprites = line;
            animationFrame = 0;
            animate = true;
            this.loop = loop ?? this.loop;
        }

        public IObservable<Unit> LaunchOnce(SpritesLine line)
        {
            StartLine(line, loop: false);

            return OnAnimationEnd.Take(1);
        }

        private void OnValidate()
        {
            _revalidate = true;
        }

        private void LateUpdate()
        {
            _revalidate |= _oldFrame != animationFrame || _oldSprites != sprites;

            if (_revalidate)
            {
                SyncSprite();
            }

            if (animate)
            {
                _frameTime += Time.deltaTime;
                if (_frameTime >= frameLength)
                {
                    NextFrame();
                }
            }

            _oldFrame = animationFrame;
            _oldSprites = sprites;
        }

        private void NextFrame()
        {
            _frameTime = 0;
            if (animationFrame >= sprites.sprites.Length - 1)
            {
                if (loop)
                {
                    animationFrame = 0;
                }
                else
                {
                    animate = false;
                    _animationEnd.OnNext(Unit.Default);
                }
            }
            else
            {
                animationFrame++;
            }

            SyncSprite();
        }

        private void SyncSprite()
        {
            _renderer.sprite = sprites.sprites[animationFrame];
            _revalidate = false;
        }

        private void OnDestroy() => _animationEnd?.Dispose();
    }
}