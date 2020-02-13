using System;

using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(CharVariable), menuName = nameof(Atom) + "/Variables/Commons/" + nameof(Char))]
    public class CharVariable : AtomVariable<char> { }
}
