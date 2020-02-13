﻿using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(UIntVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + "UInt")]
    public class UIntVariableResettable : AtomVariableResettable<uint> { }
}
