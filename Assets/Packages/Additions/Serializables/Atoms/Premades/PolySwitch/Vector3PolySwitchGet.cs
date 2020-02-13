using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(Vector3PolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Vector3))]
    public class Vector3PolySwitchGet : AtomPolySwitchGetter<Vector3> { }
}