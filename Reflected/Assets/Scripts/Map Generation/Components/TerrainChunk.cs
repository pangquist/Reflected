using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;

    public MeshRenderer MeshRenderer() { return meshRenderer; }
    public MeshFilter MeshFilter() { return meshFilter; }
    public MeshCollider MeshCollider() { return meshCollider; }

    public void UpdateMesh(ref Vector3[] meshVertices)
    {
        meshFilter.mesh.vertices = meshVertices;
        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();
        meshCollider.sharedMesh = meshFilter.mesh;
    }
}
