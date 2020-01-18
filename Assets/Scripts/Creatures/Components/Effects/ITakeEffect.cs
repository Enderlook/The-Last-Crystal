namespace Creatures.Effects
{
    public interface ITakeEffect<T>
    {
        void TakeEffect(Effect<T> effect);
    }
}