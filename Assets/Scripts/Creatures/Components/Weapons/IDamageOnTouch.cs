using Creatures.Effects;

namespace Creatures.Weapons
{
    public interface IDamageOnTouch<T>
    {
        /// <summary>
        /// Produce damage.
        /// </summary>
        /// <param name="takeDamage">Interface used to take damage. If <see langword="null"/>, it's immune to damage.</param>
        /// <param name="takePush">Interface used to be pushed. If <see langword="null"/>, it's immune to be pushed.</param>
        /// <param name="takePush">Interface used to take effects. If <see langword="null"/>, it's immune to effects.</param>
        void ProduceDamage(IHasHealth takeDamage, ITakePush takePush, ITakeEffect<T> takeEffect);
    }
}