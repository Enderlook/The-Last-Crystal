using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(ULongVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "ULong")]
    public class ULongVariable : AtomVariable<ulong> { }
}
