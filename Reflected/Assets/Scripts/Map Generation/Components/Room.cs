using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject floorPrefab;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private RectInt rect;
    [ReadOnly][SerializeField] private Floor floor;
    [ReadOnly][SerializeField] private List<Wall> walls;
    [ReadOnly][SerializeField] private List<Chamber> chambers;
    [ReadOnly][SerializeField] private bool cleared;
    
    // Properties

    public RectInt Rect => rect;
    public Floor Floor => floor;
    public List<Wall> Walls => walls;
    public List<Chamber> Chambers => chambers;

    public Room Initialize(RectInt rect, int index)
    {
        this.rect = rect;
        name = "Room " + index;
        return this;
    }

    public void CreateFloor(int wallThickness)
    {
        floor = GameObject.Instantiate(floorPrefab, transform).GetComponent<Floor>().Initialize(rect.Inflated(wallThickness, wallThickness));
    }

    /// <summary>
    /// Deactivates this room and all connected chambers except the caller
    /// </summary>
    /// <param name="caller"></param>
    public void Deactivate(Chamber caller)
    {
        foreach (Chamber chamber in chambers)
        {
            if (chamber != caller)
                chamber.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates all connected chambers
    /// </summary>
    /// <param name="caller"></param>
    private void OnActivate()
    {
        foreach (Chamber chamber in chambers)
            chamber.gameObject.SetActive(true);

        gameObject.SetActive(true);
    }
}
