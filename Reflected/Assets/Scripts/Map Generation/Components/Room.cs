using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform pathsParent;
    [SerializeField] private Transform terrainParent;
    [SerializeField] private Transform structuresParent;
    [SerializeField] private Transform decorationsParent;
    [SerializeField] private Transform spawnPointsParent;
    [SerializeField] private Transform enemiesParent;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private Rect rect;
    [ReadOnly][SerializeField] private bool cleared;
    [ReadOnly][SerializeField] private RoomType type;
    [ReadOnly][SerializeField] private List<Wall> walls;
    [ReadOnly][SerializeField] private List<Chamber> chambers;
    [ReadOnly][SerializeField] private List<PathCreator> paths;
    [ReadOnly][SerializeField] private List<Vector3> pathPoints;
    [ReadOnly][SerializeField] private List<Structure> structures;
    [ReadOnly][SerializeField] private Stats trueStat;
    [ReadOnly][SerializeField] private Stats mirrorStat;

    private static Map map;

    // Properties

    public Transform PathsParent => pathsParent;
    public Transform TerrainParent => terrainParent;
    public Transform StructuresParent => structuresParent;
    public Transform DecorationsParent => decorationsParent;
    public Transform SpawnPointsParent => spawnPointsParent;
    public Transform EnemiesParent => enemiesParent;

    public Rect Rect => rect;
    public bool Cleared => cleared;
    public RoomType Type => type;
    public List<Wall> Walls => walls;
    public List<Chamber> Chambers => chambers;
    public List<PathCreator> Paths => paths;
    public List<Vector3> PathPoints => pathPoints;
    public List<Structure> Structures => structures;
    public Stats TrueStat => trueStat;
    public Stats MirrorStat => mirrorStat;

    private void Awake()
    {
        //AiDirector.RoomCleared.AddListener(() => SetCleared(true));

        Array stats = Enum.GetValues(typeof(Stats));
        trueStat = (Stats)stats.GetValue(Random.Range(0, stats.Length));

        do { mirrorStat = (Stats)stats.GetValue(Random.Range(0, stats.Length)); }
        while (trueStat == mirrorStat);
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

        if (cleared)
            return;

        if (type == RoomType.Boss)
        {
            if (structuresParent.GetComponentInChildren<Boss>().Dead())
                SetCleared(true);
        }

        else
        {
            if (map.GameManager.AiDirector.AllEnemiesKilled)
                SetCleared(true);
        }
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

        Map.ActiveRoom = null;
        terrainParent.gameObject.SetActive(false);
        structuresParent.gameObject.SetActive(false);
        decorationsParent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates this room and all connecting chambers
    /// </summary>
    public void Activate(bool instant)
    {
        StartCoroutine(Coroutine_Activate(instant));
    }

    private IEnumerator Coroutine_Activate(bool instant)
    {
        terrainParent.gameObject.SetActive(true);
        if (!instant) yield return null;
        structuresParent.gameObject.SetActive(true);
        if (!instant) yield return null;
        decorationsParent.gameObject.SetActive(true);
        if (!instant) yield return null;

        foreach (Chamber chamber in chambers)
            chamber.gameObject.SetActive(true);

        if (!instant) yield return null;
        Map.ActiveRoom = this;

        if (!cleared && type == RoomType.Start)
            SetCleared(true);

        Map.RoomEntered.Invoke();
        if (!instant) yield return null;

        if (cleared)
            foreach (Chamber chamber in chambers)
                chamber.Open(this);

        yield return 0;
    }

    public void SetCleared(bool cleared)
    {
        this.cleared = cleared;

        if (cleared)
        {
            foreach (Chamber chamber in chambers)
                chamber.Open(this);

            if (type != RoomType.Start)
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
        Gizmos.color = Color.white;
        foreach (Vector3 pathPoint in pathPoints)
            Gizmos.DrawSphere(pathPoint, 0.5f);

        Gizmos.color = Color.red;
        foreach (Transform spawnPoint in spawnPointsParent)
            Gizmos.DrawSphere(spawnPoint.position, 1f);
    }

}
