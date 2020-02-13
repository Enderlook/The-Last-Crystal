using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(LongVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "Long")]
    public class LongVariableResettable : AtomVariableResettable<long> { }
}
