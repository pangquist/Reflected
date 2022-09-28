using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject blockPrefab;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private CardinalDirection direction;

    private static int height;
    private static int thickness;

    // Properties

    public static int Thickness => thickness;
    public static int Height => height;

    public static void StaticInitialize(int thickness, int height)
    {
        Wall.thickness = thickness;
        Wall.height = height;
    }

    public Wall Initialize(CardinalDirection direction)
    {
        this.direction = direction;
        name = "Wall " + direction.ToString();

        return this;
    }

    public Wall AddPortion(RectInt rect)
    {
        GameObject block = GameObject.Instantiate(blockPrefab, transform);
        block.transform.position = new Vector3(rect.x, 0, rect.y) * MapGenerator.ChunkSize;
        block.transform.localScale = new Vector3(rect.width, height, rect.height) * MapGenerator.ChunkSize;
        block.GetComponentInChildren<MeshRenderer>().material.color = new Color(1f, 1f, 1f);

        return this;
    }

}
