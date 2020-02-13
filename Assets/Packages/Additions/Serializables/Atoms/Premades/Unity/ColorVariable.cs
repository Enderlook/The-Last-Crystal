using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [CreateAssetMenu(fileName = nameof(ColorVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Color))]
    public class ColorVariable : AtomVariable<Color> { }
}
