using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchRect), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchRect))]
    public class PolySwitchRect : PolySwitch<Rect> { }
}