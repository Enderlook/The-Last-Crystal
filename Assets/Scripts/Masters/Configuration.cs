using System.Reflection;

using UnityEngine;

namespace Master
{
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake() => StoreGlobals();

        private void StoreGlobals()
        {
            // https://stackoverflow.com/questions/8151888/c-sharp-iterate-through-class-properties
            // Use Fields instead of Properties fixes a bug
            foreach (FieldInfo field in typeof(Configuration).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                // https://stackoverflow.com/questions/3460745/setting-properties-with-reflection-on-static-classes or typeof(Global), whatever works...
                // https://stackoverflow.com/questions/7334067/how-to-get-fields-and-their-values-from-a-static-class-in-referenced-assembly
                typeof(Global).GetField(field.Name, BindingFlags.Public | BindingFlags.Static)?.SetValue(typeof(Global), field.GetValue(this));
            }

            //Global.SetCoinMeter(coinMeter, startingMoney);
            Global.TransformCreature.Crystal.SetTransform(crystal);
        }
    }
}
