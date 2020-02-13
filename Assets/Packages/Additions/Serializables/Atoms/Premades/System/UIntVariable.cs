using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(UIntVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "UInt")]
    public class UIntVariable : AtomVariable<uint> { }
}
