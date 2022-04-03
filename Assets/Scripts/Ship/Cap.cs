using UnityEngine;

namespace Ld50.Ship
{
    public class Cap : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Collider2D _collider;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            // _collider = GetComponent<Collider2D>();
            //
            // _collider.enabled = false;
        }

        public void Brake()
        {
            _collider.enabled = true;
            _renderer.enabled = false;
        }
    }
}