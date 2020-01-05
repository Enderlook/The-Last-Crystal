using AdditionalExtensions;

using UnityEditor;

using UnityEngine;

namespace AdditionalAttributes
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    internal class ShowIfDrawer : AdditionalPropertyDrawer
    {
        /// <summary>
        /// If <see langword="true"/>, the property field is either disabled or hidden.
        /// </summary>
        private bool active;

        private ShowIfAttribute.ActionMode mode;

        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIfAttribute = (ShowIfAttribute)attribute;
            mode = showIfAttribute.mode;

            if (helper.TryGetParentTargetObjectOfProperty(out object parent))
            {
                active = parent.GetValueFromFirstMember<bool>(showIfAttribute.nameOfConditional);
                EditorGUI.BeginProperty(position, label, property);
                void DrawField()
                {
                    bool idented = showIfAttribute.indented;
                    if (idented)
                        EditorGUI.indentLevel++;
                    GUIDrawer.GetGUIContent(helper, ref label);
                    EditorGUI.PropertyField(position, property, label, true);
                    if (idented)
                        EditorGUI.indentLevel--;
                }

                active = active == showIfAttribute.goal;
                if (mode == ShowIfAttribute.ActionMode.ShowHide)
                {
                    if (active)
                        DrawField();
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(active);
                    DrawField();
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUI.EndProperty();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => mode == ShowIfAttribute.ActionMode.ShowHide && active ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
    }
}