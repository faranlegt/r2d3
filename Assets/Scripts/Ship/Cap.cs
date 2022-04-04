using System;
using Ld50.Characters;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ld50.Ship
{
    public class Cap : MonoBehaviour
    {
        public bool isBroken;
        
        private SpriteRenderer _renderer;
        private Collider2D _collider;

        public Transform fallbackPoint, explosionPoint;

        public Ld50.Interactable.Explosion explosionPrefab;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();

            _collider.enabled = false;
        }

        public void Brake()
        {
            if (isBroken) return;
            
            isBroken = true;
            _collider.enabled = true;
            _renderer.enabled = false;

            this.OnCollisionEnter2DAsObservable()
                .Select(c => c.gameObject.TryGetComponent(out Character ch) ? ch : null)
                .Where(c => c)
                .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(0.1f)))
                .Do(c => c.transform.position = fallbackPoint.position)
                .Subscribe()
                .AddTo(this);

            Instantiate(explosionPrefab, explosionPoint.transform);
        }

        public void Fix()
        {
            if (!isBroken) return;

            isBroken = false;
            _collider.enabled = false;
            _renderer.enabled = true;
        }
    }
}