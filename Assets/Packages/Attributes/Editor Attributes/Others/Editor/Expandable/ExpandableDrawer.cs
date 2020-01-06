using UnityEditor;

using UnityEngine;

namespace AdditionalAttributes
{
    //[CustomPropertyDrawer(typeof(UnityEngine.Object), true)] // Will affect all objects
    //[CustomPropertyDrawer(typeof(ScriptableObject), true)] // Will only affect scriptable objects
    [CustomPropertyDrawer(typeof(ExpandableAttribute), true)]
    internal class ExpandableDrawer : AdditionalPropertyDrawer
    {
        // https://forum.unity.com/threads/editor-tool-better-scriptableobject-inspector-editing.484393/

        // Cached scriptable object editor
        private Editor editor;

        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);

            // If we have a value
            if (property.objectReferenceValue != null)
            {
                // We can make the field expandable with a Foldout
                // No GUIContent because the property field below already has it.
                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none, true);
            }

            // If the foldout is expanded
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                if (editor == null)
                    Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
                // Check again because it may not be created by the Editor.CreateChachedEditor
                if (editor != null)
                {
                    EditorGUI.BeginChangeCheck();
                    editor.OnInspectorGUI();
                    if (EditorGUI.EndChangeCheck())
                        property.serializedObject.ApplyModifiedProperties();
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}