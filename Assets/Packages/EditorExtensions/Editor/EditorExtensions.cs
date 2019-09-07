using System;
using UnityEditor;
using UnityEngine;

public static class EditorExtensions
{
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
    public static void ToggleableField<T>(this Editor source, ref T field, Func<T> toShowField, ref bool confirmationVariable, string confirmationLabel, string confirmationTooltip = "", string reasonForUndo = null)
    {
        source.serializedObject.ToggleableField(ref field, toShowField, ref confirmationVariable, confirmationLabel, confirmationTooltip, reasonForUndo);
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
    public static void ToggleableField<T>(this Editor source, ref T field, Func<T> toShowfield, ref bool confirmationVariable, GUIContent confirmationContent, string reasonForUndo)
    {
        source.serializedObject.ToggleableField(ref field, toShowfield, ref confirmationVariable, confirmationContent, reasonForUndo);
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
    public static void ToggleableField(this Editor source, SerializedProperty serializedProperty, ref bool confirmationVariable, GUIContent confirmationContent, string reasonForUndo, bool includeChildren = false)
    {
        source.serializedObject.ToggleableField(serializedProperty, ref confirmationVariable, confirmationContent, reasonForUndo);
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
    public static void ToggleableField(this Editor source, string serializedProperty, ref bool confirmationVariable, GUIContent confirmationContent, string reasonForUndo, bool includeChildren = false)
    {
        source.ToggleableField(serializedProperty, ref confirmationVariable, confirmationContent, reasonForUndo, includeChildren);
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty"><see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="booleanSerializedProperty"><see cref="SerializedProperty"/> to show in the inspector as confirmation checkbox./param>
    /// <param name="includeChildren"/>If <see langword="true"/> the <paramref name="serializedProperty"/> including children is drawn.</param>
    public static void ToggleableField(this Editor source, SerializedProperty serializedProperty, SerializedProperty booleanSerializedProperty, bool includeChildren = false)
    {
        source.ToggleableField(serializedProperty, serializedProperty, includeChildren);
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty">Name of the <see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="booleanSerializedProperty">Name of the <see cref="SerializedProperty"/> to show in the inspector as confirmation checkbox./param>
    /// <param name="includeChildren"/>If <see langword="true"/> the <paramref name="serializedProperty"/> including children is drawn.</param>
    public static void ToggleableField(this Editor source, string serializedProperty, string booleanSerializedProperty, bool includeChildren = false)
    {
        source.serializedObject.ToggleableField(serializedProperty, booleanSerializedProperty, includeChildren);
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty"Name of <see cref="SerializedProperty"/> to show in the inspector.<br>
    /// This field must have a <see cref="HasConfirmationFieldAttribute"/>.</param>
    /// <param name="includeChildren"/>If <see langword="true"/> the <paramref name="serializedProperty"/> including children is drawn.</param>
    public static void ToggleableField(this Editor source, string serializedProperty, bool includeChildren = false)
    {
        source.serializedObject.ToggleableField(serializedProperty, includeChildren);
    }

    /// <summary>
    /// Store the change done in <paramref name="func"/> inside <paramref name="field"/>.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="func"/>.</typeparam>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="func">Function to execute in order to get changes.</param>
    /// <param name="field">Variable to store the changes made by <paramref name="func"/>.</param>
    /// <param name="reason">Reason to save.</param>
    public static void ChangeCheck<T>(this Editor source, Func<T> func, ref T field, string reason) => source.serializedObject.ChangeCheck(func, ref field, reason);

    /// <summary>
    /// Create a Property Field and save it changes.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty"><see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="includeChildren"/>If <see langword="true"/> the property including children is drawn.</param>
    /// <see url="https://docs.unity3d.com/ScriptReference/EditorGUILayout.PropertyField.html"/>
    public static void PropertyFieldAutoSave(this Editor source, SerializedProperty serializedProperty, bool includeChildren = false) => source.serializedObject.PropertyFieldAutoSave(serializedProperty, includeChildren);

    /// <summary>
    /// Create a Property Field and save it changes.
    /// </summary>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="serializedProperty">Name of the <see cref="SerializedProperty"/> to show in the inspector./param>
    /// <param name="includeChildren"/>If <see langword="true"/> the property including children is drawn.</param>
    public static void PropertyFieldAutoSave(this Editor source, string serializedProperty, bool includeChildren = false) => source.serializedObject.PropertyFieldAutoSave(serializedProperty, includeChildren);
}
