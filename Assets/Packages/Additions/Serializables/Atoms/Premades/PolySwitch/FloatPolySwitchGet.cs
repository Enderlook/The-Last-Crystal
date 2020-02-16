using Additions.Serializables.PolySwitcher;

using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(FloatPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "Float")]
    public class FloatPolySwitchGet : AtomPolySwitchGetter<PolySwitchFloat, float> { }
}