﻿using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.Unity
{
    [Serializable, CreateAssetMenu(fileName = nameof(Vector3Int), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Vector3Int))]
    public class Vector3IntVariable : AtomVariable<Vector3Int> { }
}