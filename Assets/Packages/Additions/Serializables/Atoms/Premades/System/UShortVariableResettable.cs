using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(UShortVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "UShort")]
    public class UShortVariableResettable : AtomVariableResettable<ushort> { }
}
