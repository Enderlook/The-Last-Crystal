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
        EditorExtensions.Header("Floating Text Override Configuration");

        this.ToggleableField(nameof(FloatingTextController.scaleMultiplier), nameof(FloatingTextController.overrideScaleMultiplier));
        this.ToggleableField(nameof(FloatingTextController.textColor), nameof(FloatingTextController.overrideTextColor));
        this.ToggleableField(nameof(FloatingTextController.timeBeforeDestroy), nameof(FloatingTextController.overrideTimeBeforeDestroy));
        this.ToggleableField(nameof(FloatingTextController.randomOffset), nameof(FloatingTextController.overrideRandomOffset));
        this.ToggleableField(nameof(FloatingTextController.digitPrecision), nameof(FloatingTextController.overrideDigitPrecision));
        this.ToggleableField(nameof(FloatingTextController.typeOfRounding), nameof(FloatingTextController.overrideTypeOfRounding));

    }
}
