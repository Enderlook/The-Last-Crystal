using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(ColorConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Color))]
    public class ColorConstant : AtomConstant<Color> { }
}
