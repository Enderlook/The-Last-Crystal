using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(QuaternionPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Quaternion))]
    public class QuaternionPolySwitchGet : AtomPolySwitchGetter<Quaternion> { }
}