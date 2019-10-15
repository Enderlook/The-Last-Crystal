using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityEditorHelper
{
    public static class SerializedPropertyExtensions
    {
        // https://github.com/lordofduct/spacepuppy-unity-framework/blob/master/SpacepuppyBaseEditor/EditorHelper.cs

        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private static object GetValue(this object source, string name)
        {
            if (source == null)
                return null;
            Type type = source.GetType();

            while (type != null)
            {
                FieldInfo fieldInfo = type.GetField(name, bindingFlags);
                if (fieldInfo != null)
                    return fieldInfo.GetValue(source);

                PropertyInfo propertyInfo = type.GetProperty(name, bindingFlags | BindingFlags.IgnoreCase);
                if (propertyInfo != null)
                    return propertyInfo.GetValue(source, null);
            }

            return null;
        }

        private static object GetValue(this object source, string name, int index)
        {
            if (!(source.GetValue(name) is IEnumerable enumerable))
                return null;

            IEnumerator enumerator = enumerable.GetEnumerator();

            for (int i = 0; i <= index; i++)
            {
                if (!enumerator.MoveNext())
                    return null;
            }
            return enumerator.Current;
        }

        /// <summary>
        /// Gets the target object of <paramref name="source"/>. It does work for nested serialized properties.
        /// </summary>
        /// <param name="source"><see cref="SerializedProperty"/> whose value will be get.</param>
        /// <returns>Value of the <paramref name="source"/> as <see cref="object"/>.</returns>
        public static object GetTargetObjectOfProperty(this SerializedProperty source)
        {
            if (source == null)
                return null;

            string path = source.propertyPath.Replace(".Array.data[", "[");
            object targetObject = source.serializedObject.targetObject;

            string[] elements = path.Split('.');
            foreach (string element in elements)
            {
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("["));
                    int index = int.Parse(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    targetObject = targetObject.GetValue(elementName, index);
                }
                else
                    targetObject = targetObject.GetValue(element);
            }

            return targetObject;
        }

        /// <summary>
        /// Produce a <see cref="GUIContent"/> with the <see cref="SerializedProperty.displayName"/> as <see cref="GUIContent.text"/> and <see cref="SerializedProperty.tooltip"/> as <see cref="GUIContent.tooltip"/>.
        /// </summary>
        /// <param name="source">><see cref="SerializedProperty"/> to get <see cref="GUIContent"/>.</param>
        /// <returns><see cref="GUIContent"/> of <paramref name="source"/>.</returns>
        public static GUIContent GetGUIContent(this SerializedProperty source) => new GUIContent(source.displayName, source.tooltip);
    }
}