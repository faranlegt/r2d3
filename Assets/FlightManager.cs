using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ld50
{
    public class FlightManager : MonoBehaviour
    {
        bool _started = false;

        public GameObject space;
        private float noiseT = 0f;
        private Vector3 spaceStartPos;

        public ParticleSystem psFlow;

        public Camera mainCamera;

        void Start()
        {
            spaceStartPos = space.transform.position;
        }

        void Update()
        {
            if (!_started)
            {
                psFlow.Play();
                _started = true;
            }


            noiseT += 0.001f;

            var noise1 = Mathf.PerlinNoise(noiseT, 0) - 0.5f;
            var noise2 = Mathf.PerlinNoise(0, noiseT) - 0.5f;
            var noise3 = Mathf.PerlinNoise(noiseT, noiseT) - 0.5f;

            //space.transform.SetPositionAndRotation(
            //    spaceStartPos +- new Vector3(noise2, noise3) * 20f,
            //    space.transform.rotation
            //);

            space.transform.Rotate(0, 0, noise1 * 0.5f);
            psFlow.transform.rotation = Quaternion.Euler(0, 0, noise1 * 50f);

            mainCamera.transform.rotation = Quaternion.Euler(0, 0, noise1 * 10f);
        }
    }
}
