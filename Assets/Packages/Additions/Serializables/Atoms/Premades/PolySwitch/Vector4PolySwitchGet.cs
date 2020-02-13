using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(Vector3PolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Vector4))]
    public class Vector4PolySwitchGet : AtomPolySwitchGetter<Vector4> { }
}