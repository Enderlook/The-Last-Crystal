﻿using Additions.Serializables.PolySwitcher;

using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(Vector3PolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Vector3))]
    public class Vector3PolySwitchGet : AtomPolySwitchGetter<PolySwitchVector3, Vector3> { }
}