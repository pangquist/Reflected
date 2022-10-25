using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Room : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform pathsChild;
    [SerializeField] private Transform terrainChild;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private Rect rect;
    [ReadOnly][SerializeField] private bool cleared;
    [ReadOnly][SerializeField] private RoomType type;
    [ReadOnly][SerializeField] private List<Wall> walls;
    [ReadOnly][SerializeField] private List<Chamber> chambers;
    [ReadOnly][SerializeField] private List<PathCreator> paths;

    private static Map map;

    // Properties

    public Transform PathsChild => pathsChild;
    public Transform TerrainChild => terrainChild;

    public Rect Rect => rect;
    public List<Wall> Walls => walls;
    public List<Chamber> Chambers => chambers;
    public bool Cleared => cleared;
    public RoomType Type => type;
    public List<PathCreator> Paths => paths;

    private void Start()
    {
        if (type == RoomType.Boss)
        {
            foreach (Wall wall in walls)
            {
                foreach (GameObject portion in wall.Portions)
                    portion.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.3f, 0.3f, 0.3f);

                foreach (Pillar pillar in wall.Pillars)
                    pillar.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.6f, 0.1f, 0.1f);
            }

            foreach (Chamber chamber in chambers)
            {
                foreach (Pillar pillar in chamber.Pillars)
                    pillar.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.6f, 0.1f, 0.1f);
            }
        }
    }

    public static void StaticInitialize(Map map)
    {
        Room.map = map;
    }

    public Room Initialize(Rect rect, int index)
    {
        this.rect = rect;
        name = "Room " + index;
        return this;
    }

    public void SetType(RoomType type)
    {
        this.type = type;
    }

    public void ScaleUpData()
    {
        rect = new Rect(rect.position * MapGenerator.ChunkSize, rect.size * MapGenerator.ChunkSize);
    }

    private void Update()
    {
        if (map.ActiveRoom != this)
            return;

        if (!cleared)
            SetCleared(true);
    }

    /// <summary>
    /// Deactivates this room and all connected chambers except the caller
    /// </summary>
    public void Deactivate(Chamber caller)
    {
        foreach (Chamber chamber in chambers)
        {
            if (chamber != caller)
                chamber.gameObject.SetActive(false);
        }

        map.ActiveRoom = null;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates all connected chambers
    /// </summary>
    public void Activate()
    {
        map.ActiveRoom = this;

        foreach (Chamber chamber in chambers)
            chamber.gameObject.SetActive(true);

        if (!cleared)
        {
            if (type == RoomType.Monster || type == RoomType.Boss)
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<AiDirector>().EnterRoom();

            else
                SetCleared(true);
        }
    }

    public void SetCleared(bool cleared)
    {
        this.cleared = cleared;

        if (cleared)
        {
            foreach (Chamber chamber in chambers)
                chamber.Open(this);

            if (type == RoomType.Monster || type == RoomType.Boss)
                map.DimensionManager.GainCharges(1);
        }
          
        else
        {
            foreach (Chamber chamber in chambers)
                chamber.Close(this);
        } 
    }
}
