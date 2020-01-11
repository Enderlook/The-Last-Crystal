using UnityEngine.UI;

namespace Additions.Prefabs.HealthBarGUI
{
    public interface IHealthBarViewer
    {
        /// <summary>
        /// Whenever the health bar is showed or hidden.<br/>
        /// Take into account that the script is still enabled and will update the health bar even if it's hidden.
        /// </summary>
        /// <seealso cref="IsEnabled"/>
        bool IsVisible { get; set; }

        /// <summary>
        /// Whenever the health bar will be updated each frame or not.<br/>
        /// Take into account that the script is still enabled but it won't be updated on each frame. Also, it's still visible.
        /// </summary>
        /// <seealso cref="IsHidden"/>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Returns <seealso cref="Image.fillAmount"/> of the <see cref="healthBar"/>.<br/>
        /// </summary>
        float HealthBarPercentFill { get; }

        /// <summary>
        /// Returns <seealso cref="Image.fillAmount"/> of the <see cref="healingBar"/>.<br/>
        /// Warning! If <see cref="healingBar"></see> is <see langword="null"/> it will return <see langword="null"/>.
        /// </summary>
        float? HealingBarPercentFill { get; }

        /// <summary>
        /// Returns <seealso cref="Image.fillAmount"/> of the <see cref="damageBar"/>.<br/>
        /// Warning! If <see cref="damageBar"/> is <see langword="null"/> it will return <see langword="null"/>.
        /// </summary>
        float? DamageBarPercentFill { get; }

        /// <summary>
        /// Returns <see langword="true"/> if <seealso cref="Image.fillAmount"/> of the <see cref="healingBar"/> if 0 or <see cref="healingBar"/> is <see langword="null"/>.<br/>
        /// </summary>
        bool IsHealingBarPercentHide { get; }

        /// <summary>
        /// Returns <see langword="true"/> if <seealso cref="Image.fillAmount"/> of the <see cref="damageBar"/> if 0 or <see cref="damageBar"/> is <see langword="null"/>.<br/>
        /// </summary>
        bool IsDamageBarPercentHide { get; }
    }
}