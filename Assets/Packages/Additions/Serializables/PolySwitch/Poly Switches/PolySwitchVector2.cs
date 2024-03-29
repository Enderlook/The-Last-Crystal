﻿using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchVector2), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchVector2))]
    public class PolySwitchVector2 : PolySwitch<Vector2> { }
}