﻿using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(Vector2Int), menuName = nameof(Atom) + "/Variables/Constants/" + nameof(Vector2Int))]
    public class Vector2IntConstant : AtomConstant<Vector2Int> { }
}
