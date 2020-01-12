using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace Additions.Attributes
{
    public sealed class HasConfirmationFieldAttribute : PropertyAttribute
    {
        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public readonly string confirmFieldName;

        public HasConfirmationFieldAttribute(string confirmFieldName) => this.confirmFieldName = confirmFieldName;

        /// <summary>
        /// Check if the given attribute is confirmed or not in <paramref name="instance"/>.<br>
        /// Return <see langword="null"/> if the <c>instance.<see cref="confirmFieldName"/></c> wasn't found.
        /// </summary>
        /// <typeparam name="T">Type of data to look for the confirmation field.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/> used to find the field value.</param>
        /// <param name="bindingFlags">Binding flags used to find the field.</param>
        /// <returns>Boolean value of <c>instance.<see cref="confirmFieldName"/></c>. <see langword="null"/> if the field doesn't exist.</returns>
        public bool? IsConfirmed<T>(T instance, BindingFlags bindingFlags)
        {
            FieldInfo confirmationField = typeof(T).GetField(confirmFieldName, bindingFlags);
            return confirmationField != null ? (bool)confirmationField.GetValue(instance) : (bool?)null;
        }

        /// <summary>
        /// Check if the given attribute is confirmed or not in <paramref name="instance"/>.<br>
        /// Return <see langword="null"/> if the <c>instance.<see cref="confirmFieldName"/></c> wasn't found.
        /// </summary>
        /// <typeparam name="T">Type of data to look for the confirmation field.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/> used to find the field value.</param>
        /// <returns>Boolean value of <c>instance.<see cref="confirmFieldName"/></c>. <see langword="null"/> if the field doesn't exist.</returns>
        public bool? IsConfirmed<T>(T instance) => IsConfirmed(instance, bindingFlags);

        /// <summary>
        /// Get all fields from <typeparamref name="T"/> type in <paramref name="instance"/> which has <see cref="HasConfirmationFieldAttribute"/>.
        /// </summary>
        /// <typeparam name="T">Type of data to look for fields.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/> used to find fields.</param>
        /// <param name="bindingFlags">Binding flags used to find fields.</param>
        /// <returns>Field and its confirmation attribute</returns>
        public static IEnumerable<(FieldInfo field, HasConfirmationFieldAttribute confirmationAttribute)> GetFieldsWithConfirmationAttribute<T>(T instance, BindingFlags bindingFlags)
        {
            Type type = instance.GetType();
            foreach (FieldInfo field in type.GetFields(bindingFlags))
            {
                Attribute attribute = field.GetCustomAttribute(typeof(HasConfirmationFieldAttribute), true);
                if (attribute != null)
                    yield return (field, (HasConfirmationFieldAttribute)attribute);
            }
        }

        /// <summary>
        /// Get all fields from <typeparamref name="T"/> type in <paramref name="instance"/> which has <see cref="HasConfirmationFieldAttribute"/>.
        /// </summary>
        /// <typeparam name="T">Type of data to look for fields.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/> used to find fields.</param>
        /// <returns>Field and its confirmation attribute</returns>
        public static IEnumerable<(FieldInfo field, HasConfirmationFieldAttribute confirmationAttribute)> GetFieldsWithConfirmationAttribute<T>(T instance) => GetFieldsWithConfirmationAttribute(instance, bindingFlags);

        /// <summary>
        /// Get all fields from <typeparamref name="T"/> type in <paramref name="instance"/> which has <see cref="HasConfirmationFieldAttribute"/> and is <see langword="true"/>.
        /// </summary>
        /// <typeparam name="T">Type of data to look for fields.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/> used to find fields.</param>
        /// <param name="bindingFlags">Binding flags used to find fields.</param>
        /// <returns>Fields which attribute are <see langword="true"/></returns>
        public static IEnumerable<FieldInfo> GetConfirmedFields<T>(T instance, BindingFlags bindingFlags)
        {
            foreach ((FieldInfo field, HasConfirmationFieldAttribute confirmationAttribute) in GetFieldsWithConfirmationAttribute(instance, bindingFlags))
            {
                if (confirmationAttribute.IsConfirmed(instance, bindingFlags) == true)
                {
                    yield return field;
                }
            }
        }

        /// <summary>
        /// Get all fields from <typeparamref name="T"/> type in <paramref name="instance"/> which has <see cref="HasConfirmationFieldAttribute"/> and is <see langword="true"/>.
        /// </summary>
        /// <typeparam name="T">Type of data to look for fields.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/> used to find fields.</param>
        /// <returns>Fields which attribute are <see langword="true"/></returns>
        public static IEnumerable<FieldInfo> GetConfirmedFields<T>(T instance) => GetConfirmedFields(instance, bindingFlags);
    }
}