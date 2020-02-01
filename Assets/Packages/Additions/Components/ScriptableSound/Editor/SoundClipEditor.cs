using Additions.Utils.UnityEditor;

using System;

using UnityEditor;

using UnityEngine;

using UnityObject = UnityEngine.Object;
namespace Additions.Components.ScriptableSound
{
    [CustomEditor(typeof(SoundClip))]
    internal class SoundClipEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawScriptField();

            EditorGUI.BeginChangeCheck();

            SerializedProperty audioClipProperty = serializedObject.FindProperty("audioClip");
            EditorGUILayout.PropertyField(audioClipProperty, true);
            UnityObject audioClip = audioClipProperty.objectReferenceValue;
            if (audioClip != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField(new GUIContent("Duration", "Duration of the clip."), TimeSpan.FromSeconds(((AudioClip)audioClip).length).ToString(@"hh\:mm\:ss\:ff"));
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("modifiers"), true);
            SoundPlayerEditor.DrawAmountField(serializedObject.FindProperty("playsAmount"));

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}