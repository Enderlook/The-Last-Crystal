using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(LongVariableResettable), menuName = nameof(Atom) + "/Variables/Constants/" + "Long")]
    public class LongConstant : AtomConstant<long> { }
}
