using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(ShortVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "Short")]
    public class ShortVariableResettable : AtomVariableResettable<short> { }
}
