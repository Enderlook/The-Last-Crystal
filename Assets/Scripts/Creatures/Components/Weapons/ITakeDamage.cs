namespace Creatures.Weapons
{
    public interface ITakeDamage
    {
        void TakeDamage(float amount, bool displayText = true, bool displayAnimation = true);
    }
}