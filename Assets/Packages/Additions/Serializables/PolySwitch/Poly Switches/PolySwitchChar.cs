using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchChar), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchChar))]
    public class PolySwitchChar : PolySwitch<char> { }
}