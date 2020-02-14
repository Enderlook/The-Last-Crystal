using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(UShortPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "UShort")]
    public class UShortPolySwitchGet : AtomPolySwitchGetter<ushort> { }
}