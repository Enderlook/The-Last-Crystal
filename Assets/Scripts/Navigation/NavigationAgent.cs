using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

public class NavigationAgent : MonoBehaviour
{
    [Tooltip("Navigation used to move.")]
    public Navigation navigation;

    public Node FindClosestNode() => navigation.FindClosestNode(transform.position);

    public List<Connection> FindPathTo(Node node) => navigation.DijkstraSearchPath(FindClosestNode(), node);

#if UNITY_EDITOR
    public bool drawPathToMouse = true;
    private void OnDrawGizmos()
    {
        if (drawPathToMouse)
        {
            foreach (Connection connection in FindPathTo(navigation.FindClosestNodeToMouse()))
            {
                connection.start.DrawNode(Color.blue);
                connection.end.DrawNode(Color.blue);
                connection.DrawConnection(Color.blue);
            }
        }
    }
#endif
}