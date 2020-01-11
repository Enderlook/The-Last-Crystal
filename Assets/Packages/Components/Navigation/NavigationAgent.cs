using CreaturesAddons;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Navigation
{
    [Serializable]
    public class NavigationAgent : MonoBehaviour, IInit
    {
        [Header("Setup")]
        [SerializeField, Tooltip("Navigation Graph used to find paths.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2235:Mark all non-serializable fields", Justification = "Serialized by Unity")]
        private NavigationGraph navigationGraph;
        public NavigationGraph NavigationGraph {
            get => navigationGraph;
            set => navigationGraph = value;
        }

        [NonSerialized]
        private Transform thisTransform;

        void IInit.Init(Creature creature) => thisTransform = creature.thisRigidbody2D.transform;

        public Node FindClosestNode() => NavigationGraph.FindClosestNode(thisTransform.position);

        public List<Connection> FindPathTo(Node node)
        {
#if !UNITY_EDITOR
            List<Connection> lastPath;
#endif
            lastPath = NavigationGraph.SearchPath(FindClosestNode(), node);
            return lastPath;
        }

        public List<Connection> FindPathTo(Vector2 target) => FindPathTo(NavigationGraph.FindClosestNode(target));

        public List<Connection> FindPathTo(Transform target) => FindPathTo(target.position);

        public List<Connection> FindPathTo(Node node, out float distance)
        {
            distance = NavigationGraph.SearchPath(FindClosestNode(), out List<Connection> path, node);
#if UNITY_EDITOR
            lastPath = path;
#endif
            return path;
        }

        public List<Connection> FindPathTo(Vector2 target, out float distance) => FindPathTo(NavigationGraph.FindClosestNode(target), out distance);

        public List<Connection> FindPathTo(Transform target, out float distance) => FindPathTo(NavigationGraph.FindClosestNode(target.position), out distance);

        public float FindDistanceTo(Node node) => NavigationGraph.CalculatePathDistance(FindClosestNode(), node);

        public float FindDistanceTo(Vector2 target) => FindDistanceTo(NavigationGraph.FindClosestNode(target));

        public float FindDistanceTo(Transform target) => FindDistanceTo(target.position);

        public static void InjectNavigationGraph(GameObject gameObject, NavigationGraph navigationGraph)
        {
            NavigationAgent navigationAgent = gameObject.GetComponent<NavigationAgent>();
            if (navigationAgent != null)
                navigationAgent.NavigationGraph = navigationGraph;
        }

#if UNITY_EDITOR
#pragma warning disable CS0649
        [Header("Editor Only")]
        [SerializeField, Tooltip("Draw last path calculated on play.")]
        private bool drawPath;

        [SerializeField, Tooltip("Color of path.")]
        private Color pathColor = Color.blue;

        [NonSerialized]
        private List<Connection> lastPath;
#pragma warning restore CS0649

        private void OnDrawGizmos()
        {
            if (drawPath && lastPath != null)
            {
                Gizmos.color = pathColor;
                foreach (Connection connection in lastPath)
                {
                    Gizmos.DrawLine(connection.start.position, connection.end.position);
                }
            }
        }
#endif
    }
}