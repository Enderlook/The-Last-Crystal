using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(Vector3Int), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Vector3Int))]
    public class Vector3IntVariableResettable : AtomVariableResettable<Vector3Int> { }
}
