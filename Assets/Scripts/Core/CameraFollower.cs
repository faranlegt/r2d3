using System;
using UnityEngine;

namespace Ld50.Core
{
    public class CameraFollower : MonoBehaviour
    {
        public Transform target;

        public float speed = 10f;

        public Vector3 offset = Vector3.zero;

        private void LateUpdate()
        {
            var targetPos = target.transform.position + offset;
            var smoothPos = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

            transform.position = smoothPos;
        }
    }
}