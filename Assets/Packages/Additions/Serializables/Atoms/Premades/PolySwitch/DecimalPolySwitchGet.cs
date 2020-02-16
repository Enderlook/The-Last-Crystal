using Additions.Serializables.PolySwitcher;

using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(DecimalPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Decimal))]
    public class DecimalPolySwitchGet : AtomPolySwitchGetter<PolySwitchDecimal, decimal> { }
}