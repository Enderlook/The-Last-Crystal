using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(LongPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "Long")]
    public class LongPolySwitchGet : AtomPolySwitchGetter<long> { }
}