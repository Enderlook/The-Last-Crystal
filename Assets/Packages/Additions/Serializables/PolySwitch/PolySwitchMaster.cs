using Additions.Attributes;

using System;

using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = "PolySwitchMaster", menuName = "PolySwitcher/PolySwitchMaster")]
    public class PolySwitchMaster : ScriptableObject
    {
        // Keep members names in sync with PolySwitchMasterEditor

        [field: SerializeField, Tooltip("Amount of indexes."), IsProperty, Min(1)]
        public int Indexes { get; private set; } = 1;

        [field: SerializeField, Tooltip("Current index. Must be number between 0 and indexes,"), IsProperty, Min(0)]
        // This value is zero-based index, but in the inspector is shown as one-based index.
        internal int Index { get; private set; } = 1;

        /// <summary>
        /// Current difficulty level, from
        /// </summary>
        public int CurrentIndex {
            get => Index + 1;
            set {
                if (value <= 1 || value > Indexes)
                    throw new ArgumentOutOfRangeException($"Must be a number between 1 and {nameof(Indexes)} ({Indexes}). Was {value}.");
                Index = value - 1;
            }
        }
    }
}