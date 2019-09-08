using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Navigation : MonoBehaviour
{
    /* Based on:
     * http://www.jgallant.com/nodal-pathfinding-in-unity-2d-with-a-in-non-grid-based-games/
     * https://github.com/7ark/Unity-Pathfinding/blob/master/AINavMeshGenerator.cs
     */
    private enum Directions { RIGHT, DOWN_RIGHT, DOWN, DOWN_LEFT, LEFT, UP_LEFT, UP, UP_RIGHT }
    [Tooltip("Initial position of the grid.")]
    public Transform startPoint;
    [Tooltip("Distance between each node.")]
    public float spacePerNode;
    [Tooltip("Amount of rows.")]
    public int rows;
    [Tooltip("Amount of columns.")]
    public int columns;

    private List<Node> grid;
    private List<Node> Grid {
        get {
            if (grid == null)
                FillGrid();
            return grid;
        }
    }

    private void FillGrid()
    {
        int nodeAmount = rows * columns;
        grid = new List<Node>(nodeAmount);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                float width = c * spacePerNode * 2;
                if (r % 2 == 0) // Add diamond shape ♦
                    width += spacePerNode;
                Vector2 position = (Vector2)startPoint.position + new Vector2(width, r * spacePerNode); // Not * 2 as column in order to make diamond shape ♦
                Node node = new Node(position);
                grid.Add(node);
            }
        }
        AddConnectionsToNodes();
    }

    private void AddConnectionsToNodes()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            for (int direction = 0; direction < grid[i].connections.Length; direction++)
            {
                grid[i].connections[direction] = GetNodeFromDirection(i, (Directions)direction);
            }
        }
    }

    private Node GetNodeFromDirection(int nodeIndex, Directions direction)
    {
        int index = -1;
        bool isStartOfRow = nodeIndex % columns == 0;
        bool isEndOfRow = (nodeIndex + 1) % columns == 0;
        bool isOddRow = (nodeIndex / columns) % 2 == 0; // Due to diamond shape ♦

        switch (direction)
        {
            case Directions.RIGHT:
                if (isEndOfRow) return null;
                index = nodeIndex + 1;
                break;
            case Directions.DOWN_RIGHT:
                if (isEndOfRow && isOddRow) return null;
                index = nodeIndex - columns + (isOddRow ? 1 : 0);
                break;
            case Directions.DOWN:
                index = nodeIndex - columns * 2;
                break;
            case Directions.DOWN_LEFT:
                if (isStartOfRow && !isOddRow) return null;
                index = nodeIndex - columns - (isOddRow ? 0 : 1);
                break;
            case Directions.LEFT:
                if (isStartOfRow) return null;
                index = nodeIndex - 1;
                break;
            case Directions.UP_LEFT:
                if (isStartOfRow && !isOddRow) return null;
                index = nodeIndex + columns - (isOddRow ? 0 : 1);
                break;
            case Directions.UP:
                index = nodeIndex + columns * 2;
                break;
            case Directions.UP_RIGHT:
                if (isEndOfRow && isOddRow) return null;
                index = nodeIndex + columns + (isOddRow ? 1 : 0);
                break;
        }

        return index >= 0 && index < grid.Count ? grid[index] : null;
    }

#if UNITY_EDITOR
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "Usado por Unity.")]
    private void OnDrawGizmos()
    {
        foreach (Node node in Grid)
        {
            node.DrawNode();
            node.DrawConnections();
        }
    }
#endif
}

public class Node
{
    public Vector2 position;
    public Node[] connections;

    public Node(Vector2 position)
    {
        this.position = position;
        connections = new Node[8];
    }

#if UNITY_EDITOR
    public void DrawNode()
    {
        Handles.color = Color.yellow;
        Handles.DrawSolidDisc(position, Vector3.forward, 0.05f);
    }
    public void DrawConnections()
    {
        foreach (Node connection in connections)
        {
            if (connection != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(position, connection.position);
            }
        }
    }
#endif
}
