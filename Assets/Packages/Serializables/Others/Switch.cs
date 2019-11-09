using System;
using AdditionalAttributes;
using UnityEngine;

namespace Serializables
{
    [Serializable]
    public class Switch<T1, T2>
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Whenever it should use Item2, instead of Item1.")]
        private bool useAlternative;
        // Don't change to private of Unity Editor Freeze
        [SerializeField, ShowIf(nameof(Alternative), false), Tooltip("Value used if Use Alternative is false.")]
        protected T1 item1;
        [SerializeField, ShowIf(nameof(Alternative)), Tooltip("Value used if Use Alternative is true.")]
        protected T2 item2;
#pragma warning restore CS0649

        [NonSerialized]
        private readonly string invalidOperationError = $"Can't read property {{0}} because {nameof(useAlternative)} is {{1}}";

        public bool Alternative => useAlternative;
        public T1 Value1 => Alternative ? throw new InvalidOperationException(string.Format(invalidOperationError, nameof(Value1), true)) : item1;
        public T2 Value2 => Alternative ? item2 : throw new InvalidOperationException(string.Format(invalidOperationError, nameof(Value2), false));
    }
}