using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(Vector4), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Vector4))]
    public class Vector4Variable : AtomVariable<Vector4> { }
}
