using System;
using System.Collections.Generic;
using System.Reflection;
using AdditionalExtensions;
using UnityEditor;

namespace UnityEditorHelper
{
    public static class PropertyDrawerHelper
    {
        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        public static IEnumerable<(SerializedProperty serializedProperty, T field, Editor editor)> FindAllSerializePropertiesInActiveEditorOf<T>()
        {
            foreach (Editor editor in ActiveEditorTracker.sharedTracker.activeEditors)
            {
                SerializedProperty serializedProperty = editor.serializedObject.GetIterator();
                while (serializedProperty.Next(true))
                {
                    UnityEngine.Object targetObject = serializedProperty.serializedObject.targetObject;
                    // Used to skip missing components
                    if (targetObject == null)
                        continue;
                    Type targetObjectClassType = targetObject.GetType();
                    FieldInfo field = targetObjectClassType.GetInheritedField(serializedProperty.propertyPath, bindingFlags);
                    // If the field exist, it's the class type we want
                    if (field != null && field.GetValue(targetObject) is T value)
                        yield return (serializedProperty, value, editor);
                }
            }
        }

        public static IEnumerable<(SerializedProperty serializedProperty, object field, T attribute, Editor editor)> FindAllSerializePropertiesInActiveEditorWithTheAttribute<T>(bool inherit = true) where T : Attribute
        {
            foreach (Editor editor in ActiveEditorTracker.sharedTracker.activeEditors)
            {
                SerializedProperty serializedProperty = editor.serializedObject.GetIterator();
                while (serializedProperty.Next(true))
                {
                    UnityEngine.Object targetObject = serializedProperty.serializedObject.targetObject;
                    // Used to skip missing components
                    if (targetObject == null)
                        continue;
                    Type targetObjectClassType = targetObject.GetType();
                    FieldInfo field = targetObjectClassType.GetInheritedField(serializedProperty.propertyPath, bindingFlags);
                    if (field == null)
                        continue;
                    Attribute attribute = field.GetCustomAttribute(typeof(T), inherit);
                    if (attribute != null && attribute.GetType() == typeof(T))
                        yield return (serializedProperty, field, (T)attribute, editor);
                }
            }
        }
    }
}