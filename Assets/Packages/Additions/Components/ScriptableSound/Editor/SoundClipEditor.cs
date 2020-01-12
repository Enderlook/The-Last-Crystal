using Additions.Utils.UnityEditor;

using UnityEditor;

namespace Additions.Components.ScriptableSound
{
    [CustomEditor(typeof(SoundClip))]
    internal class SoundClipEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawScriptField();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("audioClip"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("modifiers"), true);
            SoundPlayerEditor.DrawAmountField(serializedObject.FindProperty("playsAmount"));

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}