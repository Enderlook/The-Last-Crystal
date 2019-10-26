using System;
using Range;
using UnityEditor;
using UnityEditorHelper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AdditionalAttributes
{
    [CustomPropertyDrawer(typeof(RangeMinMaxAttribute))]
    internal class MinMaxRangeDrawer : RangeDrawer
    {
        private readonly string error = $"{nameof(RangeMinMaxAttribute)} only supports serialized properties of {nameof(SerializedPropertyType.Vector2Int)} ({typeof(Vector2Int)}) and {nameof(SerializedPropertyType.Vector2)} ({typeof(Vector2)})).";
        private const int FIELDS_SPACE = 2;
        private const string MIN_FIELD_NAME = "min";
        private const string MAX_FIELD_NAME = "max";

        protected override void DrawProperty(Rect position, SerializedProperty serializedProperty)
        {
            void ShowSlider<T, U>(
                Func<SerializedProperty, T> getter, Action<SerializedProperty, T> setter,
                Action<Rect, SerializedProperty, U, U, GUIContent> field,
                Func<U, U, T> random, Func<T, U, T> stepper, Rect? rect = null
            )
            {
                ShowField<T, U>(
                    rect ?? position, serializedProperty, attribute as RangeMinMaxAttribute,
                    getter, setter,
                    (r, property, lower, upper) => field(r, property, lower, upper, property.GetGUIContent()),
                    field, random, stepper
                );
            }

            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Vector2:
                    ShowSlider(
                        e => e.vector2Value, (e, v) => e.vector2Value = v,
                        GetFieldDrawer(
                            position, (property) =>
                            {
                                Vector2 vector2 = property.vector2Value;
                                return (vector2.x, vector2.y);
                            },
                            (property, minF, maxF) => property.vector2Value = new Vector2(minF, maxF),
                            EditorGUI.FloatField
                        ),
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
                    ShowSlider(
                        e => e.vector2IntValue, (e, v) => e.vector2IntValue = v,
                        GetFieldDrawer(
                            position, (property) =>
                            {
                                Vector2Int vector2Int = property.vector2IntValue;
                                return (vector2Int.x, vector2Int.y);
                            },
                            (property, minI, maxI) => property.vector2IntValue = new Vector2Int(minI, maxI),
                            EditorGUI.IntField
                        ),
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
                    string MIN = MIN_FIELD_NAME, MAX = MAX_FIELD_NAME;

                    object objectProperty = serializedProperty.GetTargetObjectOfProperty();
                    Type objectType = objectProperty.GetType();

                    // Replace names
                    bool isRangeObject = false;
                    if (objectType == typeof(RangeIntStep) || objectType == typeof(RangeFloatStep))
                    {
                        MIN = Range.RangeDrawer.MIN_FIELD_NAME;
                        MAX = Range.RangeDrawer.MAX_FIELD_NAME;
                        isRangeObject = true;
                    }

                    SerializedProperty min = serializedProperty.FindPropertyRelative(MIN);
                    if (min == null)
                        break;
                    SerializedProperty max = serializedProperty.FindPropertyRelative(MAX);
                    if (max == null)
                        break;

                    // Only if both properties are the same type allow it
                    if (min.propertyType != max.propertyType)
                        throw new ArgumentException($"Serialized properties {MIN_FIELD_NAME} and {MAX_FIELD_NAME} of Property {serializedProperty.name} must have the same property type.");

                    void FloatSlider(Rect? rect = null)
                    {
                        (float min, float max) GenericFloatGetter(SerializedProperty _) => (min.floatValue, max.floatValue);
                        void GenericFloatSetter(SerializedProperty _, float minValue, float maxValue)
                        {
                            min.floatValue = minValue;
                            max.floatValue = maxValue;
                        }
                        ShowSlider<(float min, float max), float>(
                            GenericFloatGetter, (_, value) => GenericFloatSetter(_, value.min, value.max),
                            GetFieldDrawer(
                                rect ?? position, GenericFloatGetter,
                                GenericFloatSetter,
                                EditorGUI.FloatField
                            ),
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
                            }, rect
                        );
                    }

                    void IntSlider(Rect? rect = null)
                    {
                        (int min, int max) GenericIntGetter(SerializedProperty _) => (min.intValue, max.intValue);
                        void GenericIntSetter(SerializedProperty _, int minValue, int maxValue)
                        {
                            min.intValue = minValue;
                            max.intValue = maxValue;
                        }
                        ShowSlider<(int min, int max), int>(
                            GenericIntGetter, (_, value) => GenericIntSetter(_, value.min, value.max),
                            GetFieldDrawer(
                                rect ?? position, GenericIntGetter,
                                GenericIntSetter,
                                EditorGUI.IntField
                            ),
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
                            }, rect
                        );
                    }

                    if (isRangeObject)
                    {
                        SerializedProperty stepProperty = serializedProperty.FindPropertyRelative(Range.RangeDrawer.STEP_FIELD_NAME);

                        void DrawFieldWithStep(Action<Rect?> slider)
                        {
                            HorizontalRectBuilder horizontalRectBuilder = new HorizontalRectBuilder(position);
                            const float labelWidth = 15; // GUI.skin.label.CalcSize(new GUIContent(stepProperty.displayName)).x;
                            float block = (horizontalRectBuilder.RemainingWidth - labelWidth) / 10;
                            slider(horizontalRectBuilder.GetRect(block * 9));
                            horizontalRectBuilder.AddSpace(5);
                            GUI.Label(horizontalRectBuilder.GetRect(labelWidth), new GUIContent(Range.RangeDrawer.STEP_FIELD_DISPLAY_NAME, stepProperty.tooltip));
                            EditorGUI.PropertyField(horizontalRectBuilder.GetRemainingRect(), stepProperty, new GUIContent("", stepProperty.tooltip));
                        }

                        // Check a property to get type
                        switch (min.propertyType)
                        {
                            case SerializedPropertyType.Float:
                                DrawFieldWithStep(FloatSlider);
                                break;
                            case SerializedPropertyType.Integer:
                                DrawFieldWithStep(IntSlider);
                                break;
                            default:
                                ShowError(position, error + $" Property {MIN_FIELD_NAME}, {MAX_FIELD_NAME}, and {Range.RangeDrawer.STEP_FIELD_NAME} of {serializedProperty.name} are {serializedProperty.propertyType}.");
                                break;
                        }
                    }
                    else
                    {
                        // Only check one of them since both are the same type
                        switch (min.propertyType)
                        {
                            case SerializedPropertyType.Float:
                                FloatSlider();
                                break;
                            case SerializedPropertyType.Integer:
                                IntSlider();
                                break;
                            default:
                                ShowError(position, error + $" Property {MIN_FIELD_NAME} and {MAX_FIELD_NAME} of {serializedProperty.name} are {serializedProperty.propertyType}.");
                                break;
                        }
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