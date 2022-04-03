using System;
using System.Collections.Generic;
using Ld50.Interactable;
using Ld50.Interactable.Shields;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ld50.Core
{
    public class GameTimeManager : MonoBehaviour
    {
        public Camera playerCamera;

        public Fire firePrefab;

        public Hole holePrefab;

        public Turret mainTurret;

        public Shield[] shields;

        public List<Transform> firePositions, holePositions;

        public Transform fireGroup, holeGroup;

        private List<Callback> _callbacks; // (1 / float seconds) probability

        private void Awake()
        {
            _callbacks = new List<Callback> {
                new(30, StartFire),
                new(30, MakeHole),
                new(2, BrakeShield),
                new(30, BrakeTurret),
            };
        }

        private void StartFire()
        {
            var placeIndex = Random.Range(0, firePositions.Count);
            var pos = firePositions[placeIndex].position;

            Instantiate(firePrefab, pos, Quaternion.identity, fireGroup);
        }

        private void MakeHole()
        {
            if (holePositions.Count == 0)
                return;

            var placeIndex = Random.Range(0, holePositions.Count);
            var pos = holePositions[placeIndex].position;

            holePositions.RemoveAt(placeIndex);

            Instantiate(holePrefab, pos, Quaternion.identity, holeGroup);
        }

        private void BrakeShield() => shields[Random.Range(0, shields.Length)].Brake();


        private void BrakeTurret() => mainTurret.Brake();

        private void Update()
        {
            var e = Random.value;

            foreach (var callback in _callbacks)
            {
                if (e <= callback.Frequency)
                {
                    callback.callback();
                    break;
                }

                e -= callback.Frequency;
            }
        }

        [Serializable]
        private class Callback
        {
            public float averagePerSeconds;

            public Action callback;

            public float Frequency => Time.deltaTime / averagePerSeconds;

            public Callback(float averagePerSeconds, Action callback)
            {
                this.averagePerSeconds = averagePerSeconds;
                this.callback = callback;
            }
        }
    }
}