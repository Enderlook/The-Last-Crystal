using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(ByteVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "Byte")]
    public class ByteVariableResettable : AtomVariableResettable<byte> { }
}
