using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchLayerMask), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchLayerMask))]
    public class PolySwitchLayerMask : PolySwitch<LayerMask> { }
}