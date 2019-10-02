using System.Reflection;
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
#pragma warning disable IDE0051
#pragma warning disable CS0649
    [Header("Configuration")]
    [SerializeField, Tooltip("Stating money.")]
    private int startingMoney;

    [Header("Setup")]
    [Header("Parents")]
    [SerializeField, Tooltip("Enemies parent transform.")]
    private Transform enemiesParent;
    [SerializeField, Tooltip("Projectiles parent transform.")]
    private Transform projectilesParent;
    [SerializeField, Tooltip("Floating text parent transform.")]
    private Transform floatingTextParent;
    [SerializeField, Tooltip("Pickups parent transform.")]
    private Transform pickupsParent;

    //[Header("Menu")]
    //[SerializeField, Tooltip("Money controller script.")]
    //private CoinMeter coinMeter;

    [SerializeField, Tooltip("Crystal")]
    private Transform crystal;
#pragma warning restore CS0649
#pragma warning restore IDE0051

    private void Awake() => StoreGlobals();

    private void StoreGlobals()
    {
        // https://stackoverflow.com/questions/8151888/c-sharp-iterate-through-class-properties
        // Use Fields instead of Properties fixes a bug
        foreach (FieldInfo field in typeof(Configuration).GetFields())
        {
            // https://stackoverflow.com/questions/3460745/setting-properties-with-reflection-on-static-classes or typeof(Global), whatever works...
            // https://stackoverflow.com/questions/7334067/how-to-get-fields-and-their-values-from-a-static-class-in-referenced-assembly
            typeof(Global).GetField(field.Name, BindingFlags.Public | BindingFlags.Static)?.SetValue(typeof(Global), field.GetValue(this));
        }

        //Global.SetCoinMeter(coinMeter, startingMoney);
        FloatingTextController.SetFloatingTextParentStatic(floatingTextParent);
    }
}
