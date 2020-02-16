using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(Vector2), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Vector2))]
    public class Vector2Variable : AtomVariable<Vector2> { }
}
