using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(IntConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "Int")]
    public class IntConstant : AtomConstant<int> { }
}
