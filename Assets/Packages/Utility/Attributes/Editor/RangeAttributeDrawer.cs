using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CustomPropertyDrawer(typeof(RangeAttribute))]
public class RangeAttributeDrawer : PropertyDrawer
{
    private readonly string error = $"{nameof(RangeAttributeDrawer)} only supports {typeof(int)} and {typeof(float)}) values.";
    private readonly string errorLabel = $"Use {nameof(RangeAttributeDrawer)} with {typeof(int)} or {typeof(float)} fields.";

    private bool foldout;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RangeAttribute rangeAttribute = attribute as RangeAttribute;

        if (foldout)
            position.height -= EditorGUIUtility.singleLineHeight;

        bool ShowField<T>(T value, T min, T max, T step,
                       Func<T, T, T> randomMinMax, Func<Rect, GUIContent, T, T, T, T> slider1,
                       Action<Rect, SerializedProperty, T, T> slider2,
                       out T result,
                       Func<T, T, T, T, T> stepper = null)
        {
            result = default;
            if (stepper != null || rangeAttribute.showRandomButton)
            {
                if (rangeAttribute.showRandomButton)
                {
                    if ((foldout = EditorGUI.Foldout(position, foldout, label, true)) &&
                        GUI.Button(
                            new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height),
                            new GUIContent("Randomize", $"Produce a random value from {min} to {max}{(stepper == null ? "." : $" in steps of {step}.")}")
                        ))
                        randomMinMax(min, max);
                }
                else
                    EditorGUI.LabelField(position, label);
                value = slider1(position, label, value, min, max);

                if (stepper == null)
                    return true;
                result = stepper(value, min, max, step);
                return true;
            }
            slider2(position, property, min, max);
            return false;
        }

        EditorGUI.BeginProperty(position, label, property);
        switch (property.propertyType)
        {
            case SerializedPropertyType.Float:
                if (ShowField(
                    property.floatValue, rangeAttribute.min, rangeAttribute.max, rangeAttribute.step,
                    Random.Range, EditorGUI.Slider, EditorGUI.Slider,
                    out float resultFloat,
                    rangeAttribute.step > 0 // If there is step use special rounding
                    ? (Func<float, float, float, float, float>)((value, min, max, step) => Mathf.Clamp(
                        (float)Math.Round(value / step, MidpointRounding.AwayFromZero) * step - min,
                        0, max - min) + min) : null))
                    property.floatValue = resultFloat;
                break;
            case SerializedPropertyType.Integer:
                if (ShowField(
                    property.intValue, (int)rangeAttribute.min, (int)rangeAttribute.max, (int)rangeAttribute.step,
                    Random.Range, EditorGUI.IntSlider, EditorGUI.IntSlider,
                    out int resultInt,
                    rangeAttribute.step > 0 // If there is step use special rounding
                    ? (Func<int, int, int, int, int>)((value, min, max, step) => value /  step * (int)step)
                    : null))
                    property.intValue = resultInt;
                break;
            default:
                string tooltip = error + $" Property {property.name} is {property.propertyType}.";
                EditorGUI.LabelField(position, new GUIContent($"{property.name}: errorLabel", tooltip));
                Debug.LogException(new ArgumentException(tooltip));
                break;
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property) + (foldout ? EditorGUIUtility.singleLineHeight : 0);
}
