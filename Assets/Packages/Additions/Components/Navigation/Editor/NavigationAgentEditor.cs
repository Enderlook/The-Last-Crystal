using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace Additions.Components.Navigation
{
    [CustomEditor(typeof(NavigationAgent))]
    internal class NavigationAgentEditor : Editor
    {
        private NavigationAgent navigationAgent;

        private bool drawPathToMouse;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            navigationAgent = (NavigationAgent)target;

            drawPathToMouse = EditorGUILayout.Toggle("Draw Path To Mouse", drawPathToMouse);
        }

        public void OnSceneGUI()
        {
            if (drawPathToMouse)
            {
                List<Connection> path = navigationAgent.FindPathTo(navigationAgent.NavigationGraph.FindClosestNodeToMouseInEditor());
                if (path == null)
                    return;
                foreach (Connection connection in path)
                {
                    connection.start.DrawNode(Color.blue, navigationAgent.NavigationGraph.Graph);
                    connection.end.DrawNode(Color.blue, navigationAgent.NavigationGraph.Graph);
                    connection.DrawConnection(Color.blue, navigationAgent.NavigationGraph.Graph);
                }
            }
        }
    }
}