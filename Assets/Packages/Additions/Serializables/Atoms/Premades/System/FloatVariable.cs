using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(FloatVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "Float")]
    public class FloatVariable : AtomVariable<float> { }
}
