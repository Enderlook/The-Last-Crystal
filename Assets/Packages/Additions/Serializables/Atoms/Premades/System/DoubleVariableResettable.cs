using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(DoubleVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Double))]
    public class DoubleVariableResettable : AtomVariableResettable<double> { }
}
