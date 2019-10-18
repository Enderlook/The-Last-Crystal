using System;
using System.Collections.Generic;
using CreaturesAddons;
using UnityEngine;

namespace Navigation
{
    [Serializable]
    public class NavigationAgent : MonoBehaviour, IInit
    {
        [Tooltip("Navigation Graph used to find paths.")]
        private NavigationGraph navigationGraph;
        public NavigationGraph NavigationGraph
        {
            get => navigationGraph;
            set => navigationGraph = value;
        }

        private Transform thisTransform;

        void IInit.Init(Creature creature)
        {
            thisTransform = creature.thisRigidbody2D.transform;
            navigationGraph = GameObject.FindGameObjectWithTag("Ground").GetComponent<NavigationGraph>();
        }

        public Node FindClosestNode() => NavigationGraph.FindClosestNode(thisTransform.position);

        public List<Connection> FindPathTo(Node node)
        {
#if !UNITY_EDITOR
            List<Connection>
#endif
            lastPath = NavigationGraph.AStarSearchPath(FindClosestNode(), node);
            return lastPath;
        }

        public List<Connection> FindPathTo(Vector2 target) => FindPathTo(NavigationGraph.FindClosestNode(target));

#if UNITY_EDITOR
        [SerializeField, Tooltip("Draw last path calculated on play.")]
        private bool drawPath = false;
        [SerializeField, Tooltip("Color of path.")]
        private Color pathColor = Color.blue;

        private List<Connection> lastPath;

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