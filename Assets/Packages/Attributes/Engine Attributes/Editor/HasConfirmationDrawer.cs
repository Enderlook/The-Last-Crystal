using System.Reflection;
using AdditionalExtensions;
using UnityEditor;
using UnityEditorHelper;
using UnityEngine;

namespace AdditionalAttributes
{
    [CustomPropertyDrawer(typeof(HasConfirmationFieldAttribute))]
    internal class HasConfirmationDrawer : PropertyDrawer
    {
        private bool confirm;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            HasConfirmationFieldAttribute hasConfirmationFieldAttribute = (HasConfirmationFieldAttribute)attribute;

            VerticalRectBuilder verticalRectBuilder = new VerticalRectBuilder(position.x, position.y, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight);

            object targetObject = property.GetParentTargetObjectOfProperty();

            FieldInfo confirmationField = targetObject.GetType().GetField(hasConfirmationFieldAttribute.ConfirmFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            string name = confirmationField.Name.ToDisplayUnity();
            string tooltip = ((TooltipAttribute)confirmationField.GetCustomAttribute(typeof(TooltipAttribute), true))?.tooltip;
            confirm = (bool)confirmationField.GetValue(targetObject);
            confirm = EditorGUI.Toggle(verticalRectBuilder.GetRect(), new GUIContent(name, tooltip ?? ""), confirm);
            confirmationField.SetValue(targetObject, confirm);

            if (confirm)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(verticalRectBuilder.GetRect(), property, label, true);
                EditorGUI.indentLevel--;
            }

            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return confirm ? EditorGUI.GetPropertyHeight(property) + EditorGUIUtility.singleLineHeight : EditorGUIUtility.singleLineHeight;
        }
    }
}