using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(ByteConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "Byte")]
    public class ByteConstant : AtomConstant<byte> { }
}
