namespace Creatures.Weapons
{
    public interface IAutomatedAttack
    {
        /// <summary>
        /// Whenever the weapon has any target in range or not.
        /// </summary>
        bool TargetInRange { get; }

        /// <summary>
        /// Check if <see cref="TargetInRange"/>.<br>
        /// If <see langword="true"/> calls <see cref="Weapon.TryExecute(float)"/> using <paramref name="deltaTime"/> as parameter.<br>
        /// If <see langword="false"/> calls <see cref="Weapon.Recharge(float)"/> using <paramref name="deltaTime"/>.
        /// </summary>
        /// <returns>Whenever an attack was made or not.</returns>
        bool AttackIfIsReadyAndIfTargetInRange(float deltaTime = 0);

        /// <summary>
        /// Whenever should automatically attack or not when <see cref="TargetInRange"/> and <see cref="Weapon.IsReady"/> are <see langword="true"/> on each <see cref="Weapon.UpdateBehaviour(float)"/> or <see cref="Weapon.Recharge(float)"/> call.
        /// </summary>
        bool AutoAttack { get; set; }
    }
}