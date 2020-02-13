using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(ShortVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "Short")]
    public class ShortVariableResettable : AtomVariableResettable<short> { }
}
