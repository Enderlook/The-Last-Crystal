using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchVector2Int), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchVector2Int))]
    public class PolySwitchVector2Int : PolySwitch<Vector2Int> { }
}