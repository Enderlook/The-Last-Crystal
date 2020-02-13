using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(QuaternionConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Quaternion))]
    public class QuaternionConstant : AtomConstant<Quaternion> { }
}
