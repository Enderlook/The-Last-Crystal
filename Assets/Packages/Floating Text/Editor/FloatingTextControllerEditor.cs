using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FloatingTextController))]
public class FloatingTextControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FloatingTextController floatingTextController = (FloatingTextController)target;

        EditorGUILayout.Space();

        EditorExtensions.Header("Floating Text Override Configuration");

        // Draw fields
        string name = "Scale Multiplier";
        this.ToggleableField(
            ref floatingTextController.scaleMultiplier,
            () => EditorGUILayout.FloatField(
                new GUIContent(name, "Multiply the scale of the canvas by this value."),
                floatingTextController.scaleMultiplier),
            ref floatingTextController.overrideScaleMultiplier, $"Override {name}", $"Change of {name}"
        );
        name = "Text Color";
        this.ToggleableField(
            ref floatingTextController.textColor,
            () => EditorGUILayout.ColorField(
                new GUIContent("Text Color", "Text Color."),
                floatingTextController.textColor),
            ref floatingTextController.overrideTextColor, $"Override {name}", $"Change of {name}"
        );
        name = "Time Before Destroy";
        this.ToggleableField(
            ref floatingTextController.timeBeforeDestroy,
            () => EditorGUILayout.FloatField(
                new GUIContent("Time Before Destroy", "Time before self destroy in seconds. If 0, duration of the animation will be used."),
                floatingTextController.timeBeforeDestroy),
            ref floatingTextController.overrideTimeBeforeDestroy, $"Override {name}", $"Change of {name}"
        );
        name = "Random Offset";
        this.ToggleableField(
            ref floatingTextController.randomOffset,
            () => EditorGUILayout.Vector2Field(
                new GUIContent("Random Offset", "Random spawn offset."),
                floatingTextController.randomOffset),
            ref floatingTextController.overrideRandomOffset, $"Override {name}", $"Change of {name}"
        );
        name = "Digit Precision";
        this.ToggleableField(
            ref floatingTextController.digitPrecision,
            () => EditorGUILayout.IntField(
                new GUIContent("Digit Precision", "Digit precision (decimals) for numbers. Whenever a float is given to show, the number is rounded by a certain amount of digits."),
                floatingTextController.digitPrecision),
            ref floatingTextController.overrideDigitPrecision, $"Override {name}", $"Change of {name}"
        );
        name = "Type of Rounding";
        this.ToggleableField(
            ref floatingTextController.typeOfRounding,
            () => (FloatingText.TYPE_OF_ROUNDING)EditorGUILayout.EnumPopup(
                new GUIContent("Type of Rounding", "Determines how decimal digits are rounded."),
                floatingTextController.typeOfRounding),
            ref floatingTextController.overrideTypeOfRounding, $"Override {name}", $"Change of {name}"
        );
    }
}
