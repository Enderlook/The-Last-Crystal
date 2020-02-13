using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(FloatPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "Float")]
    public class FloatPolySwitchGet : AtomPolySwitchGetter<float> { }
}