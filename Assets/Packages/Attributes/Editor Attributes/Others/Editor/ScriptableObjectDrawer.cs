using UnityEditor;

using UnityEngine;

namespace AdditionalAttributes
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    internal class ScriptableObjectDrawer : AdditionalPropertyDrawer
    {
        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIContent buttonLabel = new GUIContent("+", "Open Scriptable Object Menu.");
            float buttonWidth = GUI.skin.button.CalcSize(buttonLabel).x;
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - buttonWidth, position.height), property, label);
            if (GUI.Button(new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height), buttonLabel))
                ScriptableObjectWindow.CreateWindow(property, fieldInfo);
        }
    }
}
