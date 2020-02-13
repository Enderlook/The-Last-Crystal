using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(Vector3Int), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Vector3Int))]
    public class Vector3IntConstant : AtomConstant<Vector3Int> { }
}
