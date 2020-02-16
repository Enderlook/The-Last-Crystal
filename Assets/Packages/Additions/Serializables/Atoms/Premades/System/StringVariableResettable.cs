using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(StringVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(String))]
    public class StringVariableResettable : AtomVariableResettable<string> { }
}
