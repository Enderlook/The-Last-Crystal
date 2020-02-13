﻿using System;

using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(DecimalVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Decimal))]
    public class DecimalVariableResettable : AtomVariableResettable<decimal> { }
}
