using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEditorHelper;

using UnityEngine;

using Utils;

namespace Navigation
{
    [CustomEditor(typeof(NavigationGraph))]
    internal class NavigationGraphEditor : Editor
    {
        private NavigationGraph navigationGraph;
        private List<Node> Grid => navigationGraph.Grid;

        private bool showDrawingMenu = true;
        private bool drawNodes = true;
        private bool drawConnections = true;
        private bool drawDistances = true;

        private bool isEditingEnable = false;
        private bool wasEditingEnable = false;
        private bool wasLockedBefore = false;
        private float autoSelectionRange = 0.25f;
        private Node selectedNode;

        private static bool showColorConfigurationMenu = true;
        private Color addColor = Color.magenta;
        private Color selectedColor = Color.white;
        private Color closestColor = Color.black;

        private static bool showBasicColorConfigurationMenu = true;
        public static Color activeColor = Color.green;
        public static Color disabledColor = Color.red;
        public static Color extremeColor = Color.yellow;

        private static bool showHelp = true;

        private static bool showGridGenerationConfigurationMenu = false;

        private static GUIStyle BOLDED_FOLDOUT => new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold
        };

        public override void OnInspectorGUI()
        {
            this.DrawScriptField();

            serializedObject.Update();

            navigationGraph = (NavigationGraph)target;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("graph").FindPropertyRelative("reference"));

            if (navigationGraph.graph.Reference == null)
                navigationGraph.graph.Reference = navigationGraph.transform;

            EditorGUILayout.Space();

            ShowDrawingMenu();

            EditorGUILayout.Space();

            EditingMenu();

            EditorGUILayout.Space();

            ShowGridGenerationConfigurationMenu();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
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

                showBasicColorConfigurationMenu = EditorGUILayout.Foldout(showBasicColorConfigurationMenu, "Color Configuration", true, BOLDED_FOLDOUT);
                if (showBasicColorConfigurationMenu)
                {
                    activeColor = EditorGUILayout.ColorField("Active", activeColor);
                    disabledColor = EditorGUILayout.ColorField("Disabled", disabledColor);
                    extremeColor = EditorGUILayout.ColorField(new GUIContent("Extreme", "Color used to show extreme nodes and connections."), extremeColor);
                }
            }
        }

        private void EditingMenu()
        {
            isEditingEnable = EditorGUILayout.Foldout(isEditingEnable, new GUIContent("Editing Tool", "While open, enable editing tools and lock inspector window.\nTo unlock inspector this must be closed."), true, BOLDED_FOLDOUT);
            if (isEditingEnable)
            {
                // We activate the editing mode
                if (!wasEditingEnable)
                {
                    wasEditingEnable = true;
                    // Check if it was already locked or not
                    wasLockedBefore = ActiveEditorTracker.sharedTracker.isLocked;
                    // Lock inspector window so we don't lose focus of it when we click in the scene
                    ActiveEditorTracker.sharedTracker.isLocked = true;
                }

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
                    EditorGUILayout.HelpBox("L: Left Mouse Button"
                                  + "\nR: Right Mouse Button"
                                  + "\nS: Shift Key"
                                  + "\nC: Control Key"
                                  + "\nA: Alt Key", MessageType.Info);

                    EditorGUILayout.HelpBox(
                            "L: Select Closest / Create Node."
                          + "\nL+C: Enable / Disable Node."
                          + "\nL+S: Remove Selected Node."
                          + "\nL+A: Switch node to extreme or not."
                          + "\nR+S: Enable / Disable / Add Connection from Selected to Closest (Add)."
                          + "\nR+C: Enable / Disable / Add Connection from Closest to Selected (Add)."
                          + "\nR+C+S: Do [R+S] and [R+C]."
                          + "\nR+A: Remove connection from Selected to Closest."
                          + "\nR+A+C: Remove connection from Closest to Selected."
                        , MessageType.Info);
                }
            }
            else if (wasEditingEnable)
            {
                wasEditingEnable = false;
                // Return lock to before start editing
                ActiveEditorTracker.sharedTracker.isLocked = wasLockedBefore;
            }

            if (GUILayout.Button("Reset Grid"))
            {
                navigationGraph.ResetGrid();
                selectedNode = null;
            }

            if (GUILayout.Button("Remove Connections to nothing"))
                navigationGraph.RemoveConnectionsToNothing();

            if (GUILayout.Button("Add Missing Nodes from Connections"))
                navigationGraph.AddMissingNodesFromConnections();

            if (GUILayout.Button(new GUIContent("Remove Isolated Nodes", "Remove nodes which doesn't have connection to any other node or no node is connected to them.")))
                navigationGraph.RemoveNodesWithoutToOrFromConnection();

            if (GUILayout.Button("Become local to world"))
            {
                foreach (Node node in navigationGraph.graph.Grid)
                {
                    node.position = navigationGraph.graph.GetWorldPosition(node);
                }
                navigationGraph.graph.Reference.position = Vector3.zero;
            }

            if (GUILayout.Button("Become local to world and fix childs"))
            {
                Transform reference = navigationGraph.graph.Reference;
                Vector3 position = reference.position;
                foreach (Node node in navigationGraph.graph.Grid)
                {
                    node.position = navigationGraph.graph.GetWorldPosition(node);
                }
                reference.position = Vector3.zero;

                for (int i = 0; i < reference.childCount; i++)
                {
                    reference.GetChild(i).position += position;
                }
            }
        }

        private void DrawNodesAndConnections()
        {
            foreach (Node node in Grid)
            {
                if (drawNodes)
                    node.DrawNode(navigationGraph.graph);
                if (drawConnections)
                    node.DrawConnections(navigationGraph.graph, drawDistances ? 14 : 0);
            }
        }

        [MenuItem("CONTEXT/" + nameof(NavigationGraph) + "/Reset Grid")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "Usado por Unity")]
        private static void NewMenuOption(MenuCommand menuCommand) =>
            // https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5fa
            ((NavigationGraph)menuCommand.context).ResetGrid();

        private void EditingLogic()
        {
            Vector2 mousePosition = GetAndDrawMousePosition();

            Node closestNode = GetAndDrawClosestNode(mousePosition);

            if (selectedNode != null)
                DrawSelectedNode(closestNode, mousePosition);

            Event e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 1) // Right Button
                {
                    if (e.alt)
                    {
                        if (selectedNode != null && closestNode != null && selectedNode != closestNode)
                        {
                            selectedNode.TryRemoveConnectionTo(closestNode);
                            if (e.control)
                                closestNode.TryRemoveConnectionTo(selectedNode);
                        }
                    }
                    else
                    {
                        if (e.shift)
                            // Switch Connection
                            AlternateAddOrRemoveConnection(selectedNode, GetOrAddClosestNode(mousePosition));
                        if (e.control)
                            // Switch Inverse Connection
                            AlternateAddOrRemoveConnection(GetOrAddClosestNode(mousePosition), selectedNode);
                    }
                }
                else if (e.button == 0) // Left Button
                {
                    if (e.control)
                    {
                        // Switch Node
                        if (closestNode != null)
                            closestNode.SetActive(!closestNode.IsActive);
                    }
                    else if (e.shift)
                    {
                        // Remove Node
                        if (closestNode != null)
                        {
                            navigationGraph.graph.RemoveNodeAndConnections(closestNode);
                            if (selectedNode == closestNode)
                                selectedNode = null;
                            closestNode = null;
                        }
                    }
                    else if (e.alt)
                    {
                        if (closestNode != null)
                            closestNode.isExtreme = !closestNode.isExtreme;
                    }
                    else if (closestNode == null)
                        // Add Node
                        navigationGraph.graph.AddNode(mousePosition, true, PositionReference.WORLD);
                    else
                        // Select Closest Node
                        selectedNode = closestNode;
                }
                else if (e.alt)
                    // Select Closest Node
                    selectedNode = closestNode;
            }
        }

        private static void AlternateAddOrRemoveConnection(Node from, Node to)
        {
            if (from == to || from == null || to == null)
                return;

            if (from.TryGetConnectionTo(to, out Connection connection))
            {
                // Switch Connection
                if (connection.IsActive)
                    connection.SetActive(false);
                else
                    from.RemoveConnection(connection);
            }
            else
                // Add connection
                from.AddConnectionTo(to, true);
        }

        private Vector2 GetAndDrawMousePosition()
        {
            Vector2 mousePosition = MouseHelper.GetMousePositionInEditor();
            Handles.color = addColor;
            Handles.DrawWireDisc(mousePosition, Vector3.forward, NodeEditorExtensions.nodeDrawSize);
            Handles.DrawWireDisc(mousePosition, Vector3.forward, autoSelectionRange);
            return mousePosition;
        }

        private Node GetAndDrawClosestNode(Vector2 mousePosition)
        {
            Node closestNode = navigationGraph.FindClosestNode(mousePosition, autoSelectionRange, NavigationExtensions.NodeType.ALL);
            if (closestNode != null)
                closestNode.DrawNode(closestColor, navigationGraph.graph);
            return closestNode;
        }

        private Node GetOrAddClosestNode(Vector2 mousePosition)
        {
            Node closestNode = navigationGraph.FindClosestNode(mousePosition, autoSelectionRange, NavigationExtensions.NodeType.ALL);
            if (closestNode == null)
                closestNode = navigationGraph.graph.AddNode(mousePosition, true, PositionReference.WORLD);
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
            {
                if (connection != null) // Why this?
                    connection.DrawConnection(active, inactive, reference, fontSize);
            }
        }

        public static void DrawConnections(this Node node, Color color, Graph reference = null, int fontSize = 0)
        {
            foreach (Connection connection in node.Connections)
            {
                if (connection != null) // Why this?
                    connection.DrawConnection(color, reference, fontSize);
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
                DrawDistance(start, end, color, fontSize);

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
            DrawDistance(positions[0], positions[1], textColor, fontSize);
        }

        public static void DrawDistance(this Node source, Vector2 target, Color textColor, Graph reference = null, int fontSize = 10)
        {
            Vector2 start = reference.GetWorldPosition(source);
            DrawDistance(start, target, textColor, fontSize);
        }

        public static Vector2[] GetWorldPosition(this Graph reference, params Node[] nodes) => reference == null ? nodes.Select(e => e.position).ToArray() : nodes.Select(e => reference.GetWorldPosition(e)).ToArray();
    }
}