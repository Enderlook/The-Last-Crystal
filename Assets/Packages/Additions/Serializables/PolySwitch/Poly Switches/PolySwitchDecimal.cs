using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchDecimal), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchDecimal))]
    public class PolySwitchDecimal : PolySwitch<decimal> { }
}