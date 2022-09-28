using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChamberGenerator : MonoBehaviour
{
    private class Node
    {
        public Room room;
        public List<Node> adjacentNodes = new List<Node>();
        public bool marked;

        public Node(Room room)
        {
            this.room = room;
        }
    }

    private class Path
    {
        public int index;
        public Node node1, node2;
        public RectInt overlap;
        public Orientation orientation;

        public Path(int index, Node node1, Node node2, RectInt overlap, Orientation orientation)
        {
            this.index = index;
            this.node1 = node1;
            this.node2 = node2;
            this.overlap = overlap;
            this.orientation = orientation;
        }
    }

    [Header("Warnings")]

    [SerializeField] private bool logWarnigns;

    [Header("References")]

    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject chamberPrefab;

    [Header("Chambers")]

    [Range(1, 20)]
    [SerializeField] private int chamberSize;

    [Range(0, 20)]
    [SerializeField] private int chamberMargin;

    [Tooltip("Around 55-60% of chambers are necessary.")]
    [SerializeField] private bool addRandomChambers;

    [Range(0f, 1f)]
    [SerializeField] private float minChambers;

    [Range(0f, 1f)]
    [SerializeField] private float maxChambers;

    private List<Node> nodes = new List<Node>();
    private List<Path> paths = new List<Path>();
    private List<Path> removedPaths = new List<Path>();

    public void Generate(Map map)
    {
        Chamber.StaticInitialize(map);

        FindPaths(map);
        int maxPaths = paths.Count;

        DisconnectNodes();
        mapGenerator.Log("Minimum chambers: " + paths.Count + "/" + maxPaths + " (" + (100f * paths.Count / maxPaths).ToString("0") + "%)");

        ReconnectNodes();
        mapGenerator.Log("Final chambers: " + paths.Count + "/" + maxPaths + " (" + (100f * paths.Count / maxPaths).ToString("0") + "%)");

        InstantiateChambers(map);

        nodes.Clear();
        paths.Clear();
        removedPaths.Clear();
    }

    private void FindPaths(Map map)
    {
        foreach (Room room in map.Rooms)
            nodes.Add(new Node(room));

        RectInt horizontal1, horizontal2, vertical1, vertical2, overlap;

        bool GetRects(Room room, out RectInt horizontal, out RectInt vertical)
        {
            horizontal = room.Rect.Inflated(mapGenerator.RoomGenerator.RoomPadding * 2, -chamberMargin);
            vertical   = room.Rect.Inflated(-chamberMargin, mapGenerator.RoomGenerator.RoomPadding * 2);

            if (horizontal.height < chamberSize || vertical.width < chamberSize)
            {
                if (logWarnigns)
                    Debug.LogWarning("ChamberGenerator: Chamber does not fit (" + room.name + ": " + room.Rect.width
                        + "x" + room.Rect.height + ". Required wall length: " + (chamberSize + chamberMargin * 2) + ")");

                return false;
            }
            return true;
        }

        // Check all rooms

        for (int i = 0; i < map.Rooms.Count; ++i)
        {
            // Get rects of room 1

            if (!GetRects(map.Rooms[i], out horizontal1, out vertical1))
                continue;

            // Check all other rooms

            for (int j = i + 1; j < map.Rooms.Count; ++j)
            {
                // Get rects of room 2

                if (!GetRects(map.Rooms[j], out horizontal2, out vertical2))
                    continue;

                // Check if the two rooms could be connected

                if (horizontal1.Overlaps(horizontal2, out overlap) && overlap.height >= chamberSize)
                    NewConnection(i, j, overlap, Orientation.Horizontal);

                else if (vertical1.Overlaps(vertical2, out overlap) && overlap.width >= chamberSize)
                    NewConnection(i, j, overlap, Orientation.Vertical);
            }
        }

        // Check if all rooms are connected

        if (logWarnigns && TraverseGraph() < nodes.Count)
        {
            foreach (Node node in nodes)
                if (!node.marked)
                    Debug.LogWarning("ChamberGenerator: " + node.room.name + " can not be reached from Room 0.");
        }
    }

    private void NewConnection(int i, int j, RectInt overlap, Orientation orientation)
    {
        paths.Add(new Path(paths.Count, nodes[i], nodes[j], overlap, orientation));
        nodes[i].adjacentNodes.Add(nodes[j]);
        nodes[j].adjacentNodes.Add(nodes[i]);
    }

    private int TraverseGraph()
    {
        int markedNodes = TraverseNode(nodes[0]);

        foreach (Node node in nodes)
            node.marked = false;

        return markedNodes;
    }

    private int TraverseNode(Node node)
    {
        node.marked = true;
        int markedNodes = 1;

        foreach (Node adjacentNode in node.adjacentNodes)
            if (!adjacentNode.marked)
                markedNodes += TraverseNode(adjacentNode);

        return markedNodes;
    }

    private void DisconnectNodes()
    {
        paths.Shuffle();

        for (int i = paths.Count - 1; i >= 0; --i)
        {
            Path path = paths[i];

            // Disconnect the two nodes
            path.node1.adjacentNodes.Remove(path.node2);
            path.node2.adjacentNodes.Remove(path.node1);

            // If the graph is not fully connected
            if (TraverseGraph() < nodes.Count)
            {
                // Reconnect the nodes
                path.node1.adjacentNodes.Add(path.node2);
                path.node2.adjacentNodes.Add(path.node1);
            }

            // If the graph is still fully connected
            else
            {
                // Put aside this path for later use
                paths.RemoveAt(i);
                removedPaths.Add(path);
            }
        }
    }

    private void ReconnectNodes()
    {
        if (!addRandomChambers)
            return;

        float goalPercentage = Random.Range(minChambers, maxChambers);

        while ((float)paths.Count / (paths.Count + removedPaths.Count) < goalPercentage)
        {
            // Get random removed path
            int i = Random.Range(0, removedPaths.Count);
            Path path = removedPaths[i];

            // Reconnect the rooms
            paths.Add(path);
            removedPaths.RemoveAt(i);
            path.node1.adjacentNodes.Add(path.node2);
            path.node2.adjacentNodes.Add(path.node1);
        }
    }

    private void InstantiateChambers(Map map)
    {
        paths = paths.OrderBy(path => path.index).ToList();

        foreach (Path path in paths)
        {
            RectInt rect = path.overlap;

            if (path.orientation == Orientation.Horizontal)
            {
                rect.y += Random.Range(0, rect.height - chamberSize);
                rect.height = chamberSize;
            }

            else // (path.orientation == Chamber.Orientation.Vertical)
            {
                rect.x += Random.Range(0, rect.width - chamberSize);
                rect.width = chamberSize;
            }

            map.Chambers.Add(GameObject.Instantiate(chamberPrefab, map.transform).GetComponent<Chamber>()
                .Initialize(path.node1.room, path.node2.room, rect, path.orientation));
        }
    }
    
}
