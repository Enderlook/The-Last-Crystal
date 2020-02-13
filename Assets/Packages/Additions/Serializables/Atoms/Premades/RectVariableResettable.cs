using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(RectVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Rect))]
    public class RectVariableResettable : AtomVariableResettable<Rect> { }
}
