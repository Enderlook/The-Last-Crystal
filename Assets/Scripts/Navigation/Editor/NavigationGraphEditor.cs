using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Navigation.UnityInspector
{
    [CustomEditor(typeof(NavigationGraph))]
    public class NavigationGraphEditor : Editor
    {
        private NavigationGraph navigationGraph;
        private List<Node> Grid => navigationGraph.Grid;

        private const float nodeDrawSize = NodeEditorExtensions.nodeDrawSize;

        private bool drawNodes = true;
        private bool drawConnections = true;
        private bool drawDistances = true;

        private bool isEditingEnable = false;
        private float autoSelectionRange = 0.25f;
        private Node selectedNode;

        private static bool showColorConfiguration = true;
        private Color addColor = Color.magenta;
        private Color selectedColor = Color.white;
        private Color closestColor = Color.black;
        private Color activeColor = Color.green;
        private Color disabledColor = Color.red;

        private static bool showHelp = true;
        private static bool explainedHelp = false;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            navigationGraph = (NavigationGraph)target;

            if (GUILayout.Button("Generate Automated Grid"))
            {
                navigationGraph.ResetGrid();
                navigationGraph.GenerateGrid();
            }

            if (GUILayout.Button("Reset Grid"))
                navigationGraph.ResetGrid();

            EditorGUILayout.Space();

            drawNodes = EditorGUILayout.Toggle("Draw Nodes", drawNodes);
            drawConnections = EditorGUILayout.Toggle("Draw Connections", drawConnections);

            if (drawConnections)
            {
                EditorGUI.indentLevel++;
                drawDistances = EditorGUILayout.Toggle("Draw Distances", drawDistances);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            isEditingEnable = EditorGUILayout.Toggle(new GUIContent("Enable Editing", "Enable editing tools and lock inspector window.\nTo unlock inspector this must be unchecked."), isEditingEnable);
            if (isEditingEnable)
            {
                // Lock inspector window so we don't lose focus of it when we click in the scene
                ActiveEditorTracker.sharedTracker.isLocked = true;

                EditorGUI.indentLevel++;
                autoSelectionRange = EditorGUILayout.FloatField("Auto Selection Range", autoSelectionRange);

                showColorConfiguration = EditorGUILayout.Foldout(showColorConfiguration, "Color Configuration", true);
                if (showColorConfiguration)
                {
                    addColor = EditorGUILayout.ColorField("Add", addColor);
                    selectedColor = EditorGUILayout.ColorField("Selected", selectedColor);
                    closestColor = EditorGUILayout.ColorField("Closest", closestColor);
                    activeColor = EditorGUILayout.ColorField("Active", activeColor);
                    disabledColor = EditorGUILayout.ColorField("Disabled", disabledColor);
                }

                showHelp = EditorGUILayout.Foldout(showHelp, "Help", true);
                if (showHelp)
                {
                    GUILayout.Label("Controls", EditorStyles.boldLabel);
                    GUILayout.Label("L: Left Mouse Button"
                                  + "\nR: Right Mouse Button"
                                  + "\nS: Shift Key"
                                  + "\nC: Control Key"
                                  + "\nA: Alt Key", EditorStyles.helpBox);

                    explainedHelp = EditorGUILayout.Toggle("Explained Help", explainedHelp);
                    GUILayout.Label("Commands", EditorStyles.boldLabel);
                    if (explainedHelp)
                        GUILayout.Label(
                            "L: Select closest node in range. If there is no near node make a new one in position. Used to perform connections."
                            + "\nL+C: Enable or disable closest node in range."
                            + "\nL+S+C: Remove selected node."
                            + "\nR: Connect selected node to closest node in range. If there is no near node make a new one in position and connect to it."
                            + "\nR+C: Same as [R] but in opposite direction, instead of connect selected to closest, connect closest to selected."
                            + "\nR+A: Do [R] and [R+C] (connect in both ways)."
                            + "\nR+S: Do [R], then select the closest node."
                            + "\nR+C+S: Do [R+C], then select the closest node."
                            + "\nR+C+A+S: Do [R+S] and [R+C+S] (connect in both ways, then select closest node)."
                        , EditorStyles.helpBox);
                    else
                        GUILayout.Label(
                            "L: Select Closest / Create Node."
                            + "\nL+C: Enable / Disable Node."
                            + "\nL+S+C: Remove Selected Node."
                            + "\nR: Connect Selected Node to Closest (Add)."
                            + "\nR+C: Connect Closest (Add) Node to Selected."
                            + "\nR+A: Do [R] and [R+C]."
                            + "\nR+S: Do [R] and select closest."
                            + "\nR+C+S: Do [R+S] and select closest."
                            + "\nR+C+A+S: Do [R+S] and [R+C+S]."
                        , EditorStyles.helpBox);
                }
                EditorGUI.indentLevel--;
            }
        }

        public void OnSceneGUI()
        {
            if (navigationGraph == null)
                return;
            if (drawNodes || drawConnections)
                DrawNodesAndConnections();
            if (isEditingEnable)
                EditingLogic();
        }

        private void DrawNodesAndConnections()
        {
            foreach (Node node in Grid)
            {
                if (drawNodes)
                    node.DrawNode(activeColor, disabledColor);
                if (drawConnections)
                    node.DrawConnections(activeColor, disabledColor, drawDistances ? 14 : 0);
            }
        }

        private void EditingLogic()
        {
            Vector2 mousePosition = GetAndDrawMousePosition();

            Node closestNode = GetAndDrawClosestNode(mousePosition);

            if (selectedNode != null)
                DrawSelectedNode(closestNode, mousePosition);

            Event e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    if (e.shift)
                    {
                        if (e.control)
                        {
                            // Remove or Recreate Node
                            if (selectedNode != null)
                            {
                                // If the node already exist in the grid, remove it
                                for (int i = 0; i < Grid.Count; i++)
                                {
                                    if (Grid[i] == selectedNode)
                                    {
                                        Grid.RemoveAt(i);
                                        continue;
                                    }
                                    Node node = Grid[i];
                                    for (int j = 0; i < node.connections.Count; i++)
                                    {
                                        if (node.connections[j].end == node)
                                            node.connections[j] = null;
                                    }
                                }
                                selectedNode = null;
                            }
                        }
                    }
                    else if (e.control)
                    {
                        // Switch Node
                        if (closestNode != null)
                            closestNode.SetActive(!closestNode.IsActive);
                    }
                    else
                    {
                        if (closestNode == null)
                            // Add Node
                            Grid.Add(new Node(mousePosition, true));
                        else
                            // Select Node
                            selectedNode = closestNode;
                    }
                }
                else if (e.button == 1)
                {
                    if (selectedNode != null) {
                        Node to;
                        if (closestNode != null && closestNode != selectedNode)
                            // Connect to closest Node
                            to = closestNode;
                        else
                        {
                            // Make new node to connect
                            to = new Node(mousePosition, true);
                            navigationGraph.Grid.Add(to);
                        }
                        if (e.control || e.alt)
                            // Connect inverse
                            to.AddConnectionTo(selectedNode, true);
                        if (e.alt || !e.control)
                            // Connect Node
                            selectedNode.AddConnectionTo(to, true);


                        if (e.shift)
                            // Select target node
                            selectedNode = to;
                    }
                }
            }
        }

        private Vector2 GetAndDrawMousePosition()
        {
            Vector2 mousePosition = NavigationExtensions.GetMousePosition();
            Handles.color = addColor;
            Handles.DrawWireDisc(mousePosition, Vector3.forward, nodeDrawSize);
            Handles.DrawWireDisc(mousePosition, Vector3.forward, autoSelectionRange);
            return mousePosition;
        }

        private Node GetAndDrawClosestNode(Vector2 mousePosition)
        {
            Node closestNode = navigationGraph.FindClosestNode(mousePosition, autoSelectionRange, NavigationExtensions.NodeType.ALL);
            if (closestNode != null)
            {
                Handles.color = closestColor;
                Handles.DrawWireDisc(closestNode.position, Vector3.forward, nodeDrawSize);
            }
            return closestNode;
        }

        private void DrawSelectedNode(Node closestNode, Vector2 mousePosition)
        {
            Handles.color = selectedColor;
            Handles.DrawWireDisc(selectedNode.position, Vector3.forward, nodeDrawSize);

            if (closestNode != null)
            {
                Handles.color = selectedColor;
                Handles.DrawDottedLine(closestNode.position, selectedNode.position, 1);
                ConnectionEditorExtensions.DrawDistance(selectedNode.position, closestNode.position, selectedColor, 14);
            }
            else
            {
                Handles.color = addColor;
                Handles.DrawDottedLine(selectedNode.position, mousePosition, 1);
                ConnectionEditorExtensions.DrawDistance(selectedNode.position, mousePosition, addColor, 14);
            }

            selectedNode.position = Handles.PositionHandle(selectedNode.position, Quaternion.identity);
        }
    }

    public static class NodeEditorExtensions
    {
        public const float nodeDrawSize = 0.05f;

        public static void DrawNode(this Node node, Color active, Color inactive) => node.DrawNode(node.IsActive ? active : inactive);

        public static void DrawNode(this Node node, Color color)
        {
            Handles.color = color;
            Handles.DrawSolidDisc(node.position, Vector3.forward, nodeDrawSize);
        }

        public static void DrawConnections(this Node node, Color active, Color inactive, int fontSize = 0)
        {
            foreach (Connection connection in node.connections)
            {
                connection?.DrawConnection(active, inactive, fontSize);
            }
        }

        public static void DrawConnections(this Node node, Color color, int fontSize = 0)
        {
            foreach (Connection connection in node.connections)
            {
                connection?.DrawConnection(color, fontSize);
            }
        }
    }

    public static class ConnectionEditorExtensions
    {
        public const float arrowDrawSize = 0.05f;

        public static void DrawConnection(this Connection connection, Color active, Color inactive, int fontSize = 0) => connection.DrawConnection(connection.IsActive ? active : inactive, fontSize);

        public static void DrawConnection(this Connection connection, Color color, int fontSize = 0)
        {
            Handles.color = color;
            Handles.DrawLine(connection.start.position, connection.end.position);
            Vector2 half = (connection.start.position + connection.end.position) / 2;

            // Draw arrow
            Handles.DrawSolidArc(half, Vector3.forward, (connection.start.position - connection.end.position).normalized, 35, arrowDrawSize);
            Handles.DrawSolidArc(half, Vector3.forward, (connection.start.position - connection.end.position).normalized, -35, arrowDrawSize);
            if (fontSize > 0)
                DrawDistance(connection.start.position, connection.end.position, color, fontSize);
        }

        public static void DrawDistance(Vector2 a, Vector2 b, Color textColor, int fontSize = 10)
        {
            GUIStyle style = new GUIStyle
            {
                fontSize = fontSize
            };
            style.normal.textColor = textColor;
            GUIContent content = new GUIContent(Vector2.Distance(a, b).ToString("0.##"));
            Handles.Label((a + b) / 2, content, style);
        }
    }
}