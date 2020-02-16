using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(RectVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Rect))]
    public class RectVariableResettable : AtomVariableResettable<Rect> { }
}
