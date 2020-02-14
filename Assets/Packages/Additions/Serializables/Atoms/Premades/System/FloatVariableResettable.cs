using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(FloatVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "Float")]
    public class FloatVariableResettable : AtomVariableResettable<float> { }
}
