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
}
