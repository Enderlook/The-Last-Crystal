using UnityEditor;
using UnityEngine;
using UnityEditorHelper;
using System.Reflection;

namespace AdditionalAttributes
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    internal class ScriptableObjectDrawer : AdditionalPropertyDrawer
    {
        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIContent buttonLabel = new GUIContent("Create", "Create instance of the Scriptable Object");
            float buttonWidth = GUI.skin.button.CalcSize(buttonLabel).x;
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - buttonWidth, position.height), property, label);
            if (GUI.Button(new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height), buttonLabel))
            {
                FieldInfo fieldInfo = property.GetFieldInfo();
                fieldInfo.SetValue(fieldInfo, ScriptableObject.CreateInstance(fieldInfo.FieldType));
            }
        }
    }
}
