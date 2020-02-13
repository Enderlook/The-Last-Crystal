using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(QuaternionVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Quaternion))]
    public class QuaternionVariable : AtomVariable<Quaternion> { }
}
