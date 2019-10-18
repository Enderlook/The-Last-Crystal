using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class Global
{
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

    /// <summary>
    /// List of all players
    /// </summary>
    public static List<Transform> players = new List<Transform> ();
    
    /// <summary>
    /// Menu instance.
    /// </summary>
    public static Menu menu;
}

public class Configuration : MonoBehaviour
{
#pragma warning disable IDE0051
#pragma warning disable CS0649
    [Header("Configuration")]
    [SerializeField, Tooltip("Stating money.")]
    private int startingMoney;

    //[Header("Menu")]
    //[SerializeField, Tooltip("Money controller script.")]
    //private CoinMeter coinMeter;

    [Header("Setup")]
    [SerializeField, Tooltip("Crystal.")]
    private Transform crystal;
    [SerializeField, Tooltip("Menu.")]
    private Menu menu;
#pragma warning restore CS0649
#pragma warning restore IDE0051

    private void Awake() => StoreGlobals();

    private void StoreGlobals()
    {
        if (Global.players.Count > 2) Global.players = new List<Transform>(); // Reset the list of players
        // https://stackoverflow.com/questions/8151888/c-sharp-iterate-through-class-properties
        // Use Fields instead of Properties fixes a bug
        foreach (FieldInfo field in typeof(Configuration).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
        {
            // https://stackoverflow.com/questions/3460745/setting-properties-with-reflection-on-static-classes or typeof(Global), whatever works...
            // https://stackoverflow.com/questions/7334067/how-to-get-fields-and-their-values-from-a-static-class-in-referenced-assembly
            typeof(Global).GetField(field.Name, BindingFlags.Public | BindingFlags.Static)?.SetValue(typeof(Global), field.GetValue(this));
        }

        //Global.SetCoinMeter(coinMeter, startingMoney);
    }
}
