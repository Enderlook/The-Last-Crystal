using System;
using UnityEditor;
using UnityEditorHelper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AdditionalAttributes.Drawer
{
    [CustomPropertyDrawer(typeof(RangeAttribute))]
    public class RangeAttributeDrawer : PropertyDrawer
    {
        private readonly string error = $"{nameof(RangeAttributeDrawer)} only supports serialized properties of {nameof(SerializedPropertyType.Integer)} ({typeof(int)}) and {nameof(SerializedPropertyType.Float)} ({typeof(float)})).";

        private bool foldout;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (foldout)
                position.height -= EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginProperty(position, label, property);

            DrawProperty(position, property);

            EditorGUI.EndProperty();
        }

        protected virtual void DrawProperty(Rect position, SerializedProperty property)
        {
            void ShowSlider<T>(
                Func<SerializedProperty, T> getter, Action<SerializedProperty, T> setter,
                Action<Rect, SerializedProperty, T, T> field, Action<Rect, SerializedProperty, T, T, GUIContent> field2,
                Func<T, T, T> random, Func<T, T, T> stepper
            ) => ShowField(position, property, attribute as RangeAttribute, getter, setter, field, field2, random, stepper);

            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    ShowSlider(
                        e => e.floatValue, (e, v) => e.floatValue = v, EditorGUI.Slider, EditorGUI.Slider,
                        (l, u) => Random.Range(l, u), (v, s) => (float)Math.Round(v / s, MidpointRounding.AwayFromZero) * s
                    );
                    break;
                case SerializedPropertyType.Integer:
                    ShowSlider(
                        e => e.intValue, (e, v) => e.intValue = v, EditorGUI.IntSlider, EditorGUI.IntSlider,
                        (l, u) => Random.Range(l, u), (v, s) => v / s * s
                    );
                    break;
                default:
                    string tooltip = error + $" Property {property.name} is {property.propertyType}.";
                    EditorGUI.HelpBox(position, tooltip, MessageType.Error);
                    Debug.LogException(new ArgumentException(tooltip));
                    break;
            }
        }

        protected void ShowField<T, U>(
            Rect position, SerializedProperty property, RangeAttribute rangeAttribute,
            Func<SerializedProperty, T> getter, Action<SerializedProperty, T> setter,
            Action<Rect, SerializedProperty, U, U> field, Action<Rect, SerializedProperty, U, U, GUIContent> field2,
            Func<U, U, T> random, Func<T, U, T> stepper
            )
        {
            T Get() => getter(property);
            void Set(T v) => setter(property, v);

            U min = (U)(object)rangeAttribute.min;
            U max = (U)(object)rangeAttribute.max;
            U step = (U)(object)rangeAttribute.step;

            T oldValue = Get();

            bool hasStep = !Equals(step, default(U));

            if (rangeAttribute.showRandomButton == false)
                // Show normal slider
                field(position, property, min, max);
            else
            {
                // Show slider without label. Using " " instead of "" forces the slider to set space for the label
                // We take advantage of that in order to aling it with the foldout.
                field2(position, property, min, max, new GUIContent(" ", property.tooltip));
                if ((foldout = EditorGUI.Foldout(position, foldout, property.GetGUIContent(), true)) &&
                    GUI.Button(
                        new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height),
                        new GUIContent("Randomize", $"Produce a random value from {min} to {max}{(hasStep ? $" in steps of {step}" : "")}.")
                    ))
                    Set(random(min, max));
            }

            // Only round if necessary
            T value = Get();
            if (hasStep && !Equals(value, oldValue))
                Set(stepper(value, step));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property) + (foldout ? EditorGUIUtility.singleLineHeight : 0);
    }
}