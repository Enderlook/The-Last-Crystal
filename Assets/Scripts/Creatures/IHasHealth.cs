namespace Creatures
{
    public interface IHasHealth
    {
        /// <summary>
        /// Take damage reducing its health.<br/>
        /// Animation and floating text will only be show if their parameters are <see langword="true"/> and the effective taken damage is greater than 0.
        /// </summary>
        /// <param name="amount">Amount of health lost. Must be positive.</param>
        /// <param name="displayText">Whenever the damage taken must be shown in a floating text.</param>
        /// <param name="produceFeedback">Whenever it should play hurting animation and sound.</param>

        void TakeDamage(float amount, bool displayText = true, bool produceFeedback = true);

        /// <summary>
        /// Take healing increasing its health.<br/>
        /// Animation and floating text will only be show if their parameters are <see langword="true"/> and the effective taken healing is greater than 0.
        /// </summary>
        /// <param name="amount">Amount of health restored. Must be positive.</param>
        /// <param name="displayText">Whenever the health restored must be shown in a floating text.</param>
        /// <param name="produceFeedback">Whenever it should play healing animation and sound.</param>
        void TakeHealing(float amount, bool displayText = true, bool produceFeedback = true);
    }
}