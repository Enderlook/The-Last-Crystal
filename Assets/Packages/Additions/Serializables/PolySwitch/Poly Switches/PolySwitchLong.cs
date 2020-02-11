using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchLong), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchLong))]
    public class PolySwitchLong : PolySwitch<long> { }
}