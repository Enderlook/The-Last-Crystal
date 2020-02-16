using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(RectConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Rect))]
    public class RectConstant : AtomConstant<Rect> { }
}
