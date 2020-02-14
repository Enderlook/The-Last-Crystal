using System;
using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace Additions.Attributes
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    internal class ScriptableObjectDrawer : AdditionalPropertyDrawer
    {
        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label) => DrawPropiertyField(position, property, label, fieldInfo);

        public static void DrawPropiertyField(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            GUIContent buttonLabel = new GUIContent("+", "Open Scriptable Object Menu.");
            float buttonWidth = GUI.skin.button.CalcSize(buttonLabel).x;
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - buttonWidth, position.height), property, label);
            if (GUI.Button(new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height), buttonLabel))
                ScriptableObjectWindow.CreateWindow(property, fieldInfo);
        }

        /// <summary>
        /// Draw property field if <paramref name="fieldInfo"/> type or element type is <see cref="ScriptableObject"/>.
        /// </summary>
        /// <param name="position">Position to draw property field.</param>
        /// <param name="property">Property field to draw.</param>
        /// <param name="label">Label of property.</param>
        /// <param name="fieldInfo">Field info property.</param>
        /// <returns>Whenever it drew the property field or not.</returns>
        public static bool DrawPropertyFieldIfIsScriptableObject(Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            Type type = fieldInfo.FieldType;
            if (type.IsArray)
                type = type.GetElementType();
            if (type.IsSubclassOf(typeof(ScriptableObject)))
            {
                DrawPropiertyField(position, property, label, fieldInfo);
                return true;
            }
            return false;
        }
    }
}
