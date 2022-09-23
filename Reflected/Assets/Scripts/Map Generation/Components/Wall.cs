using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject blockPrefab;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private BoundsInt bounds;

    private static int height;
    private static int thickness;

    // Properties

    public BoundsInt Bounds => bounds;
    public static int Thickness => thickness;
    public static int Height => height;

    public static void StaticInitialize(int thickness, int height)
    {
        Wall.thickness = thickness;
        Wall.height = height;
    }

    private void Initialize(int x, int z, int sizeX, int sizeZ)
    {
        bounds = new BoundsInt(x, 0, z, sizeX, height, sizeZ);

        GameObject block = GameObject.Instantiate(blockPrefab, transform);
        block.transform.position = bounds.position;
        block.transform.localScale = bounds.size;
        block.GetComponentInChildren<MeshRenderer>().material.color = new Color(1f, 1f, 1f);
    }

    public Wall InitializeNorth(RectInt r, bool extend)
    {
        name = "Wall North";
        Initialize(r.x - (extend ? thickness : 0), r.y + r.height, r.width + thickness * (extend ? 2 : 0), thickness);
        return this;
    }

    public Wall InitializeSouth(RectInt r, bool extend)
    {
        name = "Wall South";
        Initialize(r.x - (extend ? thickness : 0), r.y - thickness, r.width + thickness * (extend ? 2 : 0), thickness);
        return this;
    }

    public Wall InitializeWest(RectInt r, bool extend)
    {
        name = "Wall West";
        Initialize(r.x - thickness, r.y - (extend ? thickness : 0), thickness, r.height + thickness * (extend ? 2 : 0));
        return this;
    }

    public Wall InitializeEast(RectInt r, bool extend)
    {
        name = "Wall East";
        Initialize(r.x + r.width, r.y - (extend ? thickness : 0), thickness, r.height + thickness * (extend ? 2 : 0));
        return this;
    }

}
