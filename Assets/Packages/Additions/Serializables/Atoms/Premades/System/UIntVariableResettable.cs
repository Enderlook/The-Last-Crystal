using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(UIntVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "UInt")]
    public class UIntVariableResettable : AtomVariableResettable<uint> { }
}
