using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chamber : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject floorPrefab;

    [Header("Chamber")]

    [SerializeField] private float triggerTime;

    [Header("Read Only")]
    [ReadOnly][SerializeField] private Room room1;
    [ReadOnly][SerializeField] private Room room2;
    [ReadOnly][SerializeField] private Orientation orientation;
    [ReadOnly][SerializeField] private RectInt rect;
    [ReadOnly][SerializeField] private Floor floor;
    [ReadOnly][SerializeField] private List<Wall> walls;
    [ReadOnly][SerializeField] private List<Door> doors;

    private Bounds triggerBounds;
    private static bool inTransition;

    // Properties

    public Orientation Orientation => orientation;
    public List<Wall> Walls => walls;
    public List<Door> Doors => doors;
    public RectInt Rect => rect;

    public Chamber Initialize(Room room1, Room room2, RectInt rect, Orientation orientation)
    {
        this.room1 = room1;
        this.room2 = room2;
        this.rect = rect;
        this.orientation = orientation;

        triggerBounds = new Bounds(
            new Vector3(rect.x + rect.width * 0.5f, Wall.Height * 0.5f, rect.y + rect.height * 0.5f),
            new Vector3(rect.width, Wall.Height, rect.height));

        name = "Chamber " + room1.name.Replace("Room ", "") + "-" + room2.name.Replace("Room ", "");

        room1.Chambers.Add(this);
        room2.Chambers.Add(this);

        return this;
    }

    public void CreateFloor(int wallThickness)
    {
        RectInt floorRect;

        if (orientation == Orientation.Horizontal)
            floorRect = rect.Inflated(-wallThickness, wallThickness);
        else
            floorRect = rect.Inflated(wallThickness, -wallThickness);

        floor = GameObject.Instantiate(floorPrefab, transform).GetComponent<Floor>().Initialize(floorRect);
    }

    private void Update()
    {
        // Ensure the active room is exitable
        if (false)
            return;

        // Ensure there is no active transition
        if (inTransition)
            return;

        Vector3 playerPosition = new Vector3();

        // Ensure the player is inside the chamber
        if (triggerBounds.Contains(playerPosition) == false)
            return;

        // Get open and closed door
        Door openDoor   = doors[doors[0].IsOpen ? 0 : 1];
        Door closedDoor = doors[doors[0].IsOpen ? 1 : 0];

        // Check distance from player to each door
        float distanceToOpenDoor   = Vector3.Distance(playerPosition, openDoor  .MeasuringPosition);
        float distanceToClosedDoor = Vector3.Distance(playerPosition, closedDoor.MeasuringPosition);

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

        // Ensure the player is inside the chamber
        Vector3 playerPosition = new Vector3();

        if (triggerBounds.Contains(playerPosition) == false)
            playerPosition = triggerBounds.center;

        // Deactivate previous room
        openDoor.Room.Deactivate(this);

        // Wait another 0.5 seconds
        for (float timer = 0; timer < 0.5f; timer += Time.deltaTime)
            yield return null;

        // Activate next room
        closedDoor.Room.gameObject.SetActive(true);

        // Start opening the closed door
        StartCoroutine(closedDoor.Coroutine_Open());

        inTransition = false;
        yield return 0;
    }

}
