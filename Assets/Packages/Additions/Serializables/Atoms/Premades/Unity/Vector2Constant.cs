using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(Vector2), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Vector2))]
    public class Vector2Constant : AtomConstant<Vector2> { }
}
