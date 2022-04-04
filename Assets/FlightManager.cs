using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ld50
{
    public class FlightManager : MonoBehaviour
    {
        public GameObject space;
        private float noiseT = 0f;
        private Vector3 spaceStartPos;

        public ParticleSystem psFlow;

        public Camera mainCamera;
        private Core.CameraFollower _cameraFollower;

        void Start()
        {
            spaceStartPos = space.transform.position;
            _cameraFollower = mainCamera.GetComponent<Core.CameraFollower>();
            psFlow.Play();
        }

        void Update()
        {
            noiseT += 0.003f;

            var noise1 = Mathf.PerlinNoise(noiseT, 0) - 0.5f;
            var noise2 = Mathf.PerlinNoise(0, noiseT) - 0.5f;
            var noise3 = Mathf.PerlinNoise(noiseT, noiseT) - 0.5f;

            //space.transform.SetPositionAndRotation(
            //    spaceStartPos +- new Vector3(noise2, noise3) * 20f,
            //    space.transform.rotation
            //);

            space.transform.Rotate(0, 0, noise1 * 2f);
            psFlow.transform.rotation = Quaternion.Euler(0, 0, noise1 * 50f);

            // CAMERA

            mainCamera.transform.rotation = Quaternion.Euler(0, 0, noise1 * 10f);

            var camNoiseX = (Mathf.PerlinNoise(noiseT * 10, 0) - 0.5f) * 0.3f;
            var camNoiseY = (Mathf.PerlinNoise(0, noiseT * 10) - 0.5f) * 0.3f;

            _cameraFollower.offset = new Vector3(camNoiseX, camNoiseY, -10);
            
        }
    }
}
