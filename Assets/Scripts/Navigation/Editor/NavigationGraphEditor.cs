using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Navigation.UnityInspector
{
    [CustomEditor(typeof(NavigationGraph))]
    public class NavigationGraphEditor : Editor
    {
        private NavigationGraph navigationGraph;
        private List<Node> Grid => navigationGraph.Grid;

        private bool showDrawingMenu = true;
        private bool drawNodes = true;
        private bool drawConnections = true;
        private bool drawDistances = true;

        private bool isEditingEnable = false;
        private float autoSelectionRange = 0.25f;
        private Node selectedNode;

        private static bool showColorConfigurationMenu = true;
        private Color addColor = Color.magenta;
        private Color selectedColor = Color.white;
        private Color closestColor = Color.black;
        private Color activeColor = Color.green;
        private Color disabledColor = Color.red;

        private static bool showHelp = true;
        private static bool explainedHelp = false;

        private static bool showGridGenerationConfigurationMenu = false;

        private static GUIStyle BOLDED_FOLDOUT => new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            navigationGraph = (NavigationGraph)target;

            // https://answers.unity.com/questions/550829/how-to-add-a-script-field-in-custom-inspector.html
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour(navigationGraph), typeof(MonoScript), false);
            GUI.enabled = true;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("graph").FindPropertyRelative("reference"));

            if (navigationGraph.graph.reference == null)
                navigationGraph.graph.reference = navigationGraph.transform;

            if (GUILayout.Button("Reset Grid"))
            {
                navigationGraph.ResetGrid();
                selectedNode = null;
            }

            EditorGUILayout.Space();

            ShowDrawingMenu();

            EditorGUILayout.Space();

            EditingMenu();

            EditorGUILayout.Space();

            ShowGridGenerationConfigurationMenu();

            serializedObject.ApplyModifiedProperties();
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

        private void ShowGridGenerationConfigurationMenu()
        {
            showGridGenerationConfigurationMenu = EditorGUILayout.Foldout(showGridGenerationConfigurationMenu, "Grid Generation Configuration", true, BOLDED_FOLDOUT);
            if (showGridGenerationConfigurationMenu)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("spacePerNode"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rows"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("columns"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyMask"));

                if (GUILayout.Button("Generate Automated Grid"))
                {
                    navigationGraph.ResetGrid();
                    navigationGraph.GenerateGrid();
                }
            }
        }

        private void ShowDrawingMenu()
        {
            showDrawingMenu = EditorGUILayout.Foldout(showDrawingMenu, "Visibility Configuration", true, BOLDED_FOLDOUT);
            if (showDrawingMenu)
            {
                drawNodes = EditorGUILayout.Toggle("Draw Nodes", drawNodes);
                drawConnections = EditorGUILayout.Toggle("Draw Connections", drawConnections);

                if (drawConnections)
                {
                    EditorGUI.indentLevel++;
                    drawDistances = EditorGUILayout.Toggle("Draw Distances", drawDistances);
                    EditorGUI.indentLevel--;
                }

                GUILayout.Label("Colors", EditorStyles.boldLabel);
                activeColor = EditorGUILayout.ColorField("Active", activeColor);
                disabledColor = EditorGUILayout.ColorField("Disabled", disabledColor);
            }
        }

        private void EditingMenu()
        {
            isEditingEnable = EditorGUILayout.Foldout(isEditingEnable, new GUIContent("Editing Tool", "While open, enable editing tools and lock inspector window.\nTo unlock inspector this must be closed."), true, BOLDED_FOLDOUT);
            if (isEditingEnable)
            {
                // Lock inspector window so we don't lose focus of it when we click in the scene
                ActiveEditorTracker.sharedTracker.isLocked = true;

                autoSelectionRange = EditorGUILayout.FloatField("Auto Selection Range", autoSelectionRange);

                showColorConfigurationMenu = EditorGUILayout.Foldout(showColorConfigurationMenu, "Color Configuration", true);
                if (showColorConfigurationMenu)
                {
                    addColor = EditorGUILayout.ColorField("Add", addColor);
                    selectedColor = EditorGUILayout.ColorField("Selected", selectedColor);
                    closestColor = EditorGUILayout.ColorField("Closest", closestColor);
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
                            + "\nL+S: Enable or disable connection from selected to closest node."
                            + "\nL+C: Same as [L+S] but in opposite direction, instead of switch the connection from selected to closes, switch closest to selected."
                            + "\nL+S+C: Do [L+S] and [L+C] (switch bot connections)."
                            + "\nL+A: Remove selected node."
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
            }
        }

        private void DrawNodesAndConnections()
        {
            foreach (Node node in Grid)
            {
                if (drawNodes)
                    node.DrawNode(activeColor, disabledColor, navigationGraph.graph);
                if (drawConnections)
                    node.DrawConnections(activeColor, disabledColor, navigationGraph.graph, drawDistances ? 14 : 0);
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
                            if (e.alt)
                            {
                                // Switch Both Connections
                                if (closestNode != null && selectedNode != null && closestNode != selectedNode)
                                {
                                    Connection connection = selectedNode.GetConnectionTo(closestNode);
                                    if (connection != null)
                                        connection.SetActive(!connection.IsActive);

                                    connection = closestNode.GetConnectionTo(selectedNode);
                                    if (connection != null)
                                        connection.SetActive(!connection.IsActive);
                                }
                            }
                            else
                            {
                                // Switch Inverse Connection
                                if (closestNode != null && selectedNode != null && closestNode != selectedNode)
                                {
                                    Connection connection = closestNode.GetConnectionTo(selectedNode);
                                    if (connection != null)
                                        connection.SetActive(!connection.IsActive);
                                }
                            }
                        }
                        else if (e.alt)
                        {
                            // Remove Node
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
                                    for (int j = 0; i < node.Connections.Count; i++)
                                    {
                                        if (node.Connections[j].end == node)
                                            node.Connections[j] = null;
                                    }
                                }
                                selectedNode = null;
                            }
                        }
                        else
                        {
                            // Switch Connection
                            if (closestNode != null && selectedNode != null && closestNode != selectedNode)
                            {
                                Connection connection = selectedNode.GetConnectionTo(closestNode);
                                if (connection != null)
                                    connection.SetActive(!connection.IsActive);
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
                            navigationGraph.graph.AddNode(mousePosition, true, Graph.PositionReference.WORLD);
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
                            to = navigationGraph.graph.AddNode(mousePosition, true, Graph.PositionReference.WORLD);
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
            Handles.DrawWireDisc(mousePosition, Vector3.forward, NodeEditorExtensions.nodeDrawSize);
            Handles.DrawWireDisc(mousePosition, Vector3.forward, autoSelectionRange);
            return mousePosition;
        }

        private Node GetAndDrawClosestNode(Vector2 mousePosition)
        {
            Node closestNode = navigationGraph.FindClosestNode(mousePosition, autoSelectionRange, NavigationExtensions.NodeType.ALL);
            if (closestNode != null)
            {
                closestNode.DrawNode(closestColor, navigationGraph.graph);
            }
            return closestNode;
        }

        private void DrawSelectedNode(Node closestNode, Vector2 mousePosition)
        {
            selectedNode.DrawNode(selectedColor, navigationGraph.graph);

            if (closestNode != null)
            {
                closestNode.DrawLineTo(selectedNode, selectedColor, navigationGraph.graph, 1);
                ConnectionEditorExtensions.DrawDistance(selectedNode, closestNode, selectedColor, navigationGraph.graph, 14);
            }
            else
            {
                selectedNode.DrawLineTo(mousePosition, addColor, navigationGraph.graph, 1);
                ConnectionEditorExtensions.DrawDistance(selectedNode, mousePosition, addColor, navigationGraph.graph, 14);
            }

            selectedNode.DrawPositionHandler(navigationGraph.graph);
        }
    }

    public static class NodeEditorExtensions
    {
        public const float nodeDrawSize = 0.05f;

        public static void DrawNode(this Node node, Color active, Color inactive, Graph reference = null) => node.DrawNode(node.IsActive ? active : inactive, reference);

        public static void DrawNode(this Node node, Color color, Graph reference = null)
        {
            Vector2 position = reference == null ? node.position : reference.GetWorldPosition(node);
            Handles.color = color;
            Handles.DrawSolidDisc(position, Vector3.forward, nodeDrawSize);
        }

        public static void DrawConnections(this Node node, Color active, Color inactive, Graph reference = null, int fontSize = 0)
        {
            foreach (Connection connection in node.Connections)
            {
                connection?.DrawConnection(active, inactive, reference, fontSize);
            }
        }

        public static void DrawConnections(this Node node, Color color, Graph reference = null, int fontSize = 0)
        {
            foreach (Connection connection in node.Connections)
            {
                connection?.DrawConnection(color, reference, fontSize);
            }
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
            Vector2 start = ConnectionEditorExtensions.GetWorldPosition(reference, source);
            DrawLineTo(start, target, color, screenSpaceSize);
        }

        public static void DrawLineTo(this Node source, Node target, Color color, Graph reference = null, float? screenSpaceSize = null)
        {
            Vector2[] positions = ConnectionEditorExtensions.GetWorldPosition(reference, source, target);
            Vector2 start = positions[0];
            Vector2 end = positions[1];

            DrawLineTo(start, end, color, screenSpaceSize);
        }

        public static Vector2 DrawPositionHandler(this Node source, Graph reference = null)
        {
            Vector2 position = ConnectionEditorExtensions.GetWorldPosition(reference, source);
            position = ConnectionEditorExtensions.GetLocalPosition(reference, Handles.PositionHandle(position, Quaternion.identity));
            source.position = position;
            return position;
        }
    }

    public static class ConnectionEditorExtensions
    {
        public const float arrowDrawSize = 0.05f;

        public static void DrawConnection(this Connection connection, Color active, Color inactive, Graph reference = null, int fontSize = 0) => connection.DrawConnection(connection.IsActive ? active : inactive, reference, fontSize);

        public static void DrawConnection(this Connection connection, Color color, Graph reference = null, int fontSize = 0)
        {
            Vector2[] positions = GetWorldPosition(reference, connection.start, connection.end);
            Vector2 start = positions[0];
            Vector2 end = positions[1];

            Handles.color = color;
            Handles.DrawLine(start, end);
            Vector2 half = (start + end) / 2;

            // Draw arrow
            Handles.DrawSolidArc(half, Vector3.forward, (start - end).normalized, 35, arrowDrawSize);
            Handles.DrawSolidArc(half, Vector3.forward, (start - end).normalized, -35, arrowDrawSize);
            if (fontSize > 0)
                DrawDistance(start, end, color, fontSize);
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

        public static void DrawDistance(this Node source, Node target, Color textColor, Graph reference = null, int fontSize = 10)
        {
            Vector2[] positions = GetWorldPosition(reference, source, target);
            DrawDistance(positions[0], positions[1], textColor, fontSize);
        }

        public static void DrawDistance(this Node source, Vector2 target, Color textColor, Graph reference = null, int fontSize = 10)
        {
            Vector2 start = GetWorldPosition(reference, source);
            DrawDistance(start, target, textColor, fontSize);
        }

        public static Vector2[] GetWorldPosition(Graph reference, params Node[] nodes)
        {
            return reference == null ? nodes.Select(e => e.position).ToArray() : nodes.Select(e => reference.GetWorldPosition(e)).ToArray();
        }

        public static Vector2 GetWorldPosition(Graph reference, Node node)
        {
            return reference == null ? node.position : reference.GetWorldPosition(node);
        }

        public static Vector2 GetLocalPosition(Graph reference, Vector2 position)
        {
            return reference == null ? position : reference.GetLocalPosition(position);
        }
    }
}