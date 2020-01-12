using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Additions.Components.Navigation
{
    public static class ConnectionEditorExtensions
    {
        public const float arrowDrawSize = 0.05f;

        public static void DrawConnection(this Connection connection, Graph reference = null, int fontSize = 0) => connection.DrawConnection(NavigationGraphEditor.activeColor, NavigationGraphEditor.disabledColor, reference, fontSize);

        public static void DrawConnection(this Connection connection, Color active, Color inactive, Graph reference = null, int fontSize = 0) => connection.DrawConnection(connection.IsActive ? active : inactive, reference, fontSize);

        public static void DrawConnection(this Connection connection, Color color, Graph reference = null, int fontSize = 0)
        {
            Vector2[] positions = reference.GetWorldPosition(connection.start, connection.end);
            Vector2 start = positions[0];
            Vector2 end = positions[1];

            Handles.color = color;
            Handles.DrawLine(start, end);
            Vector2 half = (start + end) / 2;

            // Draw arrow
            Handles.DrawSolidArc(half, Vector3.forward, (start - end).normalized, 35, arrowDrawSize);
            Handles.DrawSolidArc(half, Vector3.forward, (start - end).normalized, -35, arrowDrawSize);
            if (fontSize > 0)
                start.DrawDistance(end, color, fontSize);

            if (connection.IsExtreme)
            {
                Handles.color = NavigationGraphEditor.extremeColor;
                Handles.DrawWireArc(half, Vector3.forward, (start - end).normalized, 35, arrowDrawSize);
                Handles.DrawWireArc(half, Vector3.forward, (start - end).normalized, -35, arrowDrawSize);
            }
        }

        public static void DrawDistance(this Vector2 a, Vector2 b, Color textColor, int fontSize = 10)
        {
            GUIStyle style = new GUIStyle
            {
                fontSize = fontSize
            };
            style.normal.textColor = textColor;
            GUIContent content = new GUIContent(Vector2.Distance(a, b).ToString("0.##"));
            Handles.Label((a + b) / 2, content, style);
        }

        public static void DrawDistance(this Node source, Node target, Color textColor, Graph reference = null, int fontSize = 10)
        {
            Vector2[] positions = reference.GetWorldPosition(source, target);
            positions[0].DrawDistance(positions[1], textColor, fontSize);
        }

        public static void DrawDistance(this Node source, Vector2 target, Color textColor, Graph reference = null, int fontSize = 10)
        {
            Vector2 start = reference.GetWorldPosition(source);
            start.DrawDistance(target, textColor, fontSize);
        }

        public static Vector2[] GetWorldPosition(this Graph reference, params Node[] nodes) => reference == null ? nodes.Select(e => e.position).ToArray() : nodes.Select(e => reference.GetWorldPosition(e)).ToArray();
    }
}