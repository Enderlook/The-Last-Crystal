using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchInt), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchInt))]
    public class PolySwitchInt : PolySwitch<int> { }
}