using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(IntVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "Int")]
    public class IntVariableResettable : AtomVariableResettable<int> { }
}
