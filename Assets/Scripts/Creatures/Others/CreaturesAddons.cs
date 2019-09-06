namespace CreaturesAddons
{
    public interface IAwake
    {
        void Awake(Creature creature);
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
    public interface IAttack
    {
        /// <summary>
        /// Creature attack
        /// </summary>
        /// <param name="deltaTime">Time since last update in seconds.<seealso cref="UnityEngine.Time.deltaTime"/></param>
        void Attack(float deltaTime);
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
