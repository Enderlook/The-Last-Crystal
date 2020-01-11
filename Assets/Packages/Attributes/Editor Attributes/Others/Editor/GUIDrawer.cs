using Additions.Extensions;
using Additions.Utils.UnityEditor;

using UnityEditor;

using UnityEngine;

namespace Additions.Attributes
{
    [CustomPropertyDrawer(typeof(GUIAttribute))]
    internal class GUIDrawer : AdditionalPropertyDrawer
    {
        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label) =>
            // GetGUIContent is already called by AdditionalPropertyDrawer
            EditorGUI.PropertyField(position, property, label, true);

        private static GUIContent GetGUIContent(GUIAttribute attribute, SerializedPropertyHelper helper, GUIContent label)
        {
            string text = null, tooltip = null;

            if (attribute.guiContentOrReferenceName == null)
            {
                bool reference = false;
                if (attribute.nameMode == GUIAttribute.Mode.Value)
                    text = attribute.name;
                else
                    reference = true;

                if (attribute.tooltipMode == GUIAttribute.Mode.Value)
                    tooltip = attribute.tooltip;
                else
                    reference = true;

                if (reference && helper.TryGetParentTargetObjectOfProperty(out object parent))
                {
                    if (attribute.nameMode == GUIAttribute.Mode.Reference)
                        text = parent.GetValueFromFirstMember<string>(attribute.name);
                    if (attribute.tooltipMode == GUIAttribute.Mode.Reference)
                        tooltip = parent.GetValueFromFirstMember<string>(attribute.name);
                }

                return new GUIContent(text ?? label.text, label.image, tooltip ?? label.tooltip);
            }
            else if (helper.TryGetParentTargetObjectOfProperty(out object parent))
                try
                {
                    return parent.GetValueFromFirstMember<GUIContent>(attribute.guiContentOrReferenceName);
                }
                catch (MatchingMemberNotFoundException)
                {
                    text = parent.GetValueFromFirstMember<string>(attribute.guiContentOrReferenceName);
                    return new GUIContent(text ?? label.text, label.image, label.tooltip);
                }

            return label;
        }

        /// <summary>
        /// Check if the <see cref="SerializedProperty"/> does have a <see cref="GUIAttribute"/> <see cref="Attribute"/> and if has change <paramref name="label"/> by its <see cref="GUIContent"/>.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="label">Current <see cref="GUIContent"/>.</param>
        /// <returns>Whenever there was or not an special <see cref="GUIContent"/>.</returns>
        public static bool GetGUIContent(SerializedPropertyHelper helper, ref GUIContent label)
        {
            if (helper.TryGetAttributeFromField(out GUIAttribute guiAttribute))
            {
                label = GetGUIContent(guiAttribute, helper, label);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the <see cref="SerializedProperty"/> does have a <see cref="GUIAttribute"/> <see cref="Attribute"/> and if has change <paramref name="label"/> by its <see cref="GUIContent"/>.
        /// </summary>
        /// <param name="serializedProperty"></param>
        /// <param name="label">Current <see cref="GUIContent"/>.</param>
        /// <returns>Whenever there was or not an special <see cref="GUIContent"/>.</returns>
        public static bool GetGUIContent(SerializedProperty serializedProperty, ref GUIContent label)
        {
            SerializedPropertyHelper helper = serializedProperty.GetHelper();
            if (helper.TryGetAttributeFromField(out GUIAttribute guiAttribute))
            {
                label = GetGUIContent(guiAttribute, helper, label);
                return true;
            }
            return false;
        }
    }
}