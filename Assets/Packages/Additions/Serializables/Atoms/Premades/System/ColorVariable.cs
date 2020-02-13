using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(ColorVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Color))]
    public class ColorVariable : AtomVariable<Color> { }
}
