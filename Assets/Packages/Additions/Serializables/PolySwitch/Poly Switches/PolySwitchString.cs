using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchString), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchString))]
    public class PolySwitchString : PolySwitch<string> { }
}