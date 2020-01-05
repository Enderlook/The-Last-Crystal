using AdditionalAttributes;

using System;

using UnityEngine;

namespace Serializables
{
    [Serializable]
    public abstract class Switch<T1, T2>
    {
#pragma warning disable CS0649
        // Don't change to private any of there three fields or Unity Editor Freeze
        [SerializeField, GUI(nameof(UseAlternativeGUIContent))]
        protected bool useAlternative;

        [SerializeField, ShowIf(nameof(Alternative), false), GUI(nameof(Item1GUIContent))]
        protected T1 item1;

        [SerializeField, ShowIf(nameof(Alternative)), GUI(nameof(Item2GUIContent))]
        protected T2 item2;
#pragma warning restore CS0649

        protected abstract GUIContent Item1GUIContent { get; }

        protected abstract GUIContent Item2GUIContent { get; }

        protected abstract GUIContent UseAlternativeGUIContent { get; }

        [NonSerialized]
        private readonly string invalidOperationError = $"Can't read property {{0}} because {nameof(useAlternative)} is {{1}}";

        public bool Alternative => useAlternative;

        public T1 Value1 => Alternative ? throw new InvalidOperationException(string.Format(invalidOperationError, nameof(Value1), true)) : item1;

        public T2 Value2 => Alternative ? item2 : throw new InvalidOperationException(string.Format(invalidOperationError, nameof(Value2), false));
    }
}