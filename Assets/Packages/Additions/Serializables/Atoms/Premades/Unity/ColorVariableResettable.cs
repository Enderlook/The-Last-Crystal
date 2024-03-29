﻿using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(ColorVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Color))]
    public class ColorVariableResettable : AtomVariableResettable<Color> { }
}
