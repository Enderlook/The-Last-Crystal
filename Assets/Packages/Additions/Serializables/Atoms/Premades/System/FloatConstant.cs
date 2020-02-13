using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(FloatConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "Float")]
    public class FloatConstant : AtomConstant<float> { }
}
