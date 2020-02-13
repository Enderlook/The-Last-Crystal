using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(ULongVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "ULong")]
    public class ULongVariableResettable : AtomVariableResettable<ulong> { }
}
