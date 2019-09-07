using System;
using UnityEditor;
using UnityEngine;

public static class EditorExtensions
{
    /// <summary>
    /// Draw an idented field if <paramref name="confirm"/> is <see langword="true"/> and save an undo for it changes.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="toShowField"/>.</typeparam>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="field">Field to store the changes made by the function.</param>
    /// <param name="toShowField">Function which creates the field on Unity Inspector.</param>
    /// <param name="confirm">Whenever it should be drawed or not.</param>
    /// <param name="reasonForUndo">Reason used when save undo operation.</param>
    private static void DrawFieldIfConfirmed<T>(this Editor source, ref T field, Func<T> toShowField, bool confirm, string reasonForUndo)
    {
        if (confirm)
        {
            EditorGUI.indentLevel++;
            ChangeCheck(source, toShowField, ref field, reasonForUndo);
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

        DrawFieldIfConfirmed(source, ref field, toShowfield, confirmationVariable, reasonForUndo);
    }

    /// <summary>
    /// Store the change done in <paramref name="func"/> inside <paramref name="field"/>.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="func"/>.</typeparam>
    /// <param name="source">Instance where its executed this method.</param>
    /// <param name="func">Function to execute in order to get changes.</param>
    /// <param name="field">Variable to store the changes made by <paramref name="func"/>.</param>
    /// <param name="reason">Reason to save.</param>
    public static void ChangeCheck<T>(this Editor source, Func<T> func, ref T field, string reason)
    {
        // https://forum.unity.com/threads/custom-editor-losing-settings-on-play.130889/
        // https://docs.unity3d.com/ScriptReference/Undo.RecordObject.html
        EditorGUI.BeginChangeCheck();
        T value = func();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(source.target, reason);
            field = value;
        }
    }

    /// <summary>
    /// Add a header.
    /// </summary>
    /// <param name="text">Text of header</param>
    public static void Header(string text)
    {
        // https://www.reddit.com/r/Unity3D/comments/3b43pf/unity_editor_scripting_how_can_i_draw_a_header_in/
        EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
    }
}
