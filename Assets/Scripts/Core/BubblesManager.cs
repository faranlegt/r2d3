using Unity.Mathematics;
using UnityEngine;

namespace Ld50.Core
{
    public class BubblesManager : MonoBehaviour
    {
        public ChatBubble bubblePrefab;

        public void Create(Vector3 position, string text)
        {
            var bubble = Instantiate(bubblePrefab, position, quaternion.identity, transform);
            bubble.SetText(text);
        }
    }
}