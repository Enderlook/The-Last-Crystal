using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchByte), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchByte))]
    public class PolySwitchByte : PolySwitch<byte> { }
}