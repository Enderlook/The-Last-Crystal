using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchVector3Int), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchVector3Int))]
    public class PolySwitchVector3Int : PolySwitch<Vector3Int> { }
}