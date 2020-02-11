using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchChar), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchChar))]
    public class PolySwitchChar : PolySwitch<char> { }
}