using UnityEngine;

namespace Ld50.Ship
{
    public class Cap : MonoBehaviour
    {
        public bool isBroken;
        
        private SpriteRenderer _renderer;
        private Collider2D _collider;

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