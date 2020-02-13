using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchSByte), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchSByte))]
    public class PolySwitchSByte : PolySwitch<sbyte> { }
}