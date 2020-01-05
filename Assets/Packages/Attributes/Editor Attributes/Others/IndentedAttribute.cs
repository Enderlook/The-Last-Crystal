using System;

using UnityEngine;

namespace AdditionalAttributes
{
    /// <summary>
    /// Add or remove indentation to the drew serialized property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class IndentedAttribute : PropertyAttribute
    {
        /// <summary>
        /// Indentation to add.
        /// </summary>
        public int IndentationOffset { get; }

        /// <summary>
        /// Add or remove indentation to the drew serialized property.
        /// </summary>
        /// <param name="indentationOffset">Indentation to add. Negative values remove indentation.</param>
        public IndentedAttribute(int indentationOffset = 1) => IndentationOffset = indentationOffset;
    }
}