using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(LongVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "Long")]
    public class LongVariable : AtomVariable<long> { }
}
