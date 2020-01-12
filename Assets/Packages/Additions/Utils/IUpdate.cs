namespace Additions.Utils
{
    public interface IUpdate
    {
        /// <summary>
        /// Updates behavior.
        /// </summary>
        /// <param name="deltaTime">Time since last update in seconds. <seealso cref="UnityEngine.Time.deltaTime"/></param>
        void UpdateBehaviour(float deltaTime);
    }
}
