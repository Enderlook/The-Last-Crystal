using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(ShortVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "Short")]
    public class ShortVariable : AtomVariable<short> { }
}
