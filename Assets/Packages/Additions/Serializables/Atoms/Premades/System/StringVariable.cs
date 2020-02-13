using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(StringVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(String))]
    public class StringVariable : AtomVariable<string> { }
}
