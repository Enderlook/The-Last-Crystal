using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(StringConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(String))]
    public class StringConstant : AtomConstant<string> { }
}
