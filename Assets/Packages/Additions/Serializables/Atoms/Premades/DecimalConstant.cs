using System;

using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(DecimalConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Decimal))]
    public class DecimalConstant : AtomConstant<decimal> { }
}
