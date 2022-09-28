using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chamber : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject floorPrefab;

    [Header("Values")]

    [Range(0f, 3f)]
    [SerializeField] private float cooldown;

    [Range(0f, 3f)]
    [SerializeField] private float pause;

    [Header("Read Only")]
    [ReadOnly][SerializeField] private Room room1;
    [ReadOnly][SerializeField] private Room room2;
    [ReadOnly][SerializeField] private Orientation orientation;
    [ReadOnly][SerializeField] private RectInt rect;
    [ReadOnly][SerializeField] private Bounds triggerBounds;
    [ReadOnly][SerializeField] private Floor floor;
    [ReadOnly][SerializeField] private Door door1;
    [ReadOnly][SerializeField] private Door door2;
    [ReadOnly][SerializeField] private List<Wall> walls;

    private static bool inTransition;
    private static Map map;
    private static Player player;

    private float cooldownTimer;

    // Properties

    public Orientation Orientation => orientation;
    public List<Wall> Walls => walls;
    public Door Door1 => door1;
    public Door Door2 => door2;
    public RectInt Rect => rect;

    public static void StaticInitialize(Map map)
    {
        Chamber.map = map;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public Chamber Initialize(Room room1, Room room2, RectInt rect, Orientation orientation)
    {
        this.room1 = room1;
        this.room2 = room2;
        this.rect = rect;
        this.orientation = orientation;

        triggerBounds = new Bounds(
            new Vector3(rect.x + rect.width * 0.5f, Wall.Height * 0.5f, rect.y + rect.height * 0.5f) * MapGenerator.ChunkSize,
            new Vector3(rect.width, Wall.Height, rect.height) * MapGenerator.ChunkSize);

        name = "Chamber " + room1.name.Replace("Room ", "") + "-" + room2.name.Replace("Room ", "");

        room1.Chambers.Add(this);
        room2.Chambers.Add(this);

        return this;
    }

    public void AddDoor(Door door)
    {
        if (door1 == null)
            door1 = door;

        else if (door2 == null)
            door2 = door;
    }

    public void CreateFloor(int wallThickness)
    {
        RectInt floorRect;

        if (orientation == Orientation.Horizontal)
            floorRect = rect.Inflated(0, wallThickness);
        else
            floorRect = rect.Inflated(wallThickness, 0);

        floor = GameObject.Instantiate(floorPrefab, transform).GetComponent<Floor>().Initialize(floorRect);
    }

    private void Update()
    {
        // Ensure there is no active transition, no cooldown, there is an active room, and the active room is cleared
        if (inTransition || cooldownTimer > 0f || map.ActiveRoom == null || !map.ActiveRoom.Cleared)
            return;

        // Ensure the player is inside the chamber
        if (triggerBounds.Contains(player.transform.position) == false)
            return;

        // Get open and closed door
        Door openDoor   = door1.IsOpen ? door1 : door2;
        Door closedDoor = door1.IsOpen ? door2 : door1;

        // Check distance from player to each door
        float distanceToOpenDoor   = Vector3.Distance(player.transform.position, openDoor  .MeasuringPosition);
        float distanceToClosedDoor = Vector3.Distance(player.transform.position, closedDoor.MeasuringPosition);

        // If the player is closer to the closed door than the open door
        if (distanceToClosedDoor < distanceToOpenDoor)
        {
            // Start a room transition
            StartCoroutine(Coroutine_RoomTransition(openDoor, closedDoor));
        }
    }

    private IEnumerator Coroutine_RoomTransition(Door openDoor, Door closedDoor)
    {
        inTransition = true;

        // Start closing the open door
        StartCoroutine(openDoor.Coroutine_Close());

        // Wait til closed
        while (openDoor.IsOpen)
            yield return null;

        if (triggerBounds.Contains(player.transform.position) == false)
            player.transform.position = triggerBounds.center;

        // Deactivate previous room
        openDoor.Room.Deactivate(this);

        // Wait another 0.5 seconds
        for (float timer = 0; timer < pause; timer += Time.deltaTime)
            yield return null;

        // Activate next room
        closedDoor.Room.gameObject.SetActive(true);
        closedDoor.Room.Activate();

        // Start opening the closed door
        StartCoroutine(closedDoor.Coroutine_Open());

        // End transition
        StartCoroutine(Coroutine_Cooldown());
        inTransition = false;
        yield return 0;
    }

    /// <summary>
    /// Opens the Door of this Chamber leading to the calling Room
    /// </summary>
    public void Open(Room caller)
    {
        if (door1.Room == caller)
            StartCoroutine(door1.Coroutine_Open());

        else if (door2.Room == caller)
            StartCoroutine(door2.Coroutine_Open());
    }

    /// <summary>
    /// Closes the Door of this Chamber leading to the calling Room
    /// </summary>
    public void Close(Room caller)
    {
        if (door1.Room == caller)
            StartCoroutine(door1.Coroutine_Close());

        else if (door2.Room == caller)
            StartCoroutine(door2.Coroutine_Close());
    }

    /// <summary>
    /// Disables this Chamber for a duration
    /// </summary>
    private IEnumerator Coroutine_Cooldown()
    {
        cooldownTimer = cooldown;

        while ((cooldownTimer -= Time.deltaTime) > 0f)
            yield return null;

        cooldownTimer = 0f;
        yield return 0;
    }

}
