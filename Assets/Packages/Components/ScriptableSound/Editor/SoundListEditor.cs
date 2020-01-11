using System.Linq;

using UnityEditor;

using UnityEditorHelper;

using UnityEngine;

namespace ScriptableSound
{
    [CustomEditor(typeof(SoundList))]
    public class SoundPlayerEditor : Editor
    {
        private static readonly GUIContent[] OPTIONS_GUI_CONTENTS = new GUIContent[]
            {
                new GUIContent("Don't Play", "Nothing will be played."),
                new GUIContent("Play Once", "Only one time will be played."),
                new GUIContent("Play Forever", "Play an infinite amount of times."),
                new GUIContent("Play Other", "Play a user defined amount of times.")
            };

        private static readonly int[] OPTIONS_VALUES = new int[] { 0, 1, -1, 2 };

        private static readonly int[] OPTIONS_VALUES_2 = new int[] { 0, 1, -1 };

        public override void OnInspectorGUI()
        {
            this.DrawScriptField();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("sounds"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playMode"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playListMode"), true);
            DrawAmountField(serializedObject.FindProperty("playsAmount"));

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        public static void DrawAmountField(SerializedProperty property)
        {
            EditorGUILayout.IntPopup(property, OPTIONS_GUI_CONTENTS, OPTIONS_VALUES);

            EditorGUI.indentLevel++;
            if (!OPTIONS_VALUES_2.Contains(property.intValue))
                EditorGUILayout.PropertyField(property, true);
            EditorGUI.indentLevel--;
        }
    }
}