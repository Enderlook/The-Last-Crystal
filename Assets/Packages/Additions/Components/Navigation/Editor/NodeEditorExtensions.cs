using UnityEditor;

using UnityEngine;

namespace Additions.Components.Navigation
{
    public static class NodeEditorExtensions
    {
        public const float nodeDrawSize = 0.05f;

        public static void DrawNode(this Node node, Graph reference = null) => node.DrawNode(NavigationGraphEditor.activeColor, NavigationGraphEditor.disabledColor, reference);

        public static void DrawNode(this Node node, Color active, Color inactive, Graph reference = null) => node.DrawNode(node.IsActive ? active : inactive, reference);

        public static void DrawNode(this Node node, Color color, Graph reference = null)
        {
            Vector2 position = reference == null ? node.position : reference.GetWorldPosition(node);
            Handles.color = color;
            Handles.DrawSolidDisc(position, Vector3.forward, nodeDrawSize);
            if (node.isExtreme)
            {
                Handles.color = NavigationGraphEditor.extremeColor;
                Handles.DrawWireDisc(position, Vector3.forward, nodeDrawSize);
            }
        }

        public static void DrawConnections(this Node node, Graph reference = null, int fontSize = 0) => node.DrawConnections(NavigationGraphEditor.activeColor, NavigationGraphEditor.disabledColor, reference, fontSize);

        public static void DrawConnections(this Node node, Color active, Color inactive, Graph reference = null, int fontSize = 0)
        {
            foreach (Connection connection in node.Connections)
                if (connection != null) // Why this?
                    connection.DrawConnection(active, inactive, reference, fontSize);
        }

        public static void DrawConnections(this Node node, Color color, Graph reference = null, int fontSize = 0)
        {
            foreach (Connection connection in node.Connections)
                if (connection != null) // Why this?
                    connection.DrawConnection(color, reference, fontSize);
        }

        public static void DrawLineTo(Vector2 source, Vector2 target, Color color, float? screenSpaceSize = null)
        {
            Handles.color = color;
            if (screenSpaceSize == null)
                Handles.DrawLine(source, target);
            else
                Handles.DrawDottedLine(source, target, (int)screenSpaceSize);
        }

        public static void DrawLineTo(this Node source, Vector2 target, Color color, Graph reference = null, float? screenSpaceSize = null)
        {
            Vector2 start = reference.GetWorldPosition(source);
            DrawLineTo(start, target, color, screenSpaceSize);
        }

        public static void DrawLineTo(this Node source, Node target, Color color, Graph reference = null, float? screenSpaceSize = null)
        {
            Vector2[] positions = reference.GetWorldPosition(source, target);
            Vector2 start = positions[0];
            Vector2 end = positions[1];

            DrawLineTo(start, end, color, screenSpaceSize);
        }

        public static Vector2 DrawPositionHandler(this Node source, Graph reference = null)
        {
            Vector2 position = reference.GetWorldPosition(source);
            position = reference.GetLocalPosition(Handles.PositionHandle(position, Quaternion.identity));
            source.position = position;
            return position;
        }
    }
}