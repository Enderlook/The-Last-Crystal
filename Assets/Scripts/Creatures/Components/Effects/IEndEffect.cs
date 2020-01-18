namespace Creatures.Effects
{
    public interface IEndEffect
    {
        /// <summary>
        /// Finalize the effect.
        /// </summary>
        /// <param name="wasAborted">Whenever the effect end due duration expiration or if it was aborted by an external force.</param>
        void OnEnd(bool wasAborted);
    }
}