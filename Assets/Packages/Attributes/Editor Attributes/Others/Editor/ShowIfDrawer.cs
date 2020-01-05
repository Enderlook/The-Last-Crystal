using System;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEditorHelper;

using UnityEngine;

namespace AdditionalAttributes
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    internal class ShowIfDrawer : PropertyDrawer
    {
        private const string attributeName = nameof(ShowIfAttribute);
        private readonly string requireAttribute = $"required by {attributeName} attribute";

        /// <summary>
        /// If <see langword="true"/>, the property field is either disabled or hidden.
        /// </summary>
        private bool active;
        private ShowIfAttribute.ActionMode mode;

        private bool persistentError;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string propertyName = property.name;

            ShowIfAttribute showIfAttribute = (ShowIfAttribute)attribute;
            string conditionName = showIfAttribute.NameOfConditional;
            mode = showIfAttribute.Mode;

            object parent;
            /* Sometimes when an array is resized, the property drawer is renderer before the actual array is resized.
             * That is why we may get error from our custom method GetParentTargetObjectOfProperty.*/
            try
            {
                parent = property.GetParentTargetObjectOfProperty();
            }
            catch (IndexOutOfRangeException) when (!persistentError)
            {
                persistentError = true;
                return;
            }
            persistentError = false;
            string parentType = parent.GetType().Name;

            string sign = $"in {parentType} for serialized property {propertyName}, {requireAttribute}";

            MemberInfo[] memberInfos = parent.GetType().GetMember(conditionName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (memberInfos.Length == 0)
            {
                Debug.LogError($"No member named {conditionName} found in {parentType} for serialized property {propertyName}, {requireAttribute}.");
                return;
            }

            active = false;
            foreach (MemberInfo memberInfo in memberInfos)
            {
                string castingErrorMessage = $"{{0}} in {conditionName} {sign} can't be casted to {typeof(bool)} {sign}. It is {{1}}.";
                switch (memberInfo.MemberType)
                {
                    case MemberTypes.Field:
                        FieldInfo fieldInfo = (FieldInfo)memberInfo;
                        if (fieldInfo.GetValue(parent) is bool fieldValue)
                        {
                            active = fieldValue;
                            goto outsideForeach;
                        }
                        else
                        {
                            Debug.LogException(new ArgumentException(string.Format(castingErrorMessage, "Field", fieldInfo.FieldType.Name)));
                            continue;
                        }
                    case MemberTypes.Property:
                        PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                        try
                        {
                            if (propertyInfo.GetValue(parent) is bool propertyValue)
                            {
                                active = propertyValue;
                                goto outsideForeach;
                            }
                            else
                            {
                                Debug.LogException(new ArgumentException(string.Format(castingErrorMessage, "Property", propertyInfo.PropertyType.Name)));
                                continue;
                            }
                        }
                        catch (ArgumentException e)
                        {
                            Debug.LogException(new ArgumentException($"Property in {conditionName} {sign} doesn't have Get Method", e));
                            continue;
                        }
                    case MemberTypes.Method:
                        MethodInfo methodInfo = (MethodInfo)memberInfo;
                        // Check if the method don't require any mandatory parameter
                        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                        if (parameterInfos.Count(e => !e.IsOptional) == 0)
                        {
                            if (methodInfo.Invoke(parent, parameterInfos.Where(e => e.IsOptional).Select(e => Type.Missing).ToArray()) is bool methodValue)
                            {
                                active = methodValue;
                                goto outsideForeach;
                            }
                            else
                            {
                                Debug.LogException(new ArgumentException(string.Format(castingErrorMessage, "Return type of method", methodInfo.ReturnType.Name)));
                                continue;
                            }
                        }
                        continue;
                }
            }

            Debug.LogException(new Exception($"Field, property (with Get Method) or method (without mandatory parameters and with return) {conditionName} {sign} of type/return type {typeof(bool)} not found."));
            return;

        outsideForeach:
            EditorGUI.BeginProperty(position, label, property);
            void DrawField()
            {
                bool idented = showIfAttribute.indented;
                if (idented)
                    EditorGUI.indentLevel++;
                EditorGUI.PropertyField(position, property, label, true);
                if (idented)
                    EditorGUI.indentLevel--;
            }

            active = active == showIfAttribute.Goal;
            if (mode == ShowIfAttribute.ActionMode.ShowHide)
            {
                if (active)
                    DrawField();
            }
            else
            {
                EditorGUI.BeginDisabledGroup(active);
                DrawField();
                EditorGUI.EndDisabledGroup();
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return mode == ShowIfAttribute.ActionMode.ShowHide && active ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
        }
    }
}