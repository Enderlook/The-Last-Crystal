using System;

using UnityEngine;

namespace Additions.Serializables.Atoms
{
    [Serializable]
    public abstract class Atom : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField, Multiline, Tooltip("Description of this specific atom.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used to store a description.")]
        private string developerDescription;
#endif
    }
}