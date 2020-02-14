using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchUInt), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchUInt))]
    public class PolySwitchUInt : PolySwitch<uint> { }
}