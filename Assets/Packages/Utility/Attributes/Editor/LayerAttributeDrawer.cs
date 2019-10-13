using UnityEditor;
using UnityEngine;

namespace AdditionalAttributes
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    internal class LayerAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            int layer = EditorGUI.LayerField(position, label, property.intValue);
            if (EditorGUI.EndChangeCheck())
            {
                switch (property.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        property.intValue = layer;
                        break;
                    case SerializedPropertyType.Float:
                        property.floatValue = layer;
                        break;
                    case SerializedPropertyType.LayerMask:
                        property.intValue = layer;
                        break;
                }
            }
            EditorGUI.EndProperty();
        }
    }
}