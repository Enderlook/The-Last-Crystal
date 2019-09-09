using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NavigationAgent : MonoBehaviour
{
    [Tooltip("Navigation used to move.")]
    public Navigation navigation;

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