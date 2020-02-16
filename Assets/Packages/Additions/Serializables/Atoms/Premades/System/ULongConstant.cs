using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = "ULong", menuName = nameof(Atom) + "/Variables/Constants/" + "ULong")]
    public class ULongConstant : AtomConstant<ulong> { }
}
