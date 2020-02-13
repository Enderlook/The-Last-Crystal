using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(ColorConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Color))]
    public class ColorConstant : AtomConstant<Color> { }
}
