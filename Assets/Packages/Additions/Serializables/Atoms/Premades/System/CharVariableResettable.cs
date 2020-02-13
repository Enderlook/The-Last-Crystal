using System;

using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.System
{
    [CreateAssetMenu(fileName = nameof(CharVariableResettable), menuName = nameof(Atom) + "/Variables/Resettables/" + nameof(Char))]
    public class CharVariableResettable : AtomVariableResettable<char> { }
}
