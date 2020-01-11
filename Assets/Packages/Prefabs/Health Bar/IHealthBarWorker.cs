namespace Additions.Prefabs.HealthBarGUI
{
    public interface IHealthBarWorker
    {
        /// <summary>
        /// Modify the health bar values without producing any animation effects (sliding the bar or changing the numbers).
        /// The health bar fill will be instantaneously set without producing animation. Health numbers will also change immediately.
        /// Both damage bar and healing bar fill will be set to 0, halting any current animation on them.
        /// Designed to initialize the health bar by first time.
        /// </summary>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        void ManualUpdate(float health, float maxHealth);

        /// <summary>
        /// Modify the health bar values without producing any animation effects (sliding the bar or changing the numbers).
        /// The health bar fill will be instantaneously set without producing animation. Health numbers will also change immediately.
        /// Both damage bar and healing bar fill will be set to 0, halting any current animation on them.
        /// Both current health and maximum health will be assigned by maxHealth.
        /// Designed to initialize the health bar by first time.
        /// </summary>
        void ManualUpdate(float health);

        /// <summary>
        /// Modify the current health and maximum health.
        /// This method will automatically calculate, show and animate the health bar, damage bar, healing bar and health number.
        /// </summary>
        /// <param name="health"></param>
        /// <param name="maxhealth"></param>
        void UpdateValues(float health, float maxHealth);

        /// <summary>
        /// Modify the current health.
        /// This method will automatically calculate, show and animate the health bar, damage bar, healing bar and health number.
        /// </summary>
        /// <param name="health"></param>
        void UpdateValues(float health);
    }
}