using UnityEngine;

namespace Ld50.Hints
{
    public class HintOnApproach : MonoBehaviour
    {
        private Player _player;
        public SpriteRenderer _hint;
        public float distanceToPlayer = 0;
        public float distance = 1.5f;
        public bool show = true;

        public void Awake()
        {
            _player = FindObjectOfType<Player>();
        }

        void Update()
        {
            if (!show)
            {
                _hint.enabled = false;
                return;
            }

            distanceToPlayer = (_player.transform.position - transform.position).magnitude;
            _hint.enabled = distanceToPlayer < 2f && InternalCondition();

            //_hint.transform.position =
            //    transform.position +
            //    (transform.position - _player.transform.position).normalized * distance;

            bool playerAbove = _player.transform.position.y < transform.position.y;

            _hint.transform.position =
                transform.position + new Vector3(0, distance * (playerAbove ? 1 : -1), 0);
        }

        protected virtual bool InternalCondition() => true;
    }
}
