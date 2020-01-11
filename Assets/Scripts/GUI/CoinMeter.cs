using Master;

using UnityEngine;
using UnityEngine.UI;

public class CoinMeter : MonoBehaviour
{
#pragma warning disable CS0649
    [Header("Setup")]
    [SerializeField, Tooltip("Text where money will be displayed.")]
    private Text moneyText;
#pragma warning restore CS0649

    public float showedMoney;
    private int oldMoney;
    private int realMoney;

    public int Money {
        get => realMoney;
        set {
            oldMoney = (int)showedMoney;
            realMoney = value;
        }
    }

    public void ManualUpdate(int amount)
    {
        realMoney = oldMoney = amount;
        showedMoney = amount;
        moneyText.text = amount.ToString();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1213:Remove unused member declaration.", Justification = "Used by Unity.")]
    private void Update()
    {
        if (Settings.IsPause)
            return;
        if (showedMoney != realMoney)
        {
            showedMoney = Mathf.Clamp(showedMoney + (realMoney - oldMoney) * Time.deltaTime, Mathf.Min(realMoney, oldMoney), Mathf.Max(realMoney, oldMoney));
            moneyText.text = ((int)showedMoney).ToString();
        }
    }
}
