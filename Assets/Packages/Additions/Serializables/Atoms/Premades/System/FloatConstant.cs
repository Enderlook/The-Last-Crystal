using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(FloatConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "Float")]
    public class FloatConstant : AtomConstant<float> { }
}
