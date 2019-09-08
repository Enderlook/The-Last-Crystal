using UnityEditor;

public class GUIHelper
{
    /// <summary>
    /// Add a header.
    /// </summary>
    /// <param name="text">Text of header</param>
    public static void Header(string text)
    {
        // https://www.reddit.com/r/Unity3D/comments/3b43pf/unity_editor_scripting_how_can_i_draw_a_header_in/
        EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
    }
}
