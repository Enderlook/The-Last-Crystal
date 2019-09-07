using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;

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
        ToggleableField(source, ref field, toShowField, ref confirmationVariable, new GUIContent(confirmationLabel, confirmationTooltip), reasonForUndo ?? confirmationLabel);
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
        // https://answers.unity.com/questions/192895/hideshow-properties-dynamically-in-inspector.html
        // https://www.reddit.com/r/Unity3D/comments/45bjwc/tooltip_on_custom_inspectorproperties/

        reasonForUndo = reasonForUndo ?? confirmationContent.text;

        bool toggleValue = confirmationVariable;
        ChangeCheck(source, () => GUILayout.Toggle(toggleValue, confirmationContent), ref confirmationVariable, $"{reasonForUndo}. Checkbox.");

        source.serializedObject.DrawFieldIfConfirmed(ref field, toShowfield, confirmationVariable, reasonForUndo);
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
        reasonForUndo = reasonForUndo ?? confirmationContent.text;

        bool toggleValue = confirmationVariable;
        ChangeCheck(source, () => GUILayout.Toggle(toggleValue, confirmationContent), ref confirmationVariable, $"{reasonForUndo}. Checkbox.");
        source.serializedObject.DrawFieldIfConfirmed(serializedProperty, includeChildren, confirmationVariable);
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
        reasonForUndo = reasonForUndo ?? confirmationContent.text;

        bool toggleValue = confirmationVariable;
        ChangeCheck(source, () => GUILayout.Toggle(toggleValue, confirmationContent), ref confirmationVariable, $"{reasonForUndo}. Checkbox.");
        source.serializedObject.DrawFieldIfConfirmed(serializedProperty, includeChildren, confirmationVariable);
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
        PropertyFieldAutoSave(source, booleanSerializedProperty);
        source.serializedObject.DrawFieldIfConfirmed(serializedProperty, includeChildren, booleanSerializedProperty.boolValue);
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
        ToggleableField(source, source.serializedObject.FindProperty(serializedProperty), source.serializedObject.FindProperty(booleanSerializedProperty), includeChildren);
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
        Type type = source.target.GetType();
        HasConfirmationFieldAttribute attribute = type.GetField(serializedProperty).GetCustomAttribute(typeof(HasConfirmationFieldAttribute)) as HasConfirmationFieldAttribute;
        if (attribute == null)
            throw new Exception($"The {type}.{serializedProperty} field must have the attribute {nameof(HasConfirmationFieldAttribute)}.");
        else
        {
            ToggleableField(source, serializedProperty, attribute.confirmFieldName, includeChildren);
        }
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
