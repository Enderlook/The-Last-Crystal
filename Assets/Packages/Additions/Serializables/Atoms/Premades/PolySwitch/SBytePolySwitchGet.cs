using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(SBytePolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "SByte")]
    public class SBytePolySwitchGet : AtomPolySwitchGetter<sbyte> { }
}