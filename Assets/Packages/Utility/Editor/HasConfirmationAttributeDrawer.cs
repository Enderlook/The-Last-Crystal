using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HasConfirmationFieldAttribute))]
public class HasConfirmationAttributeDrawer : PropertyDrawer
{
    private bool confirm;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        HasConfirmationFieldAttribute hasConfirmationFieldAttribute = (HasConfirmationFieldAttribute)attribute;

        Rect fieldRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight);

        SerializedProperty confirmation = property.serializedObject.FindProperty(hasConfirmationFieldAttribute.ConfirmFieldName);

        EditorGUI.PropertyField(position, confirmation);

        confirm = confirmation.boolValue;
        if (confirm)
        {
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(fieldRect, property, label, true);
            EditorGUI.indentLevel--;
        }

        property.serializedObject.ApplyModifiedProperties();

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedObject serializedObject = property.serializedObject;

        FieldInfo fieldInfo = serializedObject.targetObject.GetType().GetField(property.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        HasConfirmationFieldAttribute hasConfirmationFieldAttribute = (HasConfirmationFieldAttribute)fieldInfo.GetCustomAttribute(typeof(HasConfirmationFieldAttribute), true);

        confirm = serializedObject.FindProperty(hasConfirmationFieldAttribute.ConfirmFieldName).boolValue;

        return confirm ? EditorGUI.GetPropertyHeight(property) + EditorGUIUtility.singleLineHeight : EditorGUIUtility.singleLineHeight;
    }
}
