using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = "UShort", menuName = nameof(Atom) + "/Variables/Commons/" + "UShort")]
    public class UShortVariable : AtomVariable<ushort> { }
}
