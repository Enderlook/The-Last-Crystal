using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(RectVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Rect))]
    public class RectVariable : AtomVariable<Rect> { }
}
