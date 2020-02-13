using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(IntPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "Int")]
    public class IntPolySwitchGet : AtomPolySwitchGetter<int> { }
}