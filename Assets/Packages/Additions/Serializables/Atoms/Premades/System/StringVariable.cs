using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(StringVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(String))]
    public class StringVariable : AtomVariable<string> { }
}
