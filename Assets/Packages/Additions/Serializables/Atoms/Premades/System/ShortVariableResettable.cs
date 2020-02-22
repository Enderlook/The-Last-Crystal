using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(ShortVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "Short")]
    public class ShortVariableResettable : AtomVariableResettable<short> { }
}
