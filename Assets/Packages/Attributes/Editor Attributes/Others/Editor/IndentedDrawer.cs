using UnityEditor;

using UnityEngine;

namespace Additions.Attributes
{
    [CustomPropertyDrawer(typeof(IndentedAttribute))]
    internal class IndentedDrawer : AdditionalPropertyDrawer
    {
        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label)
        {
            IndentedAttribute indentedAttribute = (IndentedAttribute)attribute;
            int indentation = indentedAttribute.indentationOffset;
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.indentLevel += indentation;
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.indentLevel -= indentation;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label);
    }
}
