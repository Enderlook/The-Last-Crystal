using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchLong), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchLong))]
    public class PolySwitchLong : PolySwitch<long> { }
}