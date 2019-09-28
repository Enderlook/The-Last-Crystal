using UnityEngine;

public static class Global
{
    /// <summary>
    /// Enemies parent transform. Used to store all the enemies.
    /// </summary>
    public static Transform enemiesParent;
    /// <summary>
    /// Projectiles parent transform. Used to store all the projectiles.
    /// </summary>
    public static Transform projectilesParent;
    /// <summary>
    /// Pickups parent transform. Used to store all the pickups.
    /// </summary>
    public static Transform pickupsParent;

    private static CoinMeter coinMeter;
    /// <summary>
    /// Set the <seealso cref="CoinMeter"/> script that controls how money is displayed on canvas.
    /// </summary>
    /// <param name="coinMeterController"><seealso cref="CoinMeter"/> that controls displayed money.</param>
    public static void SetCoinMeter(CoinMeter coinMeterController, int statingMoney)
    {
        coinMeter = coinMeterController;
        coinMeter.ManualUpdate(statingMoney);
    }

    /// <summary>
    /// Current money of the player.
    /// </summary>
    public static int money {
        get => coinMeter.money;
        set => coinMeter.money = value;
    }

    /// <summary>
    /// Amount of money required to win the game.
    /// </summary>
    public static int moneyToWin;

    /// <summary>
    /// Crystal transform.
    /// </summary>
    public static Transform crystal;
    /// <summary>
    /// Player warrior transform.
    /// </summary>
    public static Transform warrior;

    /// <summary>
    /// Player wizard transform.
    /// </summary>
    public static Transform wizard;
}

public class Configuration : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Stating money.")]
    public int startingMoney;

    [Header("Setup")]
    [Header("Parents")]
    [Tooltip("Enemies parent transform.")]
    public Transform enemiesParent;
    [Tooltip("Projectiles parent transform.")]
    public Transform projectilesParent;
    [Tooltip("Floating text parent transform.")]
    public Transform floatingTextParent;
    [Tooltip("Pickups parent transform.")]
    public Transform pickupsParent;

    //[Header("Menu")]
    //[Tooltip("Money controller script.")]
    //public CoinMeter coinMeter;

    [Header("Goals")]
    [Tooltip("Crystal")]
    public Transform crystal;
    [Tooltip("Warrior")]
    public Transform warrior;
    [Tooltip("Wizard")]
    public Transform wizard;

    private void Awake() => StoreGlobals();

    private void StoreGlobals()
    {
        // https://stackoverflow.com/questions/8151888/c-sharp-iterate-through-class-properties
        // Use Fields instead of Properties fixes a bug
        foreach (System.Reflection.FieldInfo field in typeof(Configuration).GetFields())
        {
            // https://stackoverflow.com/questions/3460745/setting-properties-with-reflection-on-static-classes or typeof(Global), whatever works...
            // https://stackoverflow.com/questions/7334067/how-to-get-fields-and-their-values-from-a-static-class-in-referenced-assembly
            typeof(Global).GetField(field.Name, System.Reflection.BindingFlags.Public)?.SetValue(typeof(Global), field.GetValue(this));
        }

        //Global.SetCoinMeter(coinMeter, startingMoney);
        FloatingTextController.SetFloatingTextParentStatic(floatingTextParent);
    }
}
