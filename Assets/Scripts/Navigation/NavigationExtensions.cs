using UnityEditor;
using UnityEngine;

namespace Navigation
{
    public static class NavigationExtensions
    {
        public enum NodeType { ONLY_ACTIVES, ONLY_DEACTIVES, ALL }
        public static Node FindClosestNode(this NavigationGraph navigation, Vector2 position, float maxDistanceFromPoint = 0, NodeType mode = NodeType.ONLY_ACTIVES)
        {
            Node closestNode = null;
            float closest = float.MaxValue;

            foreach (Node node in navigation.Grid)
            {
                if (mode == NodeType.ALL || (mode == NodeType.ONLY_ACTIVES && node.IsActive) || (mode == NodeType.ONLY_DEACTIVES && !node.IsActive))
                {
                    float distance = (node.position - position).magnitude;
                    if (distance < closest && (maxDistanceFromPoint == 0 || distance < maxDistanceFromPoint))
                    {
                        closest = distance;
                        closestNode = node;
                    }
                }
            }

            return closestNode;
        }

        public static Node FindClosestNodeToMouse(this NavigationGraph navigation)
        {
            return navigation.FindClosestNode(GetMousePosition());
        }

        public static Vector2 GetMousePosition()
        {
            /* Draw closest node to mouse
             * https://answers.unity.com/questions/1321651/i-need-to-get-a-vector2-of-the-mouse-position-whil.html
             * http://answers.unity.com/answers/1323496/view.html */
            return HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).GetPoint(1);
        }
    }
}