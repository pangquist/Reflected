using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("Read Only")]

    [ReadOnly][SerializeField] private RectInt rect;

    // Properties

    public RectInt Rect => rect;

    public Water Initialize(RectInt rect, float waterLevel)
    {
        this.rect = rect;
        name = "Water";

        transform.position = new Vector3(rect.x * MapGenerator.ChunkSize, waterLevel, rect.y * MapGenerator.ChunkSize);
        transform.localScale = new Vector3(rect.width, 1, rect.height) * MapGenerator.ChunkSize;

        return this;
    }

}
