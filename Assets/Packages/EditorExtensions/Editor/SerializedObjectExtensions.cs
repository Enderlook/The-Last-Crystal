using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class SerializedObjectExtensions
{
    /// <summary>
    /// Create a Property Field and save it changes.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty"><see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="includeChildren"/>If <see langword="true"/> the property including children is drawn.</param>
    /// <see url="https://docs.unity3d.com/ScriptReference/EditorGUILayout.PropertyField.html"/>
    public static void PropertyFieldAutoSave(this SerializedObject source, SerializedProperty serializedProperty, bool includeChildren = false)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedProperty, includeChildren);
        if (EditorGUI.EndChangeCheck())
        {
            source.ApplyModifiedProperties();
        }
    }

    /// <summary>
    /// Create a Property Field and save it changes.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty">Name of the <see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="includeChildren"/>If <see langword="true"/> the property including children is drawn.</param>
    public static void PropertyFieldAutoSave(this SerializedObject source, string serializedProperty, bool includeChildren = false) => source.PropertyFieldAutoSave(source.FindProperty(serializedProperty), includeChildren);

    /// <summary>
    /// Store the change done in <paramref name="func"/> inside <paramref name="field"/>.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="func"/>.</typeparam>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="func">Function to execute in order to get changes.</param>
    /// <param name="field">Variable to store the changes made by <paramref name="func"/>.</param>
    /// <param name="reason">Reason to save.</param>
    public static void ChangeCheck<T>(this SerializedObject source, Func<T> func, ref T field, string reason)
    {
        // https://forum.unity.com/threads/custom-editor-losing-settings-on-play.130889/
        // https://docs.unity3d.com/ScriptReference/Undo.RecordObject.html
        EditorGUI.BeginChangeCheck();
        T value = func();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(source.targetObject, reason);
            field = value;
        }
    }

    /// <summary>
    /// Draw an idented field if <paramref name="confirm"/> is <see langword="true"/> and save an undo for it changes.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="toShowField"/>.</typeparam>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="field">Field to store the changes made by the function.</param>
    /// <param name="toShowField">Function which creates the field on Unity Inspector.</param>
    /// <param name="confirm">Whenever it should be drawed or not.</param>
    /// <param name="reasonForUndo">Reason used when save undo operation.</param>
    public static void DrawFieldIfConfirmed<T>(this SerializedObject source, ref T field, Func<T> toShowField, bool confirm, string reasonForUndo)
    {
        if (confirm)
        {
            EditorGUI.indentLevel++;
            source.ChangeCheck(toShowField, ref field, reasonForUndo);
            EditorGUI.indentLevel--;
        }
    }

    /// <summary>
    /// Draw an idented property field if <paramref name="confirm"/> is <see langword="true"/> and save an undo for it changes.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty"><see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="includeChildren"/>If <see langword="true"/> the property including children is drawn.</param>
    /// <param name="confirm">Whenever it should be drawed or not.</param>
    public static void DrawFieldIfConfirmed(this SerializedObject source, SerializedProperty serializedProperty, bool includeChildren, bool confirm)
    {
        DrawIdentedIfConfirmed(() => source.PropertyFieldAutoSave(serializedProperty, includeChildren), confirm);
    }

    /// <summary>
    /// Draw an idented property field if <paramref name="confirm"/> is <see langword="true"/> and save an undo for it changes.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty">Name of the <see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="includeChildren"/>If <see langword="true"/> the property including children is drawn.</param>
    /// <param name="confirm">Whenever it should be drawed or not.</param>
    public static void DrawFieldIfConfirmed(this SerializedObject source, string serializedProperty, bool includeChildren, bool confirm)
    {
        DrawIdentedIfConfirmed(() => source.PropertyFieldAutoSave(serializedProperty, includeChildren), confirm);
    }

    /// <summary>
    /// Do something idented in the Unity Inspecto if <paramref name="confirm"/> is <see langword="true"/>.
    /// </summary>
    /// <param name="action"><see cref="Action"/> to be performed idented.</param>
    /// <param name="confirm">Whenever it should be drawed or not.</param>
    private static void DrawIdentedIfConfirmed(Action action, bool confirm)
    {
        if (confirm)
        {
            EditorGUI.indentLevel++;
            action();
            EditorGUI.indentLevel--;
        }
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="func"/>.</typeparam>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="field">Field to store the changes made by the function.</param>
    /// <param name="toShowField">Function which creates the field on Unity Inspector.</param>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="confirmationLabel">Text of label.</param>
    /// <param name="confirmationTooltip">Text of tooltip.</param>
    /// <param name="reasonForUndo">Reason used when save undo operation.<br>
    /// If <see langword="null"/>, <paramref name="confirmationLabel"/> will be used instead.<br>
    /// To this it's appended " Checkbox." or " Change value." depending which action it was done.</param>
    public static void ToggleableField<T>(this SerializedObject source, ref T field, Func<T> toShowField, ref bool confirmationVariable, string confirmationLabel, string confirmationTooltip = "", string reasonForUndo = null)
    {
        source.ToggleableField(ref field, toShowField, ref confirmationVariable, new GUIContent(confirmationLabel, confirmationTooltip), reasonForUndo ?? confirmationLabel);
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="func"/>.</typeparam>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="field">Field to store the changes made by the function.</param>
    /// <param name="toShowfield">Function which creates the field on Unity Inspector.</param>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="confirmationContent">Text of label.</param>
    /// <param name="reasonForUndo">Reason used when save undo operation.</br>
    /// If <see langword="null"/>, <c><paramref name="confirmationContent"/>.text</c> will be used instead.<br>
    /// To this it's appended " Checkbox." or " Change value." depending which action it was done.</param>
    public static void ToggleableField<T>(this SerializedObject source, ref T field, Func<T> toShowfield, ref bool confirmationVariable, GUIContent confirmationContent, string reasonForUndo)
    {
        // https://answers.unity.com/questions/192895/hideshow-properties-dynamically-in-inspector.html
        // https://www.reddit.com/r/Unity3D/comments/45bjwc/tooltip_on_custom_inspectorproperties/

        reasonForUndo = reasonForUndo ?? confirmationContent.text;

        bool toggleValue = confirmationVariable;
        source.ChangeCheck(() => GUILayout.Toggle(toggleValue, confirmationContent), ref confirmationVariable, $"{reasonForUndo}. Checkbox.");

        source.DrawFieldIfConfirmed(ref field, toShowfield, confirmationVariable, reasonForUndo);
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty"><see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="confirmationContent">Text of label.</param>
    /// <param name="reasonForUndo">Reason used when save undo operation. To this it's appended " Checkbox.".</br>
    /// If <see langword="null"/>, <c><paramref name="confirmationContent"/>.text</c> will be used instead.</param>
    /// <param name="includeChildren"/>If <see langword="true"/> the property including children is drawn.</param>
    public static void ToggleableField(this SerializedObject source, SerializedProperty serializedProperty, ref bool confirmationVariable, GUIContent confirmationContent, string reasonForUndo, bool includeChildren = false)
    {
        reasonForUndo = reasonForUndo ?? confirmationContent.text;

        bool toggleValue = confirmationVariable;
        source.ChangeCheck(() => GUILayout.Toggle(toggleValue, confirmationContent), ref confirmationVariable, $"{reasonForUndo}. Checkbox.");
        source.DrawFieldIfConfirmed(serializedProperty, includeChildren, confirmationVariable);
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty">Name of the <see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="confirmationContent">Text of label.</param>
    /// <param name="reasonForUndo">Reason used when save undo operation. To this it's appended " Checkbox.".</br>
    /// If <see langword="null"/>, <c><paramref name="confirmationContent"/>.text</c> will be used instead.</param>
    /// <param name="includeChildren"/>If <see langword="true"/> the property including children is drawn.</param>
    public static void ToggleableField(this SerializedObject source, string serializedProperty, ref bool confirmationVariable, GUIContent confirmationContent, string reasonForUndo, bool includeChildren = false)
    {
        reasonForUndo = reasonForUndo ?? confirmationContent.text;

        bool toggleValue = confirmationVariable;
        source.ChangeCheck(() => GUILayout.Toggle(toggleValue, confirmationContent), ref confirmationVariable, $"{reasonForUndo}. Checkbox.");
        source.DrawFieldIfConfirmed(serializedProperty, includeChildren, confirmationVariable);
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty"><see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="booleanSerializedProperty"><see cref="SerializedProperty"/> to show in the inspector as confirmation checkbox./param>
    /// <param name="includeChildren"/>If <see langword="true"/> the <paramref name="serializedProperty"/> including children is drawn.</param>
    public static void ToggleableField(this SerializedObject source, SerializedProperty serializedProperty, SerializedProperty booleanSerializedProperty, bool includeChildren = false)
    {
        source.PropertyFieldAutoSave(booleanSerializedProperty);
        source.DrawFieldIfConfirmed(serializedProperty, includeChildren, booleanSerializedProperty.boolValue);
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty">Name of the <see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="booleanSerializedProperty">Name of the <see cref="SerializedProperty"/> to show in the inspector as confirmation checkbox./param>
    /// <param name="includeChildren"/>If <see langword="true"/> the <paramref name="serializedProperty"/> including children is drawn.</param>
    public static void ToggleableField(this SerializedObject source, string serializedProperty, string booleanSerializedProperty, bool includeChildren = false)
    {
        source.ToggleableField(source.FindProperty(serializedProperty), source.FindProperty(booleanSerializedProperty), includeChildren);
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty"Name of <see cref="SerializedProperty"/> to show in the inspector.<br>
    /// This field must have a <see cref="HasConfirmationFieldAttribute"/>.</param>
    /// <param name="includeChildren"/>If <see langword="true"/> the <paramref name="serializedProperty"/> including children is drawn.</param>
    public static void ToggleableField(this SerializedObject source, string serializedProperty, bool includeChildren = false)
    {
        Type type = source.targetObject.GetType();
        HasConfirmationFieldAttribute attribute = type.GetField(serializedProperty).GetCustomAttribute(typeof(HasConfirmationFieldAttribute)) as HasConfirmationFieldAttribute;
        if (attribute == null)
            throw new Exception($"The {type}.{serializedProperty} field must have the attribute {nameof(HasConfirmationFieldAttribute)}.");
        else
        {
            source.ToggleableField(serializedProperty, attribute.ConfirmFieldName, includeChildren);
        }
    }
}
