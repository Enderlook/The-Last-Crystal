﻿using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, CreateAssetMenu(fileName = nameof(PolySwitchQuaternion), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchQuaternion))]
    public class PolySwitchQuaternion : PolySwitch<Quaternion> { }
}