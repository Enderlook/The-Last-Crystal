using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(SByteVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "SByte")]
    public class SByteVariable : AtomVariable<sbyte> { }
}
