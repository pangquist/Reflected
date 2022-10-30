using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    [Header("Seed")]

    [SerializeField] private int seed;

    [Header("References")]

    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private RoomTypeGenerator roomTypeGenerator;
    [SerializeField] private ChamberGenerator chamberGenerator;
    [SerializeField] private PathGenerator pathGenerator;
    [SerializeField] private WallGenerator wallGenerator;
    [SerializeField] private PillarGenerator pillarGenerator;
    [SerializeField] private WaterGenerator waterGenerator;
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private ObjectPlacer objectPlacer;

    [Header("Map")]

    [SerializeField] private bool deactivateRooms;

    [Range(10, 500)]
    [Tooltip("In chunks")]
    [SerializeField] private int minMapSizeX;

    [Range(10, 500)]
    [Tooltip("In chunks")]
    [SerializeField] private int maxMapSizeX;

    [Range(10, 500)]
    [Tooltip("In chunks")]
    [SerializeField] private int minMapSizeZ;

    [Range(10, 500)]
    [Tooltip("In chunks")]
    [SerializeField] private int maxMapSizeZ;

    [Range(1, 20)]
    [SerializeField] private int chunkSize;

    [Header("Testing")]

    [Min(1)]
    [SerializeField] private int trials;

    [Range(0f, 2f)]
    [SerializeField] private float interval;

    [Header("Read Only")]

    [TextArea()]
    [ReadOnly][SerializeField] private string log;

    // Properties

    public static int ChunkSize { get; private set; }

    public RoomGenerator     RoomGenerator     => roomGenerator;
    public RoomTypeGenerator RoomTypeGenerator => roomTypeGenerator;
    public ChamberGenerator  ChamberGenerator  => chamberGenerator;
    public PathGenerator     PathGenerator     => pathGenerator;
    public WallGenerator     WallGenerator     => wallGenerator;
    public PillarGenerator   PillarGenerator   => pillarGenerator;
    public WaterGenerator    WaterGenerator    => waterGenerator;
    public TerrainGenerator  TerrainGenerator  => terrainGenerator;
    public ObjectPlacer      ObjectPlacer      => objectPlacer;


    private void Start()
    {
        ChunkSize = chunkSize;

        if (trials == 1)
            Generate();
        else
            StartCoroutine(Coroutine_BulkGenerate());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            Start();
    }

    private void Generate()
    {
        // Prepare

        log = "Map generation log\n";
        int newSeed = seed != 0 ? seed : (int)System.DateTime.Now.Ticks;
        Random.InitState(newSeed);
        terrainGenerator.SetRandomSeed(newSeed);
        Log("Seed: " + newSeed);
        Destroy(GameObject.Find("Map"));

        // Initialize map

        int sizeX = Random.Range(minMapSizeX, maxMapSizeX + 1);
        int sizeZ = Random.Range(minMapSizeZ, maxMapSizeZ + 1);
        Log("Map Size: " + sizeX + "x" + sizeZ + " chunks");

        Map map = GameObject.Instantiate(mapPrefab).GetComponent<Map>();
        map.Initialize(sizeX, sizeZ);

        // Generate map

        roomGenerator    .Generate(map);
        chamberGenerator .Generate(map);

        map.GenerateGraph();

        roomTypeGenerator.Generate(map);

        map.ScaleUpData();

        pathGenerator    .Generate(map);
        wallGenerator    .Generate(map);
        pillarGenerator  .Generate(map);
        waterGenerator   .Generate(map);
        terrainGenerator .Generate(map);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
        BakeNavMesh(map.GetComponent<NavMeshSurface>());
        player.SetActive(true);

        objectPlacer.Place(map);

        // Log

        Log("");
        Debug.Log(log);

        // Begin

        if (deactivateRooms)
            map.DeactivateAll();

        map.Begin();
    }

    private IEnumerator Coroutine_BulkGenerate()
    {
        float cooldown = 0f;
        int completedTrials = 0;

        while (completedTrials < trials)
        {
            cooldown += Time.deltaTime;

            if (cooldown >= interval)
            {
                Generate();
                if (completedTrials % 10 == 0)
                    Debug.Log(completedTrials + " trials completed.");
                cooldown -= interval;
                ++completedTrials;
            }
            
            yield return null;
        }

        Debug.Log("All trials completed.");
        yield return 0;
    }

    public void Log(string text)
    {
        log += "\n" + text;
    }

    private void BakeNavMesh(NavMeshSurface surface)
    {
        surface.BuildNavMesh();
    }
}
