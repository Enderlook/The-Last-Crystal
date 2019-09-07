using System;
using UnityEditor;
using UnityEngine;

public class EditorHelper
{
    private readonly UnityEngine.Object target;

    public EditorHelper(UnityEngine.Object target) => this.target = target;

    /// <summary>
    /// Draw a field and save an undo for it changes.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="toShowField"/>.</typeparam>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="field">Field to store the changes made by the function.</param>
    /// <param name="toShowField">Function which creates the field on Unity Inspector.</param>
    /// <param name="reasonForUndo">Reason used when save undo operation.</param>
    private void DrawField<T>(ref bool confirmationVariable, Func<T> toShowField, ref T field, string reasonForUndo)
    {
        if (confirmationVariable)
        {
            EditorGUI.indentLevel++;
            ChangeCheck(toShowField, ref field, reasonForUndo);
            EditorGUI.indentLevel--;
        }
    }
    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="toShowField"/>.</typeparam>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="confirmationLabel">Text of label.</param>
    /// <param name="field">Field to store the changes made by the function.</param>
    /// <param name="toShowField">Function which creates the field on Unity Inspector.</param>
    /// <param name="reasonForUndo">Reason used when save undo operation.</br>
    /// To this it's appended " Checkbox." or " Change value." depending which action it was done.</param>
    /// <seealso cref="CreateToggleButton(bool, string)"/>
    public void ToggleableField<T>(ref bool confirmationVariable, string confirmationLabel, ref T field, Func<T> toShowField, string reasonForUndo)
    {
        // https://answers.unity.com/questions/192895/hideshow-properties-dynamically-in-inspector.html
        // https://www.reddit.com/r/Unity3D/comments/45bjwc/tooltip_on_custom_inspectorproperties/

        bool toggleValue = confirmationVariable;
        ChangeCheck(() => CreateToggleButton(toggleValue, confirmationLabel), ref confirmationVariable, $"{reasonForUndo}. Checkbox.");
        DrawField(ref confirmationVariable, toShowField, ref field, $"{reasonForUndo} Change value.");
    }
    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="func"/>.</typeparam>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="confirmationLabel">Text of label.</param>
    /// <param name="confirmationTooltip">Text of tooltip.</param>
    /// <param name="field">Field to store the changes made by the function.</param>
    /// <param name="toShowField">Function which creates the field on Unity Inspector.</param>
    /// <param name="reasonForUndo">Reason used when save undo operation.</br>
    /// To this it's appended " Checkbox." or " Change value." depending which action it was done.</param>
    /// <seealso cref="CreateToogleButton(bool, string, string)"/>
    public void ToggleableField<T>(ref bool confirmationVariable, string confirmationLabel, string confirmationTooltip, ref T field, Func<T> toShowField, string reasonForUndo)
    {
        // https://answers.unity.com/questions/192895/hideshow-properties-dynamically-in-inspector.html
        // https://www.reddit.com/r/Unity3D/comments/45bjwc/tooltip_on_custom_inspectorproperties/

        bool toggleValue = confirmationVariable;
        ChangeCheck(() => CreateToggleButton(toggleValue, confirmationLabel, confirmationTooltip), ref confirmationVariable, $"{reasonForUndo}. Checkbox.");
        DrawField(ref confirmationVariable, toShowField, ref field, $"{reasonForUndo} Change value.");
    }

    /// <summary>
    /// Generate a toggleable button to hide or show a certain field, which is also created by this method.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="func"/>.</typeparam>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="confirmationContent">Text of label.</param>
    /// <param name="field">Field to store the changes made by the function.</param>
    /// <param name="toShowfield">Function which creates the field on Unity Inspector.</param>
    /// <param name="reasonForUndo">Reason used when save undo operation.</br>
    /// To this it's appended " Checkbox." or " Change value." depending which action it was done.</param>
    /// <seealso cref="CreateToggleButton(bool, GUIContent)"/>
    public void ToggleableField<T>(ref bool confirmationVariable, GUIContent confirmationContent, ref T field, Func<T> toShowfield, string reasonForUndo)
    {
        bool toggleValue = confirmationVariable;
        ChangeCheck(() => CreateToggleButton(toggleValue, confirmationContent), ref confirmationVariable, $"{reasonForUndo}. Checkbox.");

        if (confirmationVariable)
        {
            EditorGUI.indentLevel++;
            ChangeCheck(toShowfield, ref field, $"{reasonForUndo} Change value.");
            EditorGUI.indentLevel--;
        }
    }

    /// <summary>
    /// Create toggleable button.
    /// </summary>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="toggleContent">Content of toggle button</param>
    /// <returns>Whenever the toggleable button is checked or not.</returns>
    /// <seealso cref="CreateToggleButton(bool, string)"/>
    /// <seealso cref="CreateToogleButton(bool, string, string)"/>
    public bool CreateToggleButton(bool confirmationVariable, GUIContent toggleContent) => GUILayout.Toggle(confirmationVariable, toggleContent);
    /// <summary>
    /// Create toggleable button.
    /// </summary>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="toggleLabel">Label toggle button.</param>
    /// <returns>Whenever the toggleable button is checked or not.</returns>
    /// <seealso cref="CreateToggleButton(bool, GUIContent)"/>
    /// <seealso cref="CreateToogleButton(bool, string, string)"/>
    public bool CreateToggleButton(bool confirmationVariable, string toggleLabel) => GUILayout.Toggle(confirmationVariable, toggleLabel);
    /// <summary>
    /// Create toggleable button.
    /// </summary>
    /// <param name="confirmationVariable">Boolean variable to store the toggle check.</param>
    /// <param name="toggleLabel">Label toggle button.</param>
    /// <param name="toggleTooltip">Tooltip of toggle button.</param>
    /// <returns>Whenever the toggleable button is checked or not.</returns>
    /// <seealso cref="CreateToggleButton(bool, GUIContent)"/>
    /// <seealso cref="CreateToggleButton(bool, string)"/>
    public bool CreateToggleButton(bool confirmationVariable, string toggleLabel, string toggleTooltip) => GUILayout.Toggle(confirmationVariable, toggleLabel, toggleTooltip);

    /// <summary>
    /// Store the change done in <paramref name="func"/> inside <paramref name="field"/>.
    /// </summary>
    /// <typeparam name="T">Type of return value by <paramref name="func"/>.</typeparam>
    /// <param name="func">Function to execute in order to get changes.</param>
    /// <param name="field">Variable to store the changes made by <paramref name="func"/>.</param>
    /// <param name="reason">Reason to save.</param>
    public void ChangeCheck<T>(Func<T> func, ref T field, string reason)
    {
        // https://docs.unity3d.com/ScriptReference/Undo.RecordObject.html
        EditorGUI.BeginChangeCheck();
        T value = func();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, reason);
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
