using UnityEngine;

namespace Additions.Serializables.Atoms
{
    public abstract class AtomVariableResettable<T> : AtomVariable<T>, IReseteable
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Initial value of this variable.")]
        private T initialValue;
#pragma warning restore CS0649

        /// <summary>
        /// <see cref="initialValue"/> as property.
        /// </summary>
        public T InitialValuie => initialValue;

        /// <summary>
        /// Reset the value of <see cref="Value"/> using <see cref="initialValue"/>.
        /// </summary>
        public void Reset() => Value = initialValue;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnEnable() => value = initialValue;
    }
}