namespace CreaturesAddons
{
    public interface IBuild
    {
        void Build(Creature creature);
    }
    public interface IInitialize
    {
        void Initialize();
    }
    public interface IDie
    {
        /// <summary>
        /// Executed on death.
        /// </summary>
        /// <param name="suicide"><see langword="true"/> if it was a suicide. <see langword="false"/> if it was murderer.</param>
        void Die(bool suicide);
    }
    public interface IMove
    {
        /// <summary>
        /// Move creature.
        /// </summary>
        /// <param name="deltaTime">Time since last update in seconds. <seealso cref="UnityEngine.Time.deltaTime"/></param>
        /// <param name="speedMultiplier">Speed multiplier.</param>
        void Move(float deltaTime, float speedMultiplier = 1);
    }
    public interface IUpdate
    {
        /// <summary>
        /// Updates behaviour.
        /// </summary>
        /// <param name="deltaTime">Time since last update in seconds. <seealso cref="UnityEngine.Time.deltaTime"/></param>
        void Update(float deltaTime);
    }
}
