using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [CreateAssetMenu(fileName = nameof(QuaternionVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Quaternion))]
    public class QuaternionVariableResettable : AtomVariableResettable<Quaternion> { }
}
