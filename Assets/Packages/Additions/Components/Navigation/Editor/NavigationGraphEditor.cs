using Additions.Utils;
using Additions.Utils.UnityEditor;

using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace Additions.Components.Navigation
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

        private bool isEditingEnable;
        private bool wasEditingEnable;
        private bool wasLockedBefore;
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

        private static bool showQuickActionsMenu;
        private static bool showRestructureMenu;
        private static bool showLocalToWorldMenu;
        private static bool showToggleMenu;

        private static bool showHelp = true;

        private static bool showGridGenerationConfigurationMenu;

        private static GUIStyle BOLDED_FOLDOUT => new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold
        };

        public override void OnInspectorGUI()
        {
            this.DrawScriptField();

            serializedObject.Update();

            navigationGraph = (NavigationGraph)target;

            EditorGUILayout.PropertyField(serializedObject.FindBackingFieldOfProperty("Graph").FindRelativeBackingFieldOfProperty("Reference"));

            if (navigationGraph.Graph.Reference == null)
                navigationGraph.Graph.Reference = navigationGraph.transform;

            EditorGUILayout.Space();

            ShowDrawingMenu();

            EditorGUILayout.Space();

            ShowEditingMenu();

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
                    serializedObject.ApplyModifiedProperties();
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

        private void ShowEditingMenu()
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

                ShowQuickActions();
            }
            else if (wasEditingEnable)
            {
                wasEditingEnable = false;
                // Return lock to before start editing
                ActiveEditorTracker.sharedTracker.isLocked = wasLockedBefore;
            }
        }

        private void ShowQuickActions()
        {
            if (showQuickActionsMenu = EditorGUILayout.Foldout(showQuickActionsMenu, "Quick Actions", true, BOLDED_FOLDOUT))
            {
                EditorGUI.indentLevel++;
                if (showRestructureMenu = EditorGUILayout.Foldout(showRestructureMenu, "Restructure", true))
                {
                    if (GUILayout.Button("Remove Connections to nothing"))
                        navigationGraph.RemoveConnectionsToNothing();

                    if (GUILayout.Button("Add Missing Nodes from Connections"))
                        navigationGraph.AddMissingNodesFromConnections();

                    if (GUILayout.Button(new GUIContent("Remove Isolated Nodes", "Remove nodes which doesn't have connection to any other node or no node is connected to them.")))
                        navigationGraph.RemoveNodesWithoutToOrFromConnection();
                }

                if (showLocalToWorldMenu = EditorGUILayout.Foldout(showLocalToWorldMenu, "Local to World", true))
                {
                    if (GUILayout.Button("Become local to world"))
                    {
                        foreach (Node node in navigationGraph.Graph.Grid)
                            node.position = navigationGraph.Graph.GetWorldPosition(node);
                        navigationGraph.Graph.Reference.position = Vector3.zero;
                    }

                    if (GUILayout.Button("Become local to world and fix childs"))
                    {
                        Transform reference = navigationGraph.Graph.Reference;
                        Vector3 position = reference.position;
                        foreach (Node node in navigationGraph.Graph.Grid)
                            node.position = navigationGraph.Graph.GetWorldPosition(node);
                        reference.position = Vector3.zero;

                        for (int i = 0; i < reference.childCount; i++)
                            reference.GetChild(i).position += position;
                    }
                }

                if (showToggleMenu = EditorGUILayout.Foldout(showToggleMenu, "Toggle", true))
                {
                    if (GUILayout.Button("Disable all nodes"))
                    {
                        navigationGraph.ToggleAllNodes(ToggleMode.Disable);
                    }

                    if (GUILayout.Button("Enable all nodes"))
                    {
                        navigationGraph.ToggleAllNodes(ToggleMode.Enable);
                    }

                    if (GUILayout.Button(new GUIContent("Toggle all nodes", "Disable active nodes and active disabled nodes.")))
                    {
                        navigationGraph.ToggleAllNodes(ToggleMode.Toggle);
                    }

                    if (GUILayout.Button("Disable all connections"))
                    {
                        navigationGraph.ToggleAllConnections(ToggleMode.Disable);
                    }

                    if (GUILayout.Button("Enable all connections"))
                    {
                        navigationGraph.ToggleAllConnections(ToggleMode.Enable);
                    }

                    if (GUILayout.Button(new GUIContent("Toggle all connections", "Disable connections nodes and active disabled connections.")))
                    {
                        navigationGraph.ToggleAllConnections(ToggleMode.Toggle);
                    }
                }

                if (GUILayout.Button("Reset Grid"))
                {
                    navigationGraph.ResetGrid();
                    selectedNode = null;
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawNodesAndConnections()
        {
            foreach (Node node in Grid)
            {
                if (drawNodes)
                    node.DrawNode(navigationGraph.Graph);
                if (drawConnections)
                    node.DrawConnections(navigationGraph.Graph, drawDistances ? 14 : 0);
            }
        }

        [MenuItem("CONTEXT/" + nameof(NavigationGraph) + "/Reset Grid")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
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
                            navigationGraph.Graph.RemoveNodeAndConnections(closestNode);
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
                        navigationGraph.Graph.AddNode(mousePosition, true, PositionReference.WORLD);
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
                closestNode.DrawNode(closestColor, navigationGraph.Graph);
            return closestNode;
        }

        private Node GetOrAddClosestNode(Vector2 mousePosition)
        {
            Node closestNode = navigationGraph.FindClosestNode(mousePosition, autoSelectionRange, NavigationExtensions.NodeType.ALL);
            if (closestNode == null)
                closestNode = navigationGraph.Graph.AddNode(mousePosition, true, PositionReference.WORLD);
            return closestNode;
        }

        private void DrawSelectedNode(Node closestNode, Vector2 mousePosition)
        {
            selectedNode.DrawNode(selectedColor, navigationGraph.Graph);

            if (closestNode != null)
            {
                closestNode.DrawLineTo(selectedNode, selectedColor, navigationGraph.Graph, 1);
                selectedNode.DrawDistance(closestNode, selectedColor, navigationGraph.Graph, 14);
            }
            else
            {
                selectedNode.DrawLineTo(mousePosition, addColor, navigationGraph.Graph, 1);
                selectedNode.DrawDistance(mousePosition, addColor, navigationGraph.Graph, 14);
            }

            selectedNode.DrawPositionHandler(navigationGraph.Graph);
        }
    }
}