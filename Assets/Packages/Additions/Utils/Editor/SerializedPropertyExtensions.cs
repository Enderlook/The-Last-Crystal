using Additions.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace Additions.Utils.UnityEditor
{
    public static class SerializedPropertyExtensions
    {
        // https://github.com/lordofduct/spacepuppy-unity-framework/blob/master/SpacepuppyBaseEditor/EditorHelper.cs

        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private static (Func<object> get, Action<object> set) GetAccessors(this object source, string name)
        {
            if (source == null)
                return default;
            Type type = source.GetType();

            while (type != null)
            {
                FieldInfo fieldInfo = type.GetField(name, bindingFlags);
                if (fieldInfo != null)
                    return (() => fieldInfo.GetValue(source), (object value) => fieldInfo.SetValue(source, value));

                PropertyInfo propertyInfo = type.GetProperty(name, bindingFlags | BindingFlags.IgnoreCase);
                if (propertyInfo != null)
                    return (() => propertyInfo.GetValue(source, null), (object value) => propertyInfo.SetValue(source, value, null));

                type = type.BaseType;
            }
            return default;
        }

        private static (Func<object> get, Action<object> set) GetAccessors(this object source, string name, int index)
        {
            object obj = source.GetValue(name);

            if (obj is Array array)
                return (() => array.GetValue(index), (object value) => array.SetValue(value, index));

            if (!(obj is IEnumerable enumerable))
                return default;

            IEnumerator enumerator = enumerable.GetEnumerator();
            return (() =>
            {
                for (int i = 0; i <= index; i++)
                {
                    if (!enumerator.MoveNext())
                        throw new ArgumentOutOfRangeException($"{name} field from {source.GetType()} doesn't have an element at index {index}.");
                }
                return enumerator.Current;
            }, null);
        }

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

                PropertyInfo propertyInfo = type.GetProperty(name, bindingFlags);
                if (propertyInfo != null)
                    return propertyInfo.GetValue(source, null);

                type = type.BaseType;
            }
            return null;
        }

        private static object GetValue(this object source, string name, int index)
        {
            object obj = source.GetValue(name);

            if (obj is Array array)
                return array.GetValue(index);

            if (!(obj is IEnumerable enumerable))
                return null;

            IEnumerator enumerator = enumerable.GetEnumerator();

            for (int i = 0; i <= index; i++)
            {
                if (!enumerator.MoveNext())
                    throw new ArgumentOutOfRangeException($"{name} field from {source.GetType()} doesn't have an element at index {index}.");
            }
            return enumerator.Current;
        }

        /// <summary>
        /// Gets the target object hierarchy of <paramref name="source"/>. It does work for nested serialized properties.
        /// </summary>
        /// <param name="source"><see cref="SerializedProperty"/> whose value will be get.</param>
        /// <param name="includeItself">If <see langword="true"/> the first returned element will be <c><paramref name="source"/>.serializedObject.targetObject</c>.</param>
        /// <param name="preferNullInsteadOfException">If <see langword="true"/>, it will return <see langword="null"/> instead of throwing exceptions if can't find objects.</param>
        /// <returns>Hierarchy traveled to get the target object.</returns>
        public static IEnumerable<object> GetEnumerableTargetObjectOfProperty(this SerializedProperty source, bool includeItself = true, bool preferNullInsteadOfException = true)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Method();

            IEnumerable<object> Method()
            {
                string path = source.propertyPath.Replace(".Array.data[", "[");

                object obj = source.serializedObject.targetObject;

                if (includeItself)
                    yield return obj;

                void NotFound(string element) => throw new KeyNotFoundException($"The element {element} was not found in {obj.GetType()} from {source.name} in path {path}.");

                foreach (string element in path.Split('.'))
                {
                    if (element.Contains("["))
                    {
                        string elementName = element.Substring(0, element.IndexOf("["));
                        int index = int.Parse(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                        try
                        {
                            obj = obj.GetValue(elementName, index);
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            if (!preferNullInsteadOfException)
                                throw new IndexOutOfRangeException($"The element {element} has no index {index} in {obj.GetType()} from {source.name} in path {path}.", e);
                        }
                        if (obj == null && !preferNullInsteadOfException)
                            NotFound(element);
                        yield return obj;
                    }
                    else
                    {
                        obj = obj.GetValue(element);
                        if (obj == null && !preferNullInsteadOfException)
                            NotFound(element);
                        yield return obj;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the target object of <paramref name="source"/>. It does work for nested serialized properties.
        /// </summary>
        /// <param name="source"><see cref="SerializedProperty"/> whose value will be get.</param>
        /// <param name="last">At which depth from last to first should return.</param>
        /// If it doesn't have parent it will return itself.</param>
        /// <returns>Value of the <paramref name="source"/> as <see cref="object"/>.</returns>
        public static object GetTargetObjectOfProperty(this SerializedProperty source, int last = 0) => source.GetEnumerableTargetObjectOfProperty().Reverse().Skip(last).First();

        /// <summary>
        /// Gets the parent target object of <paramref name="source"/>. It does work for nested serialized properties.<br>
        /// If it doesn't have parent it will return itself.
        /// </summary>
        /// <param name="source"><see cref="SerializedProperty"/> whose value will be get.</param>
        /// If it doesn't have parent it will return itself.</param>
        /// <returns>Value of the <paramref name="source"/> as <see cref="object"/>.</returns>
        public static object GetParentTargetObjectOfProperty(this SerializedProperty source) => source.GetTargetObjectOfProperty(1);

        /// <summary>
        /// Get the getter and setter of <paramref name="source"/>. It does work for nested serialized properties.<br>
        /// </summary>
        /// <param name="source"><see cref="SerializedProperty"/> whose getter and setter will be get.</param>
        /// <returns>Getter and setter of the <paramref name="source"/>.</returns>
        public static (Func<object> get, Action<object> set) GetTargetObjectAccessors(this SerializedProperty source)
        {
            object parent = source.GetParentTargetObjectOfProperty();
            Type parentType = parent.GetType();

            string element = source.propertyPath.Replace(".Array.data[", "[").Split('.').Last();
            if (element.Contains("["))
            {
                string elementName = element.Substring(0, element.IndexOf("["));
                int index = int.Parse(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                try
                {
                    return parent.GetAccessors(elementName, index);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    throw new IndexOutOfRangeException($"The element {element} has no index {index} in {parentType} from {source.name} in path {element}.", e);
                }
            }
            else
                return parent.GetAccessors(element);
        }

        /// <summary>
        /// Produce a <see cref="GUIContent"/> with the <see cref="SerializedProperty.displayName"/> as <see cref="GUIContent.text"/> and <see cref="SerializedProperty.tooltip"/> as <see cref="GUIContent.tooltip"/>.
        /// </summary>
        /// <param name="source">><see cref="SerializedProperty"/> to get <see cref="GUIContent"/>.</param>
        /// <returns><see cref="GUIContent"/> of <paramref name="source"/>.</returns>
        public static GUIContent GetGUIContent(this SerializedProperty source) => new GUIContent(source.displayName, source.tooltip);

        /// <summary>
        /// Produce a <see cref="GUIContent"/> with the <see cref="SerializedProperty.displayName"/> as <see cref="GUIContent.text"/> and <see cref="SerializedProperty.tooltip"/> as <see cref="GUIContent.tooltip"/>, but removing the backing field tag.
        /// </summary>
        /// <param name="source">><see cref="SerializedProperty"/> to get <see cref="GUIContent"/>.</param>
        /// <returns><see cref="GUIContent"/> of <paramref name="source"/>.</returns>
        public static GUIContent GetGUIContentOfBackingField(this SerializedProperty source)
        {
            GUIContent guiContent = source.GetGUIContent();
            guiContent.text = guiContent.text.Replace("<", "").Replace(">k__Backing Field", "");
            return guiContent;
        }

        public static SerializedPropertyHelper GetHelper(this SerializedProperty source) => new SerializedPropertyHelper(source);

        /// <summary>
        /// Get the field <see cref="Type"/> of the <see cref="SerializedProperty"/>.
        /// </summary>
        /// <param name="source"><see cref="SerializedProperty"/> whose <see cref="Type"/> will be get.</param>
        /// <returns><see cref="Type"/> of the <paramref name="source"/>.</returns>
        public static Type GetFieldType(this SerializedProperty source) => source.GetTargetObjectOfProperty().GetType();

        /// <summary>
        /// Get the <see cref="FieldInfo"/> of <see cref="SerializedProperty"/>.<br/>
        /// Doesn't work on enumerable <see cref="SerializedProperty"/>.
        /// </summary>
        /// <param name="source"><see cref="SerializedProperty"/> whose <see cref="FieldInfo"/> will be get.</param>
        /// <returns><see cref="FieldInfo"/> of <paramref name="source"/>.</returns>
        public static FieldInfo GetFieldInfo(this SerializedProperty source) => source.serializedObject.targetObject.GetType().GetField(source.name);

        /// <summary>
        /// Get the index of the <paramref name="source"/> if it's an element of an array.
        /// </summary>
        /// <param name="source"><see cref="SerializedProperty"/> element of array.</param>
        /// <returns>Its index.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="source"/> doesn't come from an array.</exception>
        public static int GetIndexFromArray(this SerializedProperty source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            string part = source.propertyPath.Split('.').Last().Split('[').LastOrDefault();
            if (part == default)
                throw new ArgumentException("It doesn't come from an array", nameof(source));
            else
                return int.Parse(part.Replace("]", ""));
        }

        /// <summary>
        /// Get the <see cref="SerializedProperty"/> of the backing field of a property.
        /// </summary>
        /// <param name="source"><see cref="SerializedProperty"/> where the <see cref="SerializedProperty"/> will be taken.</param>
        /// <param name="name">Name of the property which backing field will be get.</param>
        /// <returns><see cref="SerializedProperty"/> of the backing field of <paramref name="name"/> property.</returns>
        public static SerializedProperty FindRelativeBackingFieldOfProperty(this SerializedProperty source, string name)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Can't be empty.", nameof(name));

            return source.FindPropertyRelative(ReflectionExtesions.GetBackingFieldName(name));
        }

        /// <summary>
        /// Get the <see cref="SerializedProperty"/> of the field or backing field of it property.
        /// </summary>
        /// <param name="source"><see cref="SerializedProperty"/> where the <see cref="SerializedProperty"/> will be taken.</param>
        /// <param name="name">Name of the property to get.</param>
        /// <returns><see cref="SerializedProperty"/> of the field or backing field of <paramref name="name"/> property.</returns>
        public static SerializedProperty FindRelativePropertyOrBackingField(this SerializedProperty source, string name)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Can't be empty.", nameof(name));

            SerializedProperty serializedProperty = source.FindPropertyRelative(name);
            if (serializedProperty == null)
                serializedProperty = source.FindPropertyRelative(ReflectionExtesions.GetBackingFieldName(name));
            return serializedProperty;
        }
    }
}