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
        private const int FIELDS_SPACE = 2;

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
                    Action<Rect, SerializedProperty, float, float, GUIContent> floatField = GetFieldDrawer(
                        position, (property) =>
                        {
                            Vector2 vector2 = property.vector2Value;
                            return (vector2.x, vector2.y);
                        },
                        (property, min, max) => property.vector2Value = new Vector2(min, max),
                        EditorGUI.FloatField
                        );
                    ShowSlider(
                        e => e.vector2Value, (e, v) => e.vector2Value = v,
                        (rect, property, lower, upper) => floatField(rect, property, lower, upper, property.GetGUIContent()),
                        floatField,
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
                case SerializedPropertyType.Vector2Int:
                    Action<Rect, SerializedProperty, int, int, GUIContent> intField = GetFieldDrawer(
                        position, (property) =>
                        {
                            Vector2Int vector2Int = property.vector2IntValue;
                            return (vector2Int.x, vector2Int.y);
                        },
                        (property, min, max) => property.vector2IntValue = new Vector2Int(min, max),
                        EditorGUI.IntField
                        );
                    ShowSlider(
                        e => e.vector2IntValue, (e, v) => e.vector2IntValue = v,
                        (rect, property, lower, upper) => intField(rect, property, lower, upper, property.GetGUIContent()),
                        intField,
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

        public Action<Rect, SerializedProperty, T, T, GUIContent> GetFieldDrawer<T>(
            Rect position,
            Func<SerializedProperty, (T min, T max)> getter, Action<SerializedProperty, T, T> setter,
            Func<Rect, T, T> field)
        {
            float Cast(T v) => (float)Convert.ChangeType(v, typeof(float));
            T UnCast(float v) => (T)Convert.ChangeType(v, typeof(T));
            string GetInvalidCastingErrorMessage() => $"Generic parameter {nameof(T)} {typeof(T)} must be casteable to {typeof(float)} and vice-versa.";
            return (Rect rect, SerializedProperty property, T lower, T upper, GUIContent guiContent) =>
            {
                (T min, T max) = getter(property);
                T oldMin = min, oldMax = max;

                HorizontalRectBuilder rectBuilder = new HorizontalRectBuilder(rect);

                // Make label. We must do this because we have a min field at the left
                // But only do this is we really need, since we add fake whitespace to have the proper slider offset in the RangeAttributeDrawer
                if (!string.IsNullOrWhiteSpace(guiContent.text))
                    EditorGUI.LabelField(position, guiContent);

                // Add the space used by the label or foldout
                rectBuilder.AddSpace(EditorGUIUtility.labelWidth);

                float blockWidth = rectBuilder.RemainingWidth / 5;

                // Min field
                min = field(rectBuilder.GetRect(blockWidth), min);
                rectBuilder.AddSpace(FIELDS_SPACE);

                float floatMin, floatMax, floatLower, floatUpper;
                try
                {
                    floatMin = Cast(min);
                    floatMax = Cast(max);
                    floatLower = Cast(lower);
                    floatUpper = Cast(upper);
                }
                catch (InvalidCastException e)
                {
                    throw new ArgumentException(GetInvalidCastingErrorMessage(), e);
                }

                // Slider
                EditorGUI.MinMaxSlider(rectBuilder.GetRect(blockWidth * 3), new GUIContent("", guiContent.tooltip), ref floatMin, ref floatMax, floatLower, floatUpper);

                try
                {
                    min = UnCast(floatMin);
                    max = UnCast(floatMax);
                }
                catch (InvalidCastException e)
                {
                    throw new ArgumentException(GetInvalidCastingErrorMessage(), e);
                }

                // Max field
                rectBuilder.AddSpace(FIELDS_SPACE);
                max = field(rectBuilder.GetRect(blockWidth), max);

                // Only save if there was a change
                if (min.Equals(oldMin) || max.Equals(oldMax))
                    setter(property, min, max);
                };
        }
    }
}