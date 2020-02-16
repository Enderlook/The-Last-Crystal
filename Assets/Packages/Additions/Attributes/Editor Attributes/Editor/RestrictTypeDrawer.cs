using Additions.Extensions;

using System;

using UnityEditor;

using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Additions.Attributes
{
    [CustomPropertyDrawer(typeof(RestrictTypeAttribute))]
    public class RestrictTypeDrawer : AdditionalPropertyDrawer
    {
        private float? height;
        private bool firstTime = true;

        private Rect GetBoxPosition(string message, Rect position)
        {
            height = GUI.skin.box.CalcHeight(new GUIContent(message), position.width);
            return new Rect(position.x, position.y, position.width, height.Value);
        }

        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label)
        {
            void DrawErrorBox(string message)
            {
                Debug.LogError(message);
                EditorGUI.HelpBox(GetBoxPosition(message, position), message, MessageType.Error);
            }

            height = null;

            if (!((RestrictTypeAttribute)attribute).CheckRestrictionFeasibility(fieldInfo.FieldType, out string errorMessage))
            {
                DrawErrorBox($"Field {property.name} error. {errorMessage}");
                return;
            }

            UnityObject old = property.objectReferenceValue;
            // Compatibility with ScriptableObjectDrawer
            if (!ScriptableObjectDrawer.DrawPropertyFieldIfIsScriptableObject(position, property, label, fieldInfo))
                EditorGUI.PropertyField(position, property, label);
            UnityObject result = property.objectReferenceValue;

            if ((old != result || firstTime) && result != null // We check for differences to avoid wasting perfomance
                && !((RestrictTypeAttribute)attribute).CheckIfTypeIsAllowed(result.GetType(), out errorMessage))
            {
                Debug.LogError($"Field {property.name} error. {errorMessage}");
                property.objectReferenceValue = null;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => height.HasValue ? height.Value : EditorGUI.GetPropertyHeight(property, label, true);
    }
}