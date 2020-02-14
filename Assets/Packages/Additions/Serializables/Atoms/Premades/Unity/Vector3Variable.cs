using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(Vector3), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Vector3))]
    public class Vector3Variable : AtomVariable<Vector3> { }
}
