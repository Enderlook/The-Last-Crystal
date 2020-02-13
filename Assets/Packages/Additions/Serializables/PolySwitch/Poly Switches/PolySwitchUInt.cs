using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchUInt), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchUInt))]
    public class PolySwitchUInt : PolySwitch<uint> { }
}