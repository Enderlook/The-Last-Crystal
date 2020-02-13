using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(UIntVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "UInt")]
    public class UIntVariable : AtomVariable<uint> { }
}
