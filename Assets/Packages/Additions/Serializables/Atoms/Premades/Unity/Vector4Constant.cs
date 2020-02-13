using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(Vector4), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Vector4))]
    public class Vector4Constant : AtomConstant<Vector4> { }
}
