using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(Vector2Int), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Vector2Int))]
    public class Vector2IntVariable : AtomVariable<Vector2Int> { }
}
