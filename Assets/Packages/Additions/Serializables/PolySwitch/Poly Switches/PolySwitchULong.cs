using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchULong), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchULong))]
    public class PolySwitchULong : PolySwitch<ulong> { }
}