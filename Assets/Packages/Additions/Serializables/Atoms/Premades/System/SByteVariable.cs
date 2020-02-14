using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(SByteVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "SByte")]
    public class SByteVariable : AtomVariable<sbyte> { }
}
