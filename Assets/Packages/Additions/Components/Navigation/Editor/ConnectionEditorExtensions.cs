using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Additions.Components.Navigation
{
    public static class ConnectionEditorExtensions
    {
        public const float arrowDrawSize = 0.05f;

        public static void DrawConnection(this Connection connection, Graph reference = null, int fontSize = 0, FontStyle fontStyle = FontStyle.Normal, float dottedSize = -1) => connection.DrawConnection(NavigationGraphEditor.activeColor, NavigationGraphEditor.disabledColor, reference, fontSize, fontStyle, dottedSize);

        public static void DrawConnection(this Connection connection, Color active, Color inactive, Graph reference = null, int fontSize = 0, FontStyle fontStyle = FontStyle.Normal, float dottedSize = -1) => connection.DrawConnection(connection.IsActive ? active : inactive, reference, fontSize, fontStyle, dottedSize);

        public static void DrawConnection(this Connection connection, Color color, Graph reference = null, int fontSize = 0, FontStyle fontStyle = FontStyle.Normal, float dottedSize = -1)
        {
            Vector2[] positions = reference.GetWorldPosition(connection.start, connection.end);
            Vector2 start = positions[0];
            Vector2 end = positions[1];

            Handles.color = color;
            if (dottedSize >= 0)
                Handles.DrawDottedLine(start, end, dottedSize);
            else
                Handles.DrawLine(start, end);
            Vector2 half = (start + end) / 2;

            // Draw arrow
            Handles.DrawSolidArc(half, Vector3.forward, (start - end).normalized, 35, arrowDrawSize);
            Handles.DrawSolidArc(half, Vector3.forward, (start - end).normalized, -35, arrowDrawSize);
            if (fontSize > 0)
                start.DrawDistance(end, color, fontSize, fontStyle);

            if (connection.IsExtreme)
            {
                Handles.color = NavigationGraphEditor.extremeColor;
                Handles.DrawWireArc(half, Vector3.forward, (start - end).normalized, 35, arrowDrawSize);
                Handles.DrawWireArc(half, Vector3.forward, (start - end).normalized, -35, arrowDrawSize);
            }
        }

        public static void DrawNumber(this Connection connection, float number, Graph reference = null, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal) => connection.DrawNumber(number, NavigationGraphEditor.activeColor, NavigationGraphEditor.disabledColor, reference, fontSize, fontStyle);

        public static void DrawNumber(this Connection connection, float number, Color active, Color inactive, Graph reference = null, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal)
        {
            Vector2[] positions = reference.GetWorldPosition(connection.start, connection.end);
            DrawNumber(positions[0], positions[1], number, connection.IsActive ? active : inactive, fontSize, fontStyle);
        }

        public static void DrawNumber(this Connection connection, float number, Color textColor, Graph reference = null, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal)
        {
            Vector2[] positions = reference.GetWorldPosition(connection.start, connection.end);
            DrawNumber(positions[0], positions[1], number, textColor, fontSize, fontStyle);
        }

        public static void DrawNumber(Vector2 a, Vector2 b, float number, Color textColor, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal)
        {
            GUIStyle style = GetTextStyle(textColor, fontSize, fontStyle);
            GUIContent content = new GUIContent(number.ToString("0.##"));
            Handles.Label((a + b) / 2, content, style);
        }

        private static void DrawDistance(this Vector2 a, Vector2 b, Color textColor, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal) => DrawNumber(a, b, Vector2.Distance(a, b), textColor, fontSize, fontStyle);

        private static GUIStyle GetTextStyle(Color textColor, int fontSize, FontStyle fontStyle = FontStyle.Normal)
        {
            GUIStyle style = new GUIStyle
            {
                fontSize = fontSize
            };
            style.normal.textColor = textColor;
            style.fontStyle = fontStyle;
            return style;
        }

        public static void DrawDistance(this Connection connection, Graph reference = null, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal) => connection.DrawDistance(NavigationGraphEditor.activeColor, NavigationGraphEditor.disabledColor, reference, fontSize, fontStyle);

        public static void DrawDistance(this Connection connection, Color active, Color inactive, Graph reference = null, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal) => connection.DrawDistance(connection.IsActive ? active : inactive, reference, fontSize, fontStyle);

        public static void DrawDistance(this Connection connection, Color textColor, Graph reference = null, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal)
        {
            Vector2[] positions = reference.GetWorldPosition(connection.start, connection.end);
            positions[0].DrawDistance(positions[1], textColor, fontSize, fontStyle);
        }

        public static void DrawDistance(this Node source, Node target, Color textColor, Graph reference = null, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal)
        {
            Vector2[] positions = reference.GetWorldPosition(source, target);
            positions[0].DrawDistance(positions[1], textColor, fontSize, fontStyle);
        }

        public static void DrawDistance(this Node source, Vector2 target, Color textColor, Graph reference = null, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal)
        {
            Vector2 start = reference.GetWorldPosition(source);
            start.DrawDistance(target, textColor, fontSize, fontStyle);
        }

        public static Vector2[] GetWorldPosition(this Graph reference, params Node[] nodes) => reference == null ? nodes.Select(e => e.position).ToArray() : nodes.Select(e => reference.GetWorldPosition(e)).ToArray();
    }
}