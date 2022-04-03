using System;
using UnityEngine;

namespace Ld50.Interactable
{
    public class Hole : MonoBehaviour
    {
        public Sprite repaired;

        public bool isBroken;

        public SpriteRenderer tapeRenderer;
        
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();

            isBroken = true;
            tapeRenderer.enabled = false;
        }

        private void Update()
        {
            if (isBroken)
            {
                // todo: brake ship continuously
            }
        }

        public void Repair()
        {
            isBroken = false;
            _renderer.sprite = repaired;
            tapeRenderer.enabled = true;
        }
    }
}