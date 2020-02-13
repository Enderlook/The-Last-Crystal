using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(Vector2Int), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Vector2Int))]
    public class Vector2IntVariableResettable : AtomVariableResettable<Vector2Int> { }
}
