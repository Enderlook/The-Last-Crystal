using Additions.Extensions;

using System;

using UnityEditor;

using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Additions.Attributes
{
    [CustomPropertyDrawer(typeof(RestrictTypeAttribute))]
    public class RestrictTypeDrawer : PropertyDrawer
    {
        private float? height;
        private bool firstTime = true;

        private Rect GetBoxPosition(string message, Rect position)
        {
            height = GUI.skin.box.CalcHeight(new GUIContent(message), position.width);
            return new Rect(position.x, position.y, position.width, height.Value);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            void DrawErrorBox(string message)
            {
                Debug.LogError(message);
                EditorGUI.HelpBox(GetBoxPosition(message, position), message, MessageType.Error);
            }

            height = null;

            // Get the element type from arrays or lists
            if (!fieldInfo.FieldType.TryGetElementTypeOfArrayOrList(out Type fieldType))
                fieldType = fieldInfo.FieldType;

            // Check if that element type inherit from Unity object
            if (!typeof(UnityObject).IsAssignableFrom(fieldType))
            {
                DrawErrorBox($"Field {property.name} must inherit from {typeof(UnityObject)}, or be an array or list with that type as elements. Is {fieldInfo.FieldType}.");
                return;
            }

            // Check that restrictions are feasibles
            RestrictTypeAttribute restrictTypeAttribute = (RestrictTypeAttribute)attribute;
            foreach (Type type in restrictTypeAttribute.restriction)
            {
                if (type.IsClass)
                {
                    // Restriction type must inherit from Unity object
                    if (!typeof(UnityObject).IsAssignableFrom(type))
                    {
                        DrawErrorBox($"Field {property.name}'s attribute {nameof(RestrictTypeAttribute)} has a wrong {nameof(Type)} restriction. Class types must inherit from {typeof(UnityObject)}. One of its restrictions was {type}.");
                        return;
                    }
                    // Restriction type must be casteable to field type
                    else if (!type.IsCastableTo(fieldType))
                    {
                        DrawErrorBox($"Field {property.name}'s attribute {nameof(RestrictTypeAttribute)} has a wrong {nameof(Type)} restriction. Restrictions class types must be casteable to the field element type ({fieldType}). One of its restrictions was {type}.");
                        return;
                    }
                }
                // Structs and primitives are not allowed.
                else if (type.IsValueType || type.IsPrimitive || type.IsPointer)
                {
                    DrawErrorBox($"Field {property.name}'s attribute {nameof(RestrictTypeAttribute)} has a wrong {nameof(Type)} restriction. Restrictions can't be value types, primitives nor pointers. One of its restrictions was {type}.");
                    return;
                }
                // Check for everything else, just to be sure
                else if (!type.IsInterface)
                {
                    DrawErrorBox($"Field {property.name}'s attribute {nameof(RestrictTypeAttribute)} has a wrong {nameof(Type)} restriction. Restrictions can only be classes derived from {typeof(UnityObject)} or interfaces. One of its restrictions was {type}.");
                    return;
                }
            }

            UnityObject old = property.objectReferenceValue;
            EditorGUI.PropertyField(position, property, label);
            UnityObject result = property.objectReferenceValue;

            // We check for differences to avoid wasting perfomance
            if ((old != result || firstTime) && result != null)
            {
                firstTime = false;
                Type resultType = result.GetType();
                foreach (Type type in restrictTypeAttribute.restriction)
                {
                    if (!resultType.IsCastableTo(type))
                    {
                        Debug.LogError($"Field {property.name} require values than can be casted to {type}. {resultType} can't.");
                        property.objectReferenceValue = null;
                        return;
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => height.HasValue ? height.Value : EditorGUI.GetPropertyHeight(property, label, true);
    }
}