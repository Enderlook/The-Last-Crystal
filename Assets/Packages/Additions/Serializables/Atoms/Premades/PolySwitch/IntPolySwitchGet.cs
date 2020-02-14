using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(IntPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "Int")]
    public class IntPolySwitchGet : AtomPolySwitchGetter<int> { }
}