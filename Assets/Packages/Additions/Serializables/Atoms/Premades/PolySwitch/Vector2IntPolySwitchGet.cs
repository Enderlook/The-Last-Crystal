using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(Vector2IntPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Vector2Int))]
    public class Vector2IntPolySwitchGet : AtomPolySwitchGetter<Vector2Int> { }
}