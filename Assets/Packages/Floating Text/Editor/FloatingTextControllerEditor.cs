using UnityEditor;

[CustomEditor(typeof(FloatingTextController))]
public class FloatingTextControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.DrawScriptField();

        serializedObject.Update();

        serializedObject.PropertyFieldAutoSave(nameof(FloatingTextController.spawningPoints), true);
        serializedObject.PropertyFieldAutoSave(nameof(FloatingTextController.maximumAmountFloatingText));

        serializedObject.PropertyFieldAutoSave(nameof(FloatingTextController.floatingTextPrefab));
        serializedObject.PropertyFieldAutoSave(nameof(FloatingTextController.floatingTextParent));

        EditorGUILayout.Space();
        GUIHelper.Header("Floating Text Override Configuration");

        serializedObject.ShowToggleableFields(true);
    }
}
