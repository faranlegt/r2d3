using UnityEngine;

namespace Ld50.Core
{
    public class FlightManager : MonoBehaviour
    {
        public GameObject space;
        private float noiseT = 0f;
        private Vector3 spaceStartPos;

        public ParticleSystem psFlow;

        public Camera mainCamera;
        private Core.CameraFollower _cameraFollower;

        public AudioSource explosionSound;
        public GameTimeManager gameTimeManager;
        private float _playingFactor = 0f;

        void Start()
        {
            spaceStartPos = space.transform.position;
            _cameraFollower = mainCamera.GetComponent<Core.CameraFollower>();
            psFlow.Play();
        }

        public void ShakeCamera(int time, float strength)
        {
            shakeTTL = time;
            shakeStrength = strength;
        }

        int shakeTTL = 0;
        float shakeStrength = 0;
        void Update()
        {
            if (gameTimeManager.isPlaying && _playingFactor < 1f)
            {
                _playingFactor += Time.deltaTime;
            }
            else if (!gameTimeManager.isPlaying && _playingFactor > 0)
            {
                _playingFactor -= Time.deltaTime;
            }

            noiseT += 0.003f;

            var noise1 = Mathf.PerlinNoise(noiseT, 0) - 0.5f;
            var noise2 = Mathf.PerlinNoise(0, noiseT) - 0.5f;
            var noise3 = Mathf.PerlinNoise(noiseT, noiseT) - 0.5f;

            noise1 *= _playingFactor;
            noise2 *= _playingFactor;
            noise3 *= _playingFactor;

            //space.transform.SetPositionAndRotation(
            //    spaceStartPos +- new Vector3(noise2, noise3) * 20f,
            //    space.transform.rotation
            //);

            space.transform.Rotate(0, 0, noise1 * 2f);
            psFlow.transform.rotation = Quaternion.Euler(0, 0, noise1 * 50f);

            // CAMERA

            mainCamera.transform.rotation = Quaternion.Euler(0, 0, noise1 * 10f);

            if (UnityEngine.Random.Range(0, 2000) == 13 && gameTimeManager.isPlaying)
            {
                explosionSound.transform.position = UnityEngine.Random.insideUnitCircle * 15;
                explosionSound.volume = UnityEngine.Random.Range(0.1f, 0.3f);
                explosionSound.Play();
                ShakeCamera(40, 4);
            }

            var camNoiseX = (Mathf.PerlinNoise(noiseT * 10, 0) - 0.5f) * 0.3f;
            var camNoiseY = (Mathf.PerlinNoise(0, noiseT * 10) - 0.5f) * 0.3f;

            if (shakeTTL > 0)
            {
                shakeTTL--;
                camNoiseX += UnityEngine.Random.Range(-shakeStrength, shakeStrength);
                camNoiseY += UnityEngine.Random.Range(-shakeStrength, shakeStrength);
            }

            _cameraFollower.offset = new Vector3(camNoiseX, camNoiseY, -10);
            
        }
    }
}
