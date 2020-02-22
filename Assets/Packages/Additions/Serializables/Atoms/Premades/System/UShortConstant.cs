using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(UShortConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "UShort")]
    public class UShortConstant : AtomConstant<ushort> { }
}
