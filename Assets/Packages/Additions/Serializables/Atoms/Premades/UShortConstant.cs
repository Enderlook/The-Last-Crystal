using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(UShortConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "UShort")]
    public class UShortConstant : AtomConstant<ushort> { }
}
