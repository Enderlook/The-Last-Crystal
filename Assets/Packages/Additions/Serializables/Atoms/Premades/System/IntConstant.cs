using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(IntConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "Int")]
    public class IntConstant : AtomConstant<int> { }
}
