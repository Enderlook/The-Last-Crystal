using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(Vector3), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Vector3))]
    public class Vector3Constant : AtomConstant<Vector3> { }
}
