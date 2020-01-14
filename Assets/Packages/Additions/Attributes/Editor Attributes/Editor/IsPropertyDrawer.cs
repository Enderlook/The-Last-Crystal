using UnityEditor;

using UnityEngine;

namespace Additions.Attributes
{
    [CustomPropertyDrawer(typeof(IsPropertyAttribute))]
    internal class IsPropertyDrawer : AdditionalPropertyDrawer
    {
        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label)
        {
            label.text = label.text.Replace("<", "").Replace(">k__Backing Field", "");

            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}