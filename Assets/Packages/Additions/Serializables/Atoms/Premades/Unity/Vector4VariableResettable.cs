using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [CreateAssetMenu(fileName = nameof(Vector4), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Vector4))]
    public class Vector4VariableResettable : AtomVariableResettable<Vector4> { }
}
