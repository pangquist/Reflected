using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Room : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform pathsChild;
    [SerializeField] private Transform terrainChild;
    [SerializeField] private Transform objectsChild;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private Rect rect;
    [ReadOnly][SerializeField] private bool cleared;
    [ReadOnly][SerializeField] private RoomType type;
    [ReadOnly][SerializeField] private List<Wall> walls;
    [ReadOnly][SerializeField] private List<Chamber> chambers;
    [ReadOnly][SerializeField] private List<PathCreator> paths;
    [ReadOnly][SerializeField] private List<Vector3> pathPoints;
     
    private static Map map;

    // Properties

    public Transform PathsChild => pathsChild;
    public Transform TerrainChild => terrainChild;
    public Transform ObjectsChild => objectsChild;

    public Rect Rect => rect;
    public bool Cleared => cleared;
    public RoomType Type => type;
    public List<Wall> Walls => walls;
    public List<Chamber> Chambers => chambers;
    public List<PathCreator> Paths => paths;
    public List<Vector3> PathPoints => pathPoints;

    private void Awake()
    {
        TerrainGenerator.Finished.AddListener(MoveShopStructure);
    }

    private void MoveShopStructure()
    {
        if (type == RoomType.Shop)
        {
            Vector3 shopPosition = new Vector3(rect.center.x, 100f, rect.center.y);
            RaycastHit raycastHit;
            Physics.Raycast(shopPosition, Vector3.down, out raycastHit);
            shopPosition.y = raycastHit.point.y;
            transform.GetComponentInChildren<Shop>().transform.position = shopPosition;
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

        if (type == RoomType.Start)
        {
            Map.StartRoom = this;
        }

        else if (type == RoomType.Boss)
        {
            Map.BossRoom = this;

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

    public void ScaleUpData()
    {
        rect = new Rect(rect.position * MapGenerator.ChunkSize, rect.size * MapGenerator.ChunkSize);
    }

    private void Update()
    {
        if (Map.ActiveRoom != this)
            return;

        if (!cleared && map.GameManager.AiDirector.AllEnemiesKilled)
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

        //THIS WILL BE MOVED
        //if (type == RoomType.Monster)
        //    GameObject.Find("Music Manager").GetComponent<MusicManager>().ChangeMusicIntensity(-1);
        //else if (type == RoomType.Boss)
        //    GameObject.Find("Music Manager").GetComponent<MusicManager>().ChangeMusicIntensity(-2);

        Map.ActiveRoom = null;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates all connected chambers
    /// </summary>
    public void Activate()
    {
        Map.ActiveRoom = this;
        Map.RoomEntered.Invoke();

        foreach (Chamber chamber in chambers)
            chamber.gameObject.SetActive(true);

        if (cleared)
        {
            foreach (Chamber chamber in chambers)
                chamber.Open(this);
        }

        else
        {
            if (type == RoomType.Monster || type == RoomType.Boss || type == RoomType.Shop)
            {
                map.GameManager.AiDirector.EnterRoom();
                GameObject.Find("Music Manager").GetComponent<MusicManager>().ChangeMusicIntensity(1);

                if (type == RoomType.Boss)
                {
                    GameObject.Find("Music Manager").GetComponent<MusicManager>().ChangeMusicIntensity(2);
                }
            }

            else
            {
                SetCleared(true);
            } 
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

            Map.RoomCleared.Invoke();
        }

        else
        {
            foreach (Chamber chamber in chambers)
                chamber.Close(this);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 point in pathPoints)
            Gizmos.DrawSphere(point, 0.25f);
    }

}
