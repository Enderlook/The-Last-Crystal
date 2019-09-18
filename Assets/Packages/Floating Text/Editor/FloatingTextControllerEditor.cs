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

        serializedObject.ToggleableField(nameof(FloatingTextController.scaleMultiplier));
        serializedObject.ToggleableField(nameof(FloatingTextController.textColor));
        serializedObject.ToggleableField(nameof(FloatingTextController.timeBeforeDestroy));
        serializedObject.ToggleableField(nameof(FloatingTextController.randomOffset));
        serializedObject.ToggleableField(nameof(FloatingTextController.digitPrecision));
        serializedObject.ToggleableField(nameof(FloatingTextController.typeOfRounding));
    }
}
