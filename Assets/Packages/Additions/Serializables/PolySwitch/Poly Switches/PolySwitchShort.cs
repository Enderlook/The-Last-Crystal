using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchShort), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchShort))]
    public class PolySwitchShort : PolySwitch<short> { }
}