using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject blockPrefab;

    [Header("Values")]

    [SerializeField] private Color color;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private CardinalDirection direction;
    [ReadOnly][SerializeField] private Orientation orientation;
    [ReadOnly][SerializeField] private int length;
    [ReadOnly][SerializeField] private List<GameObject> portions;

    private static int height;
    private static int thickness;

    // Properties

    public static int Thickness => thickness;
    public static int Height => height;

    public CardinalDirection Direction => direction;
    public Orientation Orientation => orientation;
    public List<GameObject> Portions => portions;

    public static void StaticInitialize(int thickness, int height)
    {
        Wall.thickness = thickness;
        Wall.height = height;
    }

    public Wall Initialize(CardinalDirection direction)
    {
        this.direction = direction;
        name = "Wall " + direction.ToString();
        orientation = direction == CardinalDirection.North || direction == CardinalDirection.South ? Orientation.Horizontal : Orientation.Vertical;

        return this;
    }

    public Wall AddPortion(Rect rect)
    {
        GameObject block = GameObject.Instantiate(blockPrefab, transform);
        block.transform.position = new Vector3(rect.x, 0, rect.y);
        block.transform.localScale = new Vector3(rect.width, height, rect.height);
        block.GetComponentInChildren<MeshRenderer>().material.color = color;
        block.GetComponentInChildren<ReCalcCubeTexture>().Calculate();
        portions.Add(block);

        return this;
    }

}
