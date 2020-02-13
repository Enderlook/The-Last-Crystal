using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(Vector3), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Vector3))]
    public class Vector3Variable : AtomVariable<Vector3> { }
}
