﻿using UnityEditor;
using UnityEngine;

public static class NavigationExtensions
{
    public static Node FindClosestNode(this Navigation navigation, Vector2 position)
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

    public static Node FindClosestNodeToMouse(this Navigation navigation)
    {
        /* Draw closest node to mouse
         * https://answers.unity.com/questions/1321651/i-need-to-get-a-vector2-of-the-mouse-position-whil.html
         * http://answers.unity.com/answers/1323496/view.html */
        Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).GetPoint(1);
        return navigation.FindClosestNode(mousePosition);
    }
}