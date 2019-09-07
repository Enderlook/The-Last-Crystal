using UnityEditor;

[CustomEditor(typeof(FloatingTextController))]
public class FloatingTextControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        this.PropertyFieldAutoSave(nameof(FloatingTextController.spawningPoints), true);
        this.PropertyFieldAutoSave(nameof(FloatingTextController.maximumAmountFloatingText));

        EditorGUILayout.Space();
        GUIHelper.Header("Floating Text Override Configuration");

        this.ToggleableField(nameof(FloatingTextController.scaleMultiplier));
        this.ToggleableField(nameof(FloatingTextController.textColor));
        this.ToggleableField(nameof(FloatingTextController.timeBeforeDestroy));
        this.ToggleableField(nameof(FloatingTextController.randomOffset));
        this.ToggleableField(nameof(FloatingTextController.digitPrecision));
        this.ToggleableField(nameof(FloatingTextController.typeOfRounding));
    }
}
