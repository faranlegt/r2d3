using UnityEngine;

namespace Ld50.Interactable.MiniGames
{
    public class MiniGame : MonoBehaviour
    {
        public SpriteRenderer tooltip;
        
        public bool playing = false;

        protected IBreakable Breakable;

        public void Start() => Cancel();

        public virtual void Launch(IBreakable breakable)
        {
            Breakable = breakable;
            playing = true;

            tooltip.enabled = true;
        }

        public virtual void Sucess()
        {
            if (!playing) return;
            
            Breakable.Fix();
            
            Cancel();
        }

        public virtual void Cancel()
        {
            Breakable = null;
            playing = false;

            tooltip.enabled = false;
        }
    }
}