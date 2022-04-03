namespace Ld50.Interactable
{
    public interface IBreakable
    {
        bool IsBroken { get; }

        void Brake();

        void Fix();
    }
}