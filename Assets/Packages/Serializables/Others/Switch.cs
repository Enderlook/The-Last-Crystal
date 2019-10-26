using System;
using AdditionalAttributes;
using UnityEngine;

namespace Serializables
{
    [Serializable]
    public class Switch<T1, T2>
    {
        [SerializeField, Tooltip("Whenever it should use Item2, instead of Item1.")]
        private bool useAlternative;
        [SerializeField, ShowIf(nameof(useAlternative)), Tooltip("Value used if Use Alternative is false.")]
        private T1 item1;
        [SerializeField, ShowIf(nameof(useAlternative)), Tooltip("Value used if Use Alternative is true.")]
        private T2 item2;

        private readonly string invalidOperationError = $"Can't read property {{0}} because {nameof(useAlternative)} is {{1}}";

        public bool Alternative => useAlternative;
        public T1 Item1 => Alternative ? throw new InvalidOperationException(string.Format(invalidOperationError, nameof(Item1), true)) : item1;
        public T2 Item2 => Alternative ? item2 : throw new InvalidOperationException(string.Format(invalidOperationError, nameof(Item2), false));
    }
}