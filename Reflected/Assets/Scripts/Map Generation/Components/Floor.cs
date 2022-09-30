using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject blockPrefab;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private RectInt rect;

    // Properties

    public RectInt Rect => rect;

    public Floor Initialize(RectInt rect)
    {
        this.rect = rect;
        name = "Floor";

        GameObject block = GameObject.Instantiate(blockPrefab, transform);
        block.transform.position = new Vector3(rect.x, -1, rect.y) * MapGenerator.ChunkSize;
        block.transform.localScale = new Vector3(rect.width, 1, rect.height) * MapGenerator.ChunkSize;
        block.GetComponentInChildren<MeshRenderer>().material.color = new Color(0f, 1f, 0f);
        block.GetComponentInChildren<ReCalcCubeTexture>().Calculate();

        return this;
    }

}
