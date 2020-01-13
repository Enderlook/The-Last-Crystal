using Additions.Utils;

using UnityEngine;

namespace Additions.Components.Navigation
{
    public static class NavigationExtensions
    {
        public enum NodeType { ONLY_ACTIVES, ONLY_DEACTIVES, ALL }

        public static Node FindClosestNode(this NavigationGraph navigation, Vector2 position, float maxDistanceFromPoint = 0, NodeType mode = NodeType.ONLY_ACTIVES)
        {
            Node closestNode = null;
            float closest = float.MaxValue;

            foreach (Node node in navigation.Grid)
                if (mode == NodeType.ALL || mode == NodeType.ONLY_ACTIVES && node.IsActive || mode == NodeType.ONLY_DEACTIVES && !node.IsActive)
                {
                    float distance = (navigation.Graph.GetWorldPosition(node) - position).magnitude;
                    if (distance < closest && (maxDistanceFromPoint == 0 || distance < maxDistanceFromPoint))
                    {
                        closest = distance;
                        closestNode = node;
                    }
                }

            return closestNode;
        }
#if UNITY_EDITOR
        public static Node FindClosestNodeToMouseInEditor(this NavigationGraph navigation) => navigation.FindClosestNode(MouseHelper.GetMouseWorldPositionInEditor());
#endif

        public static Node FindClosestNodeToMouseInGame(this NavigationGraph navigation) => navigation.FindClosestNode(MouseHelper.GetMouseWorldPositionInGame());

        public static Node FindClosestNodeToMouseInGame(this NavigationGraph navigation, Camera camera) => navigation.FindClosestNode(MouseHelper.GetMouseWorldPositionInGame(camera));
    }
}