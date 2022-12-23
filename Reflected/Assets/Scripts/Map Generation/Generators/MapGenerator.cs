using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using PathCreation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public class MapGenerator : MonoBehaviour
{
    private delegate void Generator(Map map);

    [Header("Seed")]

    [SerializeField] public int seed;

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
    [SerializeField] private StructurePlacer structurePlacer;
    [SerializeField] private ObjectPlacer objectPlacer;

    [Header("Map")]

    [SerializeField] private bool deactivateRooms;

    [SerializeField] private bool autoFocusCamera;

    [Range(10, 500)]
    [Tooltip("In chunks")]
    [SerializeField] public int minMapSizeX;

    [Range(10, 500)]
    [Tooltip("In chunks")]
    [SerializeField] public int maxMapSizeX;

    [Range(10, 500)]
    [Tooltip("In chunks")]
    [SerializeField] public int minMapSizeZ;

    [Range(10, 500)]
    [Tooltip("In chunks")]
    [SerializeField] public int maxMapSizeZ;

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

    [TextArea()]
    [ReadOnly][SerializeField] private string timerLog;

    public static UnityEvent Finished = new UnityEvent();

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
    public StructurePlacer   StructurePlacer   => structurePlacer;
    public ObjectPlacer      ObjectPlacer      => objectPlacer;

    private void Start()
    {
        ChunkSize = chunkSize;

        if (trials == 1)
            Generate();
        else
            StartCoroutine(Coroutine_BulkGenerate());
    }

    public void Generate()
    {
        // Prepare

        Stopwatch stopwatch = Stopwatch.StartNew();

        log = "Map generation log\n";
        timerLog = "";

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

        Timed(roomGenerator    .Generate, map, "Room generator");
        Timed(chamberGenerator .Generate, map, "Chamber generator");

        map.GenerateGraph();
        map.ScaleUpData();

        Timed(wallGenerator    .Generate, map, "Wall generator");
        Timed(pillarGenerator  .Generate, map, "Pillar generator");
        Timed(roomTypeGenerator.Generate, map, "Room type generator");
        Timed(waterGenerator   .Generate, map, "Water generator");
        Timed(pathGenerator    .Generate, map, "Path generator");
        Timed(terrainGenerator .Generate, map, "Terrain generator");
        Timed(structurePlacer  .Place   , map, "Structure placer");
        Timed(objectPlacer     .Place   , map, "Object placer");
        Timed(BakeNavMesh               , map, "NavMesh baker");

        Finished.Invoke();

        // Log

        stopwatch.Stop();
        timerLog += "\nTotal: " + stopwatch.Elapsed.TotalSeconds.ToString("0.0") + " seconds";
        Log(timerLog);
        Log("");
        Debug.Log(log);

        // Begin

        if (deactivateRooms)
            map.DeactivateAll();

        map.Begin();

        // Move scene view camera

        AutoFocusCamera();
    }

    private void AutoFocusCamera()
    {
#if UNITY_EDITOR

        if (autoFocusCamera)
        {
            if (SceneView.lastActiveSceneView == null)
            {
                Debug.Log("Autommatic camera focus failed because no scene view was active.");
                return;
            }

            SceneView.lastActiveSceneView.LookAt(GameObject.Find("Player").transform.position, Quaternion.Euler(70, 0, 0), 40, false, false);
        }

#endif
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

    private void BakeNavMesh(Map map)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
        map.GetComponent<NavMeshSurface>().BuildNavMesh();
        player.SetActive(true);
    }

    private void Timed(Generator generator, Map map, string generatorName)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        generator.Invoke(map);
        stopwatch.Stop();
        timerLog += "\n" + generatorName + ": \t" + stopwatch.ElapsedMilliseconds + " milliseconds";
    }
}
