using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(ShortVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "Short")]
    public class ShortVariable : AtomVariable<short> { }
}
