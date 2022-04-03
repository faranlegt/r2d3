using System;
using UnityEngine;

namespace Ld50.Interactable.MiniGames
{
    public class Press8TimesMiniGame : MiniGame
    {
        public SpriteRenderer[] circles;

        public int pressedTimes = 0;

        public float timeToLoseCircle = 0.1f;

        public float timeFromLastPress = 0f;

        public void UpdateCircles()
        {
            for (var i = 0; i < circles.Length; i++)
            {
                circles[i].enabled = i + 1 <= pressedTimes;
            }
        }

        public void Update()
        {
            if (!playing)
                return;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                timeFromLastPress = 0;
                pressedTimes++;

                if (pressedTimes >= 8)
                {
                    Success();
                }

                UpdateCircles();
            }

            timeFromLastPress += Time.deltaTime;

            if (timeFromLastPress >= timeToLoseCircle)
            {
                timeFromLastPress = 0;
                if (pressedTimes > 0)
                {
                    pressedTimes--;
                }

                UpdateCircles();
            }
        }

        public override void Launch(IBreakable breakable)
        {
            if (playing)
                return;

            base.Launch(breakable);

            timeFromLastPress = 0;
            pressedTimes = 0;

            UpdateCircles();
        }
    }
}