using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(SBytePolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "SByte")]
    public class SBytePolySwitchGet : AtomPolySwitchGetter<sbyte> { }
}