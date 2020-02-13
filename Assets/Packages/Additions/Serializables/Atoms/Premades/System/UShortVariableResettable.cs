using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(UShortVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "UShort")]
    public class UShortVariableResettable : AtomVariableResettable<ushort> { }
}
