using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchColor), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchColor))]
    public class PolySwitchColor : PolySwitch<Color> { }
}