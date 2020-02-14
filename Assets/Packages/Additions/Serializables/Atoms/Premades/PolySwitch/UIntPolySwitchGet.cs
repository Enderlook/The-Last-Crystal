using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(UIntPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "UInt")]
    public class UIntPolySwitchGet : AtomPolySwitchGetter<uint> { }
}