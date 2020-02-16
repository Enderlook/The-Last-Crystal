using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(DecimalVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Decimal))]
    public class DecimalVariable : AtomVariable<decimal> { }
}
