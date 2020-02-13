using System;

using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(DecimalVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Decimal))]
    public class DecimalVariable : AtomVariable<decimal> { }
}
