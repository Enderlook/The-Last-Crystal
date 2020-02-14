using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(ByteVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "Byte")]
    public class ByteVariableResettable : AtomVariableResettable<byte> { }
}
