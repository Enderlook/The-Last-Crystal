using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(Vector3IntPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Vector3Int))]
    public class Vector3IntPolySwitchGet : AtomPolySwitchGetter<Vector3Int> { }
}