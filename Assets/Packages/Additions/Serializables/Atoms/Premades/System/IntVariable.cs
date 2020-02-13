using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(IntVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "Int")]
    public class IntVariable : AtomVariable<int> { }
}
