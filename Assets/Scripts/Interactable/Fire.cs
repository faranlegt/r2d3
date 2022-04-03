using System;
using UnityEngine;

namespace Ld50.Interactable
{
    public class Fire : MonoBehaviour
    {
        public float life = 1f;

        public bool isBroken;

        public Transform fire;
        
        private void Start()
        {
            isBroken = true;
        }

        public void Extinguish(float power)
        {
            if (!isBroken)
                return;
            
            life -= power;
            fire.localScale = Vector3.one * life;

            if (life <= 0)
            {
                isBroken = false;
                Destroy(fire.gameObject);
            } 
        }

        private void Update()
        {
            if (isBroken)
            {
                // todo: brake ship continuously
            }
        }
    }
}