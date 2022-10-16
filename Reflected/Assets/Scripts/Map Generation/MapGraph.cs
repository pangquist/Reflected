using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGraph : MonoBehaviour
{
    public class Node
    {
        public Room room;
        public List<Node> adjacentNodes = new List<Node>();
        public List<Edge> adjacentEdges = new List<Edge>();
        public bool marked;

        public Node(Room room)
        {
            this.room = room;
        }
    }

    public class Edge
    {
        public Chamber chamber;
        public List<Node> adjacentNodes = new List<Node>();
        public List<Edge> adjacentEdges = new List<Edge>();
        public bool marked;

        public Edge(Chamber chamber)
        {
            this.chamber = chamber;
        }
    }

    [Header("References")]
    [SerializeField] private Map map;

    [Header("Read Only")]
    [ReadOnly][SerializeField] private List<Node> nodes;
    [ReadOnly][SerializeField] private List<Edge> edges;

    public void Generate()
    {
        nodes = new List<Node>();
        edges = new List<Edge>();

        nodes.Add(new Node(map.Rooms[0]));

        RecursiveBuild(nodes[0]);
        ClearMarked();

        foreach (Edge edge1 in edges)
        {
            foreach (Node node in edge1.adjacentNodes)
            {
                foreach (Edge edge2 in node.adjacentEdges)
                {
                    if (edge1 != edge2 && !edge1.adjacentEdges.Contains(edge2))
                        edge1.adjacentEdges.Add(edge2);
                }
            }
        }

        int traversedNodes = TraverseGraph();
        Debug.Log(traversedNodes + "/" + map.Rooms.Count);
    }

    private void RecursiveBuild(Node node)
    {
        if (node.marked)
            return;
        
        node.marked = true;

        foreach (Chamber chamber in node.room.Chambers)
        {
            if (node.adjacentEdges.Find(edge => edge.chamber == chamber) != null)
                continue;

            Edge edge = new Edge(chamber);
            edge.adjacentNodes.Add(node);
            node.adjacentEdges.Add(edge);

            edges.Add(edge);
            RecursiveBuild(edge);
        }
    }

    private void RecursiveBuild(Edge edge)
    {
        if (edge.marked)
            return;

        edge.marked = true;

        Room room;

        if (edge.chamber.Room1 != edge.adjacentNodes[0].room)
            room = edge.chamber.Room1;
        else
            room = edge.chamber.Room2;

        Node node = nodes.Find(node => node.room == room);

        if (node == null)
            nodes.Add(node = new Node(room));

        edge.adjacentNodes.Add(node);
        node.adjacentEdges.Add(edge);
        edge.adjacentNodes[0].adjacentNodes.Add(edge.adjacentNodes[1]);
        edge.adjacentNodes[1].adjacentNodes.Add(edge.adjacentNodes[0]);

        RecursiveBuild(node);
    }

    private int TraverseGraph()
    {
        int markedNodes = TraverseNodeToNode(nodes[0]);
        ClearMarked();
        return markedNodes;
    }

    private int TraverseNodeToNode(Node node)
    {
        node.marked = true;
        int markedNodes = 1;

        foreach (Node adjacentNode in node.adjacentNodes)
            if (!adjacentNode.marked)
                markedNodes += TraverseNodeToNode(adjacentNode);

        return markedNodes;
    }

    private void ClearMarked()
    {
        foreach (Node node in nodes)
            node.marked = false;

        foreach (Edge edge in edges)
            edge.marked = false;
    }

}
