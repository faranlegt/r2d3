using UnityEngine;

namespace Ld50.Ship
{
    public class Ship : MonoBehaviour
    {
        [Range(0, 1f)]
        public float health = 1f;
        
        public Cap[] caps;

        public void Update()
        {
            var brokenCount = Mathf.FloorToInt((1 - health) * caps.Length);
            
            for (var i = 0; i < caps.Length; i++)
            {
                if (i < brokenCount)
                {
                    caps[i].Brake();
                }
                else
                {
                    caps[i].Fix();
                }
            }
        }
    }
}