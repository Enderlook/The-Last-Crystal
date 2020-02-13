using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(SByteVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "SByte")]
    public class SByteVariableResettable : AtomVariableResettable<sbyte> { }
}
