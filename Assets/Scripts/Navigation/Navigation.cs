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
    [Tooltip("Layer used to check for collisions.\nIf a collision is found with a node, the node is destroyed.")]
    public LayerMask destroyMask;

    private List<Node> grid;
    public List<Node> Grid {
        get {
            if (grid == null)
                GenerateGrid();
            return grid;
        }
    }

    public void GenerateGrid()
    {
        FillGrid();
        DestroyBadNodes();
        DestroyBadConnections();
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
            Node node = grid[i];
            for (int direction = 0; direction < node.connections.Length; direction++)
            {
                Node nodeToConnect = GetNodeFromDirection(i, (Directions)direction);
                if (nodeToConnect != null)
                    node.connections[direction] = new Connection(node, nodeToConnect);
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

    private void DestroyBadNodes()
    {
        for (int i = Grid.Count - 1; i >= 0; i--)
        {
            Node node = Grid[i];
            // Check if it's overlapping a collider
            Collider2D hit = Physics2D.OverlapCircle(node.position, .1f, destroyMask);
            if (hit != null)
            {
                // Destroy this node by setting it to null in all its references.
                // Iterating over each node which has it has a connection.
                for (int j = 0; j < node.connections.Length; j++)
                {
                    Connection connection = node.connections[j];
                    if (connection != null)
                    {
                        // Look for this node in all the connections and set it to null
                        for (int k = 0; k < connection.end.connections.Length; k++)
                        {
                            if(connection.end.connections[k] != null && connection.end.connections[k].start == node)
                            {
                                connection.end.connections[k] = null;
                                break;
                            }
                        }
                    }
                }
                Grid.RemoveAt(i);
            }
        }
    }

    private void DestroyBadConnections()
    {
        foreach (Node node in Grid)
        {
            for (int i = 0; i < node.connections.Length; i++)
            {
                Connection connection = node.connections[i];

                if (connection != null)
                {
                    if (Physics2D.Linecast(connection.start.position, connection.end.position, destroyMask))
                    {
                        // Destroy this connection
                        node.connections[i] = null;
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Calidad del código", "IDE0051:Quitar miembros privados no utilizados", Justification = "Usado por Unity.")]
    private void OnDrawGizmos()
    {
        foreach (Node node in Grid)
        {
            node.DrawNode(Color.yellow);
            node.DrawConnections(Color.yellow);
        }
    }
#endif
}

public class Node
{
    public Vector2 position;
    public Connection[] connections;

    public Node(Vector2 position)
    {
        this.position = position;
        connections = new Connection[8];
    }

#if UNITY_EDITOR
    public void DrawNode(Color color)
    {
        Handles.color = color;
        Handles.DrawSolidDisc(position, Vector3.forward, 0.05f);
    }
    public void DrawConnections(Color color)
    {
        foreach(Connection connection in connections)
        {
            connection?.DrawConnection(color);
        }
    }
#endif
}

public class Connection
{
    public Node start;
    public Node end;

    public Connection(Node start, Node end)
    {
        this.start = start;
        this.end = end;
    }

#if UNITY_EDITOR
    public void DrawConnection(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(start.position, end.position);
    }
#endif
}