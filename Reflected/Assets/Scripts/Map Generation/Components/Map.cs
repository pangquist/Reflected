using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

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
    [ReadOnly][SerializeField] private DimensionManager dimensionManager;
    [ReadOnly][SerializeField] private GameManager gameManager;

    public static UnityEvent RoomEntered = new UnityEvent();
    public static UnityEvent RoomCleared = new UnityEvent();

    // Properties

    public int SizeX => sizeX;
    public int SizeZ => sizeZ;
    public List<Room> Rooms => rooms;
    public List<Chamber> Chambers => chambers;
    public bool SingleActiveRoom => singleActiveRoom;
    public DimensionManager DimensionManager => dimensionManager;
    public GameManager GameManager => gameManager;
    public MapGraph Graph => graph;

    public static Room ActiveRoom { get; set; }
    public static Room StartRoom  { get; set; }
    public static Room BossRoom   { get; set; }

    public void Initialize(int sizeX, int sizeZ)
    {
        this.sizeX = sizeX;
        this.sizeZ = sizeZ;
        name = "Map";

        dimensionManager = GameObject.Find("Dimension Manager").GetComponent<DimensionManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        StartRoom.gameObject.SetActive(true);
        StartRoom.Activate();

        Vector3 position = new Vector3(StartRoom.Rect.center.x, 10, StartRoom.Rect.center.y);
        RaycastHit hit;

        while (true)
        {
            if (Physics.Raycast(position, Vector3.down, out hit) && hit.collider.gameObject.GetComponentInParent<TerrainChunk>())
            {
                StartCoroutine(Coroutine_MovePlayer(position));
                break;
            }
            else
            {
                position.x += 10;
            }
        }
    }

    private IEnumerator Coroutine_MovePlayer(Vector3 position)
    {
        Color fromColor = new Color(0f, 0f, 0f, 1f);
        Color toColor = new Color(0f, 0f, 0f, 0f);

        UiManager uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        uiManager.Tint.color = fromColor;

        yield return null;
        GameObject.Find("Player").transform.position = position;
        yield return new WaitForSeconds(0.3f);

        float duration = 3f;
        float timer = 0f;

        while ((timer += Time.deltaTime) < duration)
        {
            uiManager.Tint.color = Color.Lerp(fromColor, toColor, Mathf.Pow(timer / duration, 4f));
            yield return null;
        }

        uiManager.Tint.color = toColor;
        yield return 0;
    }

}
