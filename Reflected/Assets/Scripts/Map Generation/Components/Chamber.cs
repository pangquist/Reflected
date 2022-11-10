using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chamber : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform terrainChild;

    [Header("Values")]

    [Range(0f, 3f)]
    [SerializeField] private float cooldown;

    [Range(0f, 3f)]
    [SerializeField] private float pause;

    [Header("Read Only")]
    [ReadOnly][SerializeField] private Room room1;
    [ReadOnly][SerializeField] private Room room2;
    [ReadOnly][SerializeField] private Orientation orientation;
    [ReadOnly][SerializeField] private Rect rect;
    [ReadOnly][SerializeField] private Bounds triggerBounds;
    [ReadOnly][SerializeField] private Door door1;
    [ReadOnly][SerializeField] private Door door2;
    [ReadOnly][SerializeField] private List<Wall> walls;
    [ReadOnly][SerializeField] private List<Pillar> pillars;

    private static bool inTransition;
    private static Map map;
    private static Player player;

    private float cooldownTimer;

    // Properties

    public Transform TerrainChild => terrainChild;

    public Orientation Orientation => orientation;
    public List<Wall> Walls => walls;
    public List<Pillar> Pillars => pillars;
    public Room Room1 => room1;
    public Room Room2 => room2;
    public Door Door1 => door1;
    public Door Door2 => door2;
    public Rect Rect => rect;

    public static void StaticInitialize(Map map)
    {
        Chamber.map = map;
        player = FindObjectOfType<Player>();
    }

    public Chamber Initialize(Room room1, Room room2, Rect rect, Orientation orientation)
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

    public void ScaleUpData()
    {
        rect = new Rect(rect.position * MapGenerator.ChunkSize, rect.size * MapGenerator.ChunkSize);
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
        float distanceToOpenDoor   = Vector3.Distance(player.transform.position, openDoor  .transform.position);
        float distanceToClosedDoor = Vector3.Distance(player.transform.position, closedDoor.transform.position);

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
        openDoor.Close();

        // Wait til closed
        yield return new WaitForSeconds(openDoor.ClosingDuration);

        // Move player if necessary
        if (triggerBounds.Contains(player.transform.position) == false)
        {
            player.transform.position = triggerBounds.center;
            Debug.Log("Moved player into " + name);
        }

        // Deactivate previous room
        if (map.SingleActiveRoom)
            openDoor.Room.Deactivate(this);

        // Activate next room
        closedDoor.Room.gameObject.SetActive(true);
        closedDoor.Room.Activate();

        // Pause
        yield return new WaitForSeconds(pause);

        // Start opening the closed door
        closedDoor.Open();

        // Wait til opened
        yield return new WaitForSeconds(closedDoor.OpeningDuration);

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
        {
            door1.Open();
            door2.CloseInstantly();
        }
        else if (door2.Room == caller)
        {
            door2.Open();
            door1.CloseInstantly();
        }  
    }

    /// <summary>
    /// Closes the Door of this Chamber leading to the calling Room
    /// </summary>
    public void Close(Room caller)
    {
        if (door1.Room == caller)
        {
            door1.Close();
            door2.CloseInstantly();
        }
        else if (door2.Room == caller)
        {
            door2.Close();
            door1.CloseInstantly();
        } 
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
