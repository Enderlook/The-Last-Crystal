using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AdditionalAttributes
{
    public sealed class HasConfirmationFieldAttribute : PropertyAttribute
    {
        public string ConfirmFieldName { get; private set; }

        public HasConfirmationFieldAttribute(string confirmFieldName) => ConfirmFieldName = confirmFieldName;

        /// <summary>
        /// Check if the given attribute is confirmed or not in <paramref name="instance"/>.<br>
        /// Return <see langword="null"/> if the <c>instance.<see cref="ConfirmFieldName"/></c> wasn't found.
        /// </summary>
        /// <typeparam name="T">Type of data to look for the confirmation field.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/> used to find the field value.</param>
        /// <param name="bindingFlags">Binding flags used to find the field.</param>
        /// <returns>Boolean value of <c>instance.<see cref="ConfirmFieldName"/></c>. <see langword="null"/> if the field doesn't exist.</returns>
        public bool? IsConfirmed<T>(T instance, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            FieldInfo confirmationField = typeof(T).GetField(ConfirmFieldName, bindingFlags);
            return confirmationField != null ? (bool)confirmationField.GetValue(instance) : (bool?)null;
        }

        /// <summary>
        /// Get all fields from <typeparamref name="T"/> type in <paramref name="instance"/> which has <see cref="HasConfirmationFieldAttribute"/>.
        /// </summary>
        /// <typeparam name="T">Type of data to look for fields.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/> used to find fields.</param>
        /// <param name="bindingFlags">Binding flags used to find fields.</param>
        /// <returns>Field and its confirmation attribute</returns>
        public static IEnumerable<(FieldInfo field, HasConfirmationFieldAttribute confirmationAttribute)> GetFieldsWithConfirmationAttribute<T>(T instance, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
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
        /// Get all fields from <typeparamref name="T"/> type in <paramref name="instance"/> which has <see cref="HasConfirmationFieldAttribute"/> and is <see langword="true"/>.
        /// </summary>
        /// <typeparam name="T">Type of data to look for fields.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/> used to find fields.</param>
        /// <param name="bindingFlags">Binding flags used to find fields.</param>
        /// <returns>Fields which attribute are <see langword="true"/></returns>
        public static IEnumerable<FieldInfo> GetConfirmedFields<T>(T instance, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            foreach ((FieldInfo field, HasConfirmationFieldAttribute confirmationAttribute) in GetFieldsWithConfirmationAttribute(instance, bindingFlags))
            {
                if (confirmationAttribute.IsConfirmed(instance, bindingFlags) == true)
                {
                    yield return field;
                }
            }
        }
    }
}