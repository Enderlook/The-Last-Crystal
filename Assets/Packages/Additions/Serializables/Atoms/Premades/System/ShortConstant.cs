using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(ShortConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "Short")]
    public class ShortConstant : AtomConstant<short> { }
}
