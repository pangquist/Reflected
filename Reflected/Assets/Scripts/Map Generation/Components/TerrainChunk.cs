using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private RectInt rect;

    public MeshRenderer MeshRenderer() { return meshRenderer; }
    public MeshFilter MeshFilter() { return meshFilter; }
    public MeshCollider MeshCollider() { return meshCollider; }
    public RectInt Rect() { return rect; }

    public void Initialize(Mesh mesh, RectInt rect)
    {
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        this.rect = rect;
    }
}
