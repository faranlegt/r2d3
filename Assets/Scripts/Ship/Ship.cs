using UnityEngine;

namespace Ld50.Ship
{
    public class Ship : MonoBehaviour
    {
        public float brakeCoefficient = 0.003f;
        
        [Range(0, 1f)]
        public float health = 1f;
        
        public Cap[] caps;

        public SpriteRenderer[] hearts;

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

            for (var i = 0; i < hearts.Length; i++)
            {
                hearts[i].enabled = health >= (i + 1f) / hearts.Length || i == 0;
            }
        }

        public void Brake(float power)
        {
            health -= power * brakeCoefficient;
        }
    }
}