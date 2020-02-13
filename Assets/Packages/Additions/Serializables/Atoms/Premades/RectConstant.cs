using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(RectConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Rect))]
    public class RectConstant : AtomConstant<Rect> { }
}
