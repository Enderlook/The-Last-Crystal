using Additions.Serializables.PolySwitcher;

using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(DoublePolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Double))]
    public class DoublePolySwitchGet : AtomPolySwitchGetter<PolySwitchDouble, double> { }
}