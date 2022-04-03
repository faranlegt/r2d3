using System;
using UnityEngine;

namespace Ld50.Interactable.MiniGames
{
    public class HoldMiniGame : MiniGame
    {
        public KeyCode holdKeyCode;

        public float timeToHold = 2f;

        public float holdingFor = 0;

        private void Update()
        {
            if (Input.GetKey(holdKeyCode))
            {
                holdingFor += Time.deltaTime;

                if (holdingFor >= timeToHold)
                {
                     Sucess();
                }
            }
            else
            {
                holdingFor = 0;
            }
        }
    }
}