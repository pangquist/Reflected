using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject blockPrefab;

    [Header("Values")]

    [SerializeField] private float transitionDuration;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private Room room;
    [ReadOnly][SerializeField] private bool isOpen;
    [ReadOnly][SerializeField] private Vector3 measuringPosition;
    [ReadOnly][SerializeField] private GameObject block;

    private static float thickness;

    // Properties

    public static float Thickness => thickness;

    public float TransitionDuration => transitionDuration;
    public bool IsOpen => isOpen;
    public Room Room => room;
    public Vector3 MeasuringPosition => measuringPosition;

    public static void StaticInitialize(float thickness)
    {
        Door.thickness = thickness;
    }

    public Door Initialize(CardinalDirection direction, Rect rect, Room room)
    {
        this.room = room;
        name = "Door " + direction.ToString();

        block = GameObject.Instantiate(blockPrefab, transform);
        block.transform.position = new Vector3(rect.x, 0, rect.y) * MapGenerator.ChunkSize;
        block.transform.localScale = new Vector3(rect.width, Wall.Height, rect.height) * MapGenerator.ChunkSize;
        block.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f);

        measuringPosition = block.transform.GetChild(0).position;

        return this;
    }

    public IEnumerator Coroutine_Open()
    {
        if (isOpen)
            yield return 0;

        while (block.transform.position.y > -Wall.Height * MapGenerator.ChunkSize)
        {
            yield return null;
            block.transform.position -= new Vector3(0, Wall.Height * MapGenerator.ChunkSize / transitionDuration * Time.deltaTime, 0);  
        }

        block.transform.position = new Vector3(block.transform.position.x, -Wall.Height * MapGenerator.ChunkSize, block.transform.position.z);
        block.SetActive(false);
        isOpen = true;
        yield return 0;
    }

    public IEnumerator Coroutine_Close()
    {
        if (!isOpen)
            yield return 0;

        block.SetActive(true);

        while (block.transform.position.y < 0)
        {
            yield return null;
            block.transform.position += new Vector3(0, Wall.Height * MapGenerator.ChunkSize / transitionDuration * Time.deltaTime, 0);
        }

        block.transform.position = new Vector3(block.transform.position.x, 0, block.transform.position.z);
        isOpen = false;
        yield return 0;
    }

}
