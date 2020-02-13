using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(SByteConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "SByte")]
    public class SByteConstant : AtomConstant<sbyte> { }
}
