﻿using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchVector4), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchVector4))]
    public class PolySwitchVector4 : PolySwitch<Vector4> { }
}