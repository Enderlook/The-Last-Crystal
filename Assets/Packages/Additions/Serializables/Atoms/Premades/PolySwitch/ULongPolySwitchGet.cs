using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(ULongPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "ULong")]
    public class ULongPolySwitchGet : AtomPolySwitchGetter<ulong> { }
}