using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(UIntConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "UInt")]
    public class UIntConstant : AtomConstant<uint> { }
}
