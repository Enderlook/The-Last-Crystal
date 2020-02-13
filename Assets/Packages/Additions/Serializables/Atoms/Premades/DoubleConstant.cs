using System;

using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(DoubleConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Double))]
    public class DoubleConstant : AtomConstant<double> { }
}
