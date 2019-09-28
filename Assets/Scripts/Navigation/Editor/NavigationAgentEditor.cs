using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Navigation.UnityInspector
{
    [CustomEditor(typeof(NavigationAgent))]
    public class NavigationAgentEditor : Editor
    {
        private NavigationAgent navigationAgent;

        private bool drawPathToMouse = false;

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
                List<Connection> path = navigationAgent.FindPathTo(navigationAgent.navigationGraph.FindClosestNodeToMouseInEditor());
                if (path == null)
                    return;
                foreach (Connection connection in path)
                {
                    connection.start.DrawNode(Color.blue, navigationAgent.navigationGraph.graph);
                    connection.end.DrawNode(Color.blue, navigationAgent.navigationGraph.graph);
                    connection.DrawConnection(Color.blue, navigationAgent.navigationGraph.graph);
                }
            }
        }
    }
}