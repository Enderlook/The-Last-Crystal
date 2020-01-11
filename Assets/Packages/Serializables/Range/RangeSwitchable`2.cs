using UnityEngine;

namespace Additions.Serializables.Ranges
{
    public abstract class RangeSwitchable<T, U> : Switch<T, U>
    {
        private static readonly GUIContent item1GUIContent = new GUIContent("Range", "A random value from this range will be used.");

        private static readonly GUIContent item2GUIContent = new GUIContent("Value", "This value will be used.");

        private static readonly GUIContent useAlternativeGUIContent = new GUIContent("Use Range", "If checked, a random value from a range will be used.");

        protected override GUIContent Item1GUIContent => item1GUIContent;

        protected override GUIContent Item2GUIContent => item2GUIContent;

        protected override GUIContent UseAlternativeGUIContent => useAlternativeGUIContent;
    }
}