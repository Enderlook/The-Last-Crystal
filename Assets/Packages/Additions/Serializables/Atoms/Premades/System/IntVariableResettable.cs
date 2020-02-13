using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(IntVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "Int")]
    public class IntVariableResettable : AtomVariableResettable<int> { }
}
