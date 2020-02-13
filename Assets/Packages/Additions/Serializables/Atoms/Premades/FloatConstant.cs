using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(FloatConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "Float")]
    public class FloatConstant : AtomConstant<float> { }
}
