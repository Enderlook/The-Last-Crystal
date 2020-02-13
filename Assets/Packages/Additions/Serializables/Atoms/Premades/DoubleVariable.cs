using System;

using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(DoubleVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Double))]
    public class DoubleVariable : AtomVariable<double> { }
}
