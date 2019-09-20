using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class NavigationAgent : MonoBehaviour
    {
        [Tooltip("Navigation used to move.")]
        public NavigationGraph navigationGraph;

        public Node FindClosestNode() => navigationGraph.FindClosestNode(transform.position);

        public List<Connection> FindPathTo(Node node) => navigationGraph.AStarSearchPath(FindClosestNode(), node);        
    }
}