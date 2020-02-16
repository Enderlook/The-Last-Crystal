using Additions.Serializables.PolySwitcher;

using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(ULongPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "ULong")]
    public class ULongPolySwitchGet : AtomPolySwitchGetter<PolySwitchULong, ulong> { }
}