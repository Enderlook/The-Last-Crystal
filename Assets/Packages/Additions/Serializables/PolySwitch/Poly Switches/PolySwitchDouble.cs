using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchDouble), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchDouble))]
    public class PolySwitchDouble : PolySwitch<double> { }
}