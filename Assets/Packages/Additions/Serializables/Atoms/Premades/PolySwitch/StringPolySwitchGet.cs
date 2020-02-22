using Additions.Serializables.PolySwitcher;

using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(StringPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(String))]
    public class StringPolySwitchGet : AtomPolySwitchGetter<PolySwitchString, string> { }
}