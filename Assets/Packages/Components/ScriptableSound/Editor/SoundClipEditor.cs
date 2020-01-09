using UnityEditor;

using UnityEditorHelper;

namespace ScriptableSound
{
    [CustomEditor(typeof(SoundClip))]
    public class SoundClipEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawScriptField();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("audioClip"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("modifiers"), true);

            SoundPlayerEditor.DrawAmountField(serializedObject.FindProperty("playsAmount"));
        }
    }
}