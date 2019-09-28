using FloatPool;
using UnityEditor;

[CustomEditor(typeof(Pool))]
public class FloatPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.DrawScriptField();

        serializedObject.Update();

        serializedObject.PropertyFieldAutoSave(nameof(Pool.basePool), true);

        EditorGUILayout.Space();
        GUIHelper.Header("Additional Configuration");

        serializedObject.ToggleableFields(true);
    }
}
