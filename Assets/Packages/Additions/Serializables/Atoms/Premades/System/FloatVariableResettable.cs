using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(FloatVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "Float")]
    public class FloatVariableResettable : AtomVariableResettable<float> { }
}
