using System;

using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [CreateAssetMenu(fileName = nameof(CharVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Char))]
    public class CharVariableResettable : AtomVariableResettable<char> { }
}
