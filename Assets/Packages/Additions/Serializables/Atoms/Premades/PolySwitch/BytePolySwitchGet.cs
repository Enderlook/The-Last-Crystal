using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(BytePolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "Byte")]
    public class BytePolySwitchGet : AtomPolySwitchGetter<byte> { }
}