using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(UIntVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "UInt")]
    public class UIntVariable : AtomVariable<uint> { }
}
