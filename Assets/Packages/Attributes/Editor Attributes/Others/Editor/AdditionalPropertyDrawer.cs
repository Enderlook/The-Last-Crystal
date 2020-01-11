using Additions.Utils.UnityEditor;

using UnityEditor;

using UnityEngine;

namespace Additions.Attributes
{
    public abstract class AdditionalPropertyDrawer : PropertyDrawer
    {
        protected SerializedPropertyHelper helper;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (helper == null)
                helper = property.GetHelper();
            OnGUIAdditional(position, property, label);
        }

        protected abstract void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label);

        protected void OnGUIBase(Rect position, SerializedProperty property, GUIContent label) => base.OnGUI(position, property, label);
    }
}