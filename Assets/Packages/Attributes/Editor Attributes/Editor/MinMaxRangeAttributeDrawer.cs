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
                        (property, minF, maxF) => property.vector2Value = new Vector2(minF, maxF),
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
                            Func<float, float> Stp = FloatStep(step);
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
                        (property, minI, maxI) => property.vector2IntValue = new Vector2Int(minI, maxI),
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
                            Func<int, int> Stp = IntStep(step);
                            return new Vector2Int(Stp(value.x), Stp(value.y));
                        }
                    );
                    break;
                case SerializedPropertyType.Generic:
                    const string MIN = "min";
                    const string MAX = "max";

                    SerializedProperty min = serializedProperty.FindPropertyRelative(MIN);
                    if (min == null)
                        break;
                    SerializedProperty max = serializedProperty.FindPropertyRelative(MAX);
                    if (max == null)
                        break;

                    // Only if both properties exist allow it
                    if (min.propertyType != max.propertyType)
                        throw new ArgumentException($"Serialized properties {MIN} and {MAX} of Property {serializedProperty.name} must have the same property type.");

                    // Only check one of them since both are the same type
                    switch (min.propertyType)
                    {
                        case SerializedPropertyType.Float:
                            (float min, float max) GenericFloatGetter(SerializedProperty _) => (min.floatValue, max.floatValue);
                            void GenericFloatSetter(SerializedProperty _, float minValue, float maxValue)
                            {
                                min.floatValue = minValue;
                                max.floatValue = maxValue;
                            }
                            Action<Rect, SerializedProperty, float, float, GUIContent> genericFloatField = GetFieldDrawer(
                                position, GenericFloatGetter,
                                GenericFloatSetter,
                                EditorGUI.FloatField
                            );
                            ShowSlider<(float min, float max), float>(
                                GenericFloatGetter, (_, value) => GenericFloatSetter(_, value.min, value.max),
                                (rect, property, lower, upper) => genericFloatField(rect, property, lower, upper, property.GetGUIContent()),
                                genericFloatField,
                                (lower, upper) =>
                                {
                                    float Rnd() => Random.Range(lower, upper);
                                    float a = Rnd(), b = Rnd();
                                    return (Mathf.Min(a, b), Mathf.Max(a, b));
                                },
                                (value, step) =>
                                {
                                    Func<float, float> Stp = FloatStep(step);
                                    return (Stp(value.min), Stp(value.max));
                                }
                            );
                            break;
                        case SerializedPropertyType.Integer:
                            (int min, int max) GenericIntGetter(SerializedProperty _) => (min.intValue, max.intValue);
                            void GenericIntSetter(SerializedProperty _, int minValue, int maxValue)
                            {
                                min.intValue = minValue;
                                max.intValue = maxValue;
                            }
                            Action<Rect, SerializedProperty, int, int, GUIContent> genericintField = GetFieldDrawer(
                                position, GenericIntGetter,
                                GenericIntSetter,
                                EditorGUI.IntField
                            );
                            ShowSlider<(int min, int max), int>(
                                GenericIntGetter, (_, value) => GenericIntSetter(_, value.min, value.max),
                                (rect, property, lower, upper) => genericintField(rect, property, lower, upper, property.GetGUIContent()),
                                genericintField,
                                (lower, upper) =>
                                {
                                    int Rnd() => Random.Range(lower, upper);
                                    int a = Rnd(), b = Rnd();
                                    return (Mathf.Min(a, b), Mathf.Max(a, b));
                                },
                                (value, step) =>
                                {
                                    Func<int, int> Stp = IntStep(step);
                                    return (Stp(value.min), Stp(value.max));
                                }
                            );
                            break;
                        default:
                            ShowError(position, error + $" Property {MIN} and {MAX} of {serializedProperty.name} are {serializedProperty.propertyType}.");
                            break;
                    }

                    break;
                default:
                    ShowError(position, error + $" Property {serializedProperty.name} is {serializedProperty.propertyType}.");
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

        private Func<float, float> FloatStep(float step) => value => (float)Math.Round(value / step, MidpointRounding.AwayFromZero) * step;
        private Func<int, int> IntStep(int step) => value => value / step * step;

        private void ShowError(Rect position, string tooltip)
        {
            EditorGUI.HelpBox(position, tooltip, MessageType.Error);
            Debug.LogException(new ArgumentException(tooltip));
        }
    }
}