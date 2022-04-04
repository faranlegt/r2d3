using System;
using Ld50.Hints;
using UnityEngine;

namespace Ld50.Interactable
{
    public class Fire : MonoBehaviour
    {
        public float life = 1f;

        public float brakePower = 1f;

        public bool isBroken;

        public Transform fire;
        private Ship.Ship _ship;


        private void Awake()
        {
            _ship = FindObjectOfType<Ship.Ship>();
        }

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

                GetComponentInChildren<TransportRequiredHint>().show = false;
                Destroy(fire.gameObject);
            } 
        }

        private void Update()
        {
            if (isBroken)
            {
                _ship.Brake(brakePower * Time.deltaTime);
            }
        }
    }
}