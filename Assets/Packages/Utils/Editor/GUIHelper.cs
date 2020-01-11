using System;

using UnityEditor;

using UnityEngine;

namespace Additions.Utils.UnityEditor
{
    public static class GUIHelper
    {
        /// <summary>
        /// Add a header.
        /// </summary>
        /// <param name="text">Text of header.</param>
        public static void Header(string text) =>
            // https://www.reddit.com/r/Unity3D/comments/3b43pf/unity_editor_scripting_how_can_i_draw_a_header_in/
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);

        /// <summary>
        /// Draw the grey script field of the target script of this custom editor.
        /// </summary>
        /// <param name="source">Custom editor whose target script field will be draw.</param>
        public static void DrawScriptField(this Editor source)
        {
            // https://answers.unity.com/questions/550829/how-to-add-a-script-field-in-custom-inspector.html
            GUI.enabled = false;
            object target = Convert.ChangeType(source.target, source.target.GetType());
            MonoScript script;
            if (source.target.GetType().IsSubclassOf(typeof(MonoBehaviour)))
                script = MonoScript.FromMonoBehaviour((MonoBehaviour)target);
            else if (source.target.GetType().IsSubclassOf(typeof(ScriptableObject)))
                script = MonoScript.FromScriptableObject((ScriptableObject)target);
            else
                throw new InvalidCastException($"Only support {typeof(MonoBehaviour)} or {typeof(ScriptableObject)}");
            EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
            GUI.enabled = true;
        }
    }
}