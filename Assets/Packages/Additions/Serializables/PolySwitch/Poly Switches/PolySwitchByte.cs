using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchByte), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchByte))]
    public class PolySwitchByte : PolySwitch<byte> { }
}