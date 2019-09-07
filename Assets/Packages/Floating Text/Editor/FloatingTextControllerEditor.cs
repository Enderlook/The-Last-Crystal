using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FloatingTextController))]
public class FloatingTextControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorHelper editorHelper = new EditorHelper(target);

        FloatingTextController floatingTextController = (FloatingTextController)target;

        EditorGUILayout.Space();

        EditorHelper.Header("Floating Text Override Configuration");

        // Draw fields
        string name = "Scale Multiplier";
        editorHelper.ToggleableField(ref floatingTextController.overrideScaleMultiplier, $"Override {name}", ref floatingTextController.scaleMultiplier,
            () => EditorGUILayout.FloatField(
                new GUIContent(name, "Multiply the scale of the canvas by this value."),
                floatingTextController.scaleMultiplier),
            $"Change of {name}"
        );
        name = "Text Color";
        editorHelper.ToggleableField(ref floatingTextController.overrideTextColor, $"Override {name}", ref floatingTextController.textColor,
            () => EditorGUILayout.ColorField(
                new GUIContent("Text Color", "Text Color."),
                floatingTextController.textColor),
            $"Change of {name}"
        );
        name = "Time Before Destroy";
        editorHelper.ToggleableField(ref floatingTextController.overrideTimeBeforeDestroy, $"Override {name}", ref floatingTextController.timeBeforeDestroy,
            () => EditorGUILayout.FloatField(
                new GUIContent("Time Before Destroy", "Time before self destroy in seconds. If 0, duration of the animation will be used."),
                floatingTextController.timeBeforeDestroy),
            $"Change of {name}"
        );
        name = "Random Offset";
        editorHelper.ToggleableField(ref floatingTextController.overrideRandomOffset, $"Override {name}", ref floatingTextController.randomOffset,
            () => EditorGUILayout.Vector2Field(
                new GUIContent("Random Offset", "Random spawn offset."),
                floatingTextController.randomOffset),
             $"Change of {name}"
        );
        name = "Digit Precision";
        editorHelper.ToggleableField(ref floatingTextController.overrideDigitPrecision, $"Override {name}", ref floatingTextController.digitPrecision,
            () => EditorGUILayout.IntField(
                new GUIContent("Digit Precision", "Digit precision (decimals) for numbers. Whenever a float is given to show, the number is rounded by a certain amount of digits."),
                floatingTextController.digitPrecision),
            $"Change of {name}"
        );
        name = "Type of Rounding";
        editorHelper.ToggleableField(ref floatingTextController.overrideTypeOfRounding, $"Override { name}", ref floatingTextController.typeOfRounding,
            () => (FloatingText.TYPE_OF_ROUNDING)EditorGUILayout.EnumPopup(
                new GUIContent("Type of Rounding", "Determines how decimal digits are rounded."),
                floatingTextController.typeOfRounding),
            $"Change of {name}"
        );
    }
}
