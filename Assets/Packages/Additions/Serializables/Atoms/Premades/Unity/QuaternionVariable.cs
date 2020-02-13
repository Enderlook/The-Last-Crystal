using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [CreateAssetMenu(fileName = nameof(QuaternionVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Quaternion))]
    public class QuaternionVariable : AtomVariable<Quaternion> { }
}
