using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(ByteConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "Byte")]
    public class ByteConstant : AtomConstant<byte> { }
}
