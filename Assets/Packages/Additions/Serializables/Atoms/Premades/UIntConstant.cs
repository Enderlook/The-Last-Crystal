using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(UIntConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "UInt")]
    public class UIntConstant : AtomConstant<uint> { }
}
