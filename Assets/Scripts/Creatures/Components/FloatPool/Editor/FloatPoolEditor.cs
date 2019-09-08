using FloatPool;
using UnityEditor;

[CustomEditor(typeof(Pool))]
public class FloatPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        serializedObject.PropertyFieldAutoSave(nameof(Pool.basePool), true);

        EditorGUILayout.Space();
        GUIHelper.Header("Additional Configuration");

        serializedObject.ToggleableField(nameof(Pool.callback), true);
        serializedObject.ToggleableField(nameof(Pool.bar), true);
        serializedObject.ToggleableField(nameof(Pool.recharger), true);
        serializedObject.ToggleableField(nameof(Pool.changeCallback), true);
        serializedObject.ToggleableField(nameof(Pool.decreaseReduction), true);
    }
}
