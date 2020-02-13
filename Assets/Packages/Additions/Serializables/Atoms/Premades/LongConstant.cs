using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(LongVariableResettable), menuName = nameof(Atom) + "/Variables/Constants/" + "Long")]
    public class LongConstant : AtomConstant<long> { }
}
