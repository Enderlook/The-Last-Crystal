using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class NavigationAgent : MonoBehaviour
{
    [Tooltip("Navigation used to move.")]
    public Navigation navigation;

    private static List<Node> DepthFirstSearch(Node start)
    {
        // https://www.koderdojo.com/blog/depth-first-search-algorithm-in-csharp-and-net-core
        List<Node> visited = new List<Node>();

        Stack<Node> toVisit = new Stack<Node>();
        toVisit.Push(start);

        while (toVisit.Count > 0)
        {
            Node node = toVisit.Pop();

            visited.Add(node);

            foreach (Connection connection in node.connections)
            {
                if (connection != null && !visited.Contains(connection.end))
                {
                    if (!toVisit.Contains(connection.end))
                        toVisit.Push(connection.end);
                }
            }
        }
        return visited;
    }

    private Node FindClosestNode(Vector2 position)
    {
        Node closestNode = null;
        float closest = float.MaxValue;

        foreach (Node node in navigation.Grid)
        {
            float distance = (node.position - position).sqrMagnitude;
            if (distance < closest)
            {
                closest = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawClosestNodeToMouse();
        /*foreach (Node node in DepthFirstSearch(FindClosestNode(transform.position)))
        {
            node.DrawNode(Color.red);
        }*/
        List<Node> nodes = DepthFirstSearch(FindClosestNode(transform.position));
        for (int i = 0; i < nodes.Count - 1;)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(nodes[i].position, nodes[++i].position);
        }
    }

    private void DrawClosestNodeToMouse()
    {
        /* Draw closest node to mouse
         * https://answers.unity.com/questions/1321651/i-need-to-get-a-vector2-of-the-mouse-position-whil.html
         * http://answers.unity.com/answers/1323496/view.html */
        Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).GetPoint(1);
        Node closestNode = FindClosestNode(mousePosition);
        closestNode.DrawNode(Color.blue);
    }
#endif

}
