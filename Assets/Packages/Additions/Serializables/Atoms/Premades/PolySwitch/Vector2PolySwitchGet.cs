﻿using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(Vector2PolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Vector2))]
    public class Vector2PolySwitchGet : AtomPolySwitchGetter<Vector2> { }
}