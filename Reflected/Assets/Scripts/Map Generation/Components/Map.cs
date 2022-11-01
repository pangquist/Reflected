using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private MapGraph graph;

    [Header("Values")]

    [SerializeField] private bool singleActiveRoom;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private int sizeX;
    [ReadOnly][SerializeField] private int sizeZ;
    [ReadOnly][SerializeField] private List<Room> rooms;
    [ReadOnly][SerializeField] private List<Chamber> chambers;
    [ReadOnly][SerializeField] private Room activeRoom;
    [ReadOnly][SerializeField] private Room startRoom;
    [ReadOnly][SerializeField] private Room bossRoom;
    [ReadOnly][SerializeField] private DimensionManager dimensionManager;

    // Properties

    public int SizeX => sizeX;
    public int SizeZ => sizeZ;
    public List<Room> Rooms => rooms;
    public List<Chamber> Chambers => chambers;
    public bool SingleActiveRoom => singleActiveRoom;
    public DimensionManager DimensionManager => dimensionManager;
    public MapGraph Graph => graph;

    public Room ActiveRoom { get { return activeRoom; } set { activeRoom = value; } }
    public Room StartRoom  { get { return startRoom;  } set { startRoom  = value; } }
    public Room BossRoom   { get { return bossRoom;   } set { bossRoom   = value; } }

    public void Initialize(int sizeX, int sizeZ)
    {
        this.sizeX = sizeX;
        this.sizeZ = sizeZ;
        name = "Map";

        dimensionManager = GameObject.Find("Dimension Manager").GetComponent<DimensionManager>();
    }

    public void ScaleUpData()
    {
        foreach (Room room in rooms)
            room.ScaleUpData();

        foreach (Chamber chamber in chambers)
            chamber.ScaleUpData();
    }

    public void GenerateGraph()
    {
        graph.Generate();
    }

    public void DeactivateAll()
    {
        foreach (Room room in rooms)
            room.Deactivate(null);
    }

    public void Begin()
    {
        startRoom.gameObject.SetActive(true);
        startRoom.Activate();
        
        Transform player = GameObject.Find("Player").transform;

        Debug.Log("Player " + (player == null ? "not " : "") + "found.");

        Debug.Log("Player position before: "
            + "\nx = " + player.position.x.ToString("0.0")
            + "\ny = " + player.position.y.ToString("0.0")
            + "\nz = " + player.position.z.ToString("0.0"));

        Debug.Log("Moving player to center of start room (" + startRoom + ")"
            + "\nx = " + startRoom.Rect.center.x.ToString("0.0")
            + "\ny = 10.0"
            + "\nz = " + startRoom.Rect.center.y.ToString("0.0"));

        player.position = new Vector3(startRoom.Rect.center.x, 10, startRoom.Rect.center.y);

        Debug.Log("Player position after: "
            + "\nx = " + player.position.x.ToString("0.0")
            + "\ny = " + player.position.y.ToString("0.0")
            + "\nz = " + player.position.z.ToString("0.0"));
    }
}
