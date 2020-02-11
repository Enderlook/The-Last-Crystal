using Additions.Attributes;
using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [Serializable, AbstractScriptableObject]
    public class PolySwitch<T> : ScriptableObject
    {
#pragma warning disable CA2235, CS0649
        [SerializeField, Tooltip("Difficulty selector."), Expandable]
        private PolySwitchMaster master;
#pragma warning restore CA2235, CS0649

        [SerializeField, Tooltip("Value per difficulty.")]
        private T[] values;

        public T Value => values[master.CurrentIndex];

#if UNITY_EDITOR
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnValidate()
        {
            if (values.Length != master.Indexes)
            {
                Array.Resize(ref values, master.Indexes);
                Debug.LogWarning($"Length of {nameof(values)} in {GetType()} must be equal to {nameof(master)}.{nameof(master.Indexes)}. Was {values.Length} instead of {master.Indexes}.");
            }
        }
#endif
    }
}