using UnityEngine;

namespace Ld50.Interactable.MiniGames
{
    public class MiniGame : MonoBehaviour
    {
        public GameObject tooltip;
        
        public bool playing = false;

        protected IBreakable Breakable;

        public void Start() => Cancel();

        public virtual void Launch(IBreakable breakable)
        {
            Breakable = breakable;
            playing = true;

            tooltip.SetActive(true);
        }

        public virtual void Success()
        {
            if (!playing) return;
            
            Breakable.Fix();
            
            Cancel();
        }

        public virtual void Cancel()
        {
            Breakable = null;
            playing = false;

            tooltip.SetActive(false);
        }
    }
}