using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(IntConstant), menuName = nameof(Atom) + "/Variables/Constants/" + "Int")]
    public class IntConstant : AtomConstant<int> { }
}
