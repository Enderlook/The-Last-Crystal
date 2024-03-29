﻿using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [Serializable, CreateAssetMenu(fileName = nameof(CharConstant), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Char))]
    public class CharConstant : AtomConstant<char> { }
}
