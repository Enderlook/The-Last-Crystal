using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [CreateAssetMenu(fileName = nameof(QuaternionConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Quaternion))]
    public class QuaternionConstant : AtomConstant<Quaternion> { }
}
