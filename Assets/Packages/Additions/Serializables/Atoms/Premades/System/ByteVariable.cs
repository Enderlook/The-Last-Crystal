using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(ByteVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "Byte")]
    public class ByteVariable : AtomVariable<byte> { }
}
