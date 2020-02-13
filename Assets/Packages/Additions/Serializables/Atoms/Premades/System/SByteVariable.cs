using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(SByteVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "SByte")]
    public class SByteVariable : AtomVariable<sbyte> { }
}
