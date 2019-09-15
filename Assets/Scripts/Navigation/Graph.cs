using System.Collections.Generic;
using Navigation;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class Connections : SerializableList<Connection>
{
    public Connections(int capacity) : base(capacity) { }

    public Connections(List<Connection> list) : base(list) { }
}

[Serializable]
public class NodeConnections : SerializableDictionary<Vector2, Connections> { }

[Serializable]
public class Graph : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<Node> grid;
    public List<Node> Grid {
        get {
            if (grid == null)
                grid = new List<Node>();
            return grid;
        }
        set => grid = value;
    }

    [SerializeField]
    private NodeConnections connections;

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        RemoveDuplicatedPositionsFromGrid();
        connections = new NodeConnections();
        foreach (Node node in Grid)
        {
            connections.Add(node.position, new Connections(node.Connections));
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Dictionary<Vector2, Node> nodesByPosition = grid.ToDictionary(e => e.position);

        foreach (Node node in Grid)
        {
            if (connections.TryGetValue(node.position, out Connections nodeConnections))
            {
                node.Connections = nodeConnections.GetList();
                foreach (Connection connection in node.Connections)
                {
                    connection.Deserialize(nodesByPosition);
                }
            }
        }
        connections = null;
    }

    public void RemoveDuplicatedPositionsFromGrid()
    {
        HashSet<Vector2> usedPositions = new HashSet<Vector2>();
        for (int i = Grid.Count - 1; i >= 0; i--)
        {
            if (usedPositions.Contains(Grid[i].position))
                Grid.RemoveAt(i);
            else
                usedPositions.Add(Grid[i].position);
        }
    }
}
