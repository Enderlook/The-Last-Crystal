using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(QuaternionConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Quaternion))]
    public class QuaternionConstant : AtomConstant<Quaternion> { }
}
