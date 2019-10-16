using System;
using UnityEditor;
using UnityEditorHelper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AdditionalAttributes.Drawer
{
    [CustomPropertyDrawer(typeof(RangeMinMaxAttribute))]
    public class MinMaxRangeAttributeDrawer : RangeAttributeDrawer
    {
        private readonly string error = $"{nameof(MinMaxRangeAttributeDrawer)} only supports serialized properties of {nameof(SerializedPropertyType.Vector2Int)} ({typeof(Vector2Int)}) and {nameof(SerializedPropertyType.Vector2)} ({typeof(Vector2)})).";

        protected override void DrawProperty(Rect position, SerializedProperty serializedProperty)
        {
            void ShowSlider<T, U>(
                Func<SerializedProperty, T> getter, Action<SerializedProperty, T> setter,
                Action<Rect, SerializedProperty, U, U> field, Action<Rect, SerializedProperty, U, U, GUIContent> field2,
                Func<U, U, T> random, Func<T, U, T> stepper
            )
            {
                ShowField<T, U>(
                    position, serializedProperty, attribute as RangeMinMaxAttribute,
                    getter, setter, field, field2, random, stepper
                );
            }

            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Vector2:
                    void DrawFieldFloat(Rect rect, SerializedProperty property, float lower, float upper, GUIContent guiContent)
                    {
                        const int FIELDS_SPACE = 2;
                        Vector2 v = property.vector2Value;
                        float min = v.x;
                        float max = v.y;
                        HorizontalRectBuilder rectBuilder = new HorizontalRectBuilder(rect);

                        // Make label. We must do this because we have a min field at the left
                        // But only do this is we really need, since we add fake whitespace to have the proper slider offset in the RangeAttributeDrawer
                        if (!string.IsNullOrWhiteSpace(guiContent.text))
                            EditorGUI.LabelField(position, guiContent);

                        // Add the space used by the label or foldout
                        rectBuilder.AddSpace(EditorGUIUtility.labelWidth);

                        float blockWidth = rectBuilder.RemainingWidth / 5;

                        // Min field
                        min = EditorGUI.FloatField(rectBuilder.GetRect(blockWidth), min);
                        rectBuilder.AddSpace(FIELDS_SPACE);

                        // Slider
                        EditorGUI.MinMaxSlider(rectBuilder.GetRect(blockWidth * 3), new GUIContent("", guiContent.tooltip), ref min, ref max, lower, upper);

                        // Max field
                        rectBuilder.AddSpace(FIELDS_SPACE);
                        max = EditorGUI.FloatField(rectBuilder.GetRect(blockWidth), max);

                        // Only save if there was a change
                        if (min != v.x || max != v.y)
                            property.vector2Value = new Vector2(min, max);
                    }
                    ShowSlider<Vector2, float>(
                        e => e.vector2Value, (e, v) => e.vector2Value = v,
                        (rect, property, lower, upper) => DrawFieldFloat(rect, property, lower, upper, property.GetGUIContent()),
                        DrawFieldFloat,
                        (lower, upper) =>
                        {
                            float Rnd() => Random.Range(lower, upper);
                            float a = Rnd(), b = Rnd();
                            return new Vector2(Mathf.Min(a, b), Mathf.Max(a, b));
                        },
                        (value, step) =>
                        {
                            float Stp(float v) => (float)Math.Round(v / step, MidpointRounding.AwayFromZero) * step;
                            return new Vector2(Stp(value.x), Stp(value.y));
                        }
                    );
                    break;
                case SerializedPropertyType.Integer:
                    void DrawFieldInt(Rect rect, SerializedProperty property, int lower, int upper, GUIContent guiContent)
                    {
                        Vector2Int v = property.vector2IntValue;
                        float min = v.x;
                        float max = v.y;
                        EditorGUI.MinMaxSlider(rect, guiContent, ref min, ref max, lower, upper);
                        if (min != v.x || max != v.y)
                            property.vector2IntValue = new Vector2Int((int)min, (int)max);
                    }
                    ShowSlider<Vector2Int, int>(
                        e => e.vector2IntValue, (e, v) => e.vector2IntValue = v,
                        (rect, property, lower, upper) => DrawFieldInt(rect, property, lower, upper, property.GetGUIContent()),
                        DrawFieldInt,
                        (lower, upper) =>
                        {
                            int Rnd() => Random.Range(lower, upper);
                            int a = Rnd(), b = Rnd();
                            return new Vector2Int(Mathf.Min(a, b), Mathf.Max(a, b));
                        },
                        (value, step) =>
                        {
                            int Stp(int v) => v / step * step;
                            return new Vector2Int(Stp(value.x), Stp(value.y));
                        }
                    );
                    break;
                default:
                    string tooltip = error + $" Property {serializedProperty.name} is {serializedProperty.propertyType}.";
                    EditorGUI.HelpBox(position, tooltip, MessageType.Error);
                    Debug.LogException(new ArgumentException(tooltip));
                    break;
            }
        }
    }
}