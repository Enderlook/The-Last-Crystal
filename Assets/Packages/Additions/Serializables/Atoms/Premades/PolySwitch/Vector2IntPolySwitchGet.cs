using Additions.Serializables.PolySwitcher;

using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(Vector2IntPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Vector2Int))]
    public class Vector2IntPolySwitchGet : AtomPolySwitchGetter<PolySwitchVector2Int, Vector2Int> { }
}