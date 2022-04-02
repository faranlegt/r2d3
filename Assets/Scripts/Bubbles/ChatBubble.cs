using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace Ld50.Bubbles
{
    public class ChatBubble : MonoBehaviour
    {
        public TextMeshPro text;

        public SpriteRenderer background;
        
        public Vector2 padding = Vector2.zero;

        public void SetText(string t, float lifeTime = 3f)
        {
            text.SetText(t);
            text.ForceMeshUpdate();
            var size = text.GetRenderedValues(false);

            background.size = size + padding;

            Observable
                .Timer(TimeSpan.FromSeconds(lifeTime))
                .Do(_ => Destroy(gameObject))
                .Subscribe()
                .AddTo(this);
        }
    }
}