using System;
using UnityEditor;

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
            ChangeCheck(source, toShowField, ref field, reasonForUndo);
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
        DrawIdentedIfConfirmed(() => PropertyFieldAutoSave(source, serializedProperty, includeChildren), confirm);
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
        DrawIdentedIfConfirmed(() => PropertyFieldAutoSave(source, serializedProperty, includeChildren), confirm);
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
}
