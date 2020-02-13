using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = "ULong", menuName = nameof(Atom) + "/Variables/Constants/" + "ULong")]
    public class ULongConstant : AtomConstant<ulong> { }
}
