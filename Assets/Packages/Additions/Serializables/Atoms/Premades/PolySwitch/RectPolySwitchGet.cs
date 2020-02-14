using Additions.Serializables.PolySwitcher;

using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable, CreateAssetMenu(fileName = nameof(RectPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + nameof(Rect))]
    public class RectPolySwitchGet : AtomPolySwitchGetter<PolySwitchRect, Rect> { }
}