using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Read Only")]

    [ReadOnly][SerializeField] private RectInt rect;
    [ReadOnly][SerializeField] private List<Wall> walls;
    [ReadOnly][SerializeField] private List<Chamber> chambers;
    [ReadOnly][SerializeField] private bool cleared;
    [ReadOnly][SerializeField] private RoomType type;

    private static Map map;

    // Properties

    public RectInt Rect => rect;
    public List<Wall> Walls => walls;
    public List<Chamber> Chambers => chambers;
    public bool Cleared => cleared;
    public RoomType Type => type;

    public static void StaticInitialize(Map map)
    {
        Room.map = map;
    }

    public Room Initialize(RectInt rect, int index)
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
        rect = new RectInt(rect.position * MapGenerator.ChunkSize, rect.size * MapGenerator.ChunkSize);
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
