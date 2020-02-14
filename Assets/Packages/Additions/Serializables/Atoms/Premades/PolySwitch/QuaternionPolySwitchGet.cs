using Additions.Serializables.PolySwitcher;

using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(QuaternionPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Quaternion))]
    public class QuaternionPolySwitchGet : AtomPolySwitchGetter<PolySwitchQuaternion, Quaternion> { }
}