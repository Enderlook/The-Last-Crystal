using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(LongVariable), menuName = nameof(Atom) + "/Variables/Commons/" + "Long")]
    public class LongVariable : AtomVariable<long> { }
}
