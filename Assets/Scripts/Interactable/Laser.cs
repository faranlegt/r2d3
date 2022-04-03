using System;
using UniRx;
using UnityEngine;

namespace Ld50.Interactable
{
    public class Laser : MonoBehaviour
    {
        public float lifetime = 5f;

        public float speed = 4f;

        private void Start()
        {
            Observable
                .Timer(TimeSpan.FromSeconds(lifetime))
                .DoOnCompleted(() => Destroy(gameObject))
                .Subscribe()
                .AddTo(this);
        }

        private void Update()
        {
            var t = transform;
            t.position += t.rotation * Vector3.up * speed * Time.deltaTime;
        }
    }
}