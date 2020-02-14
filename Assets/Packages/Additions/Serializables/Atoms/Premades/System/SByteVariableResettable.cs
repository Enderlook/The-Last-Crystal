using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(SByteVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "SByte")]
    public class SByteVariableResettable : AtomVariableResettable<sbyte> { }
}
