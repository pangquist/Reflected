using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private List<Vector3> pathPoints = new List<Vector3>();

    public MeshRenderer MeshRenderer() { return meshRenderer; }
    public MeshFilter MeshFilter() { return meshFilter; }
    public MeshCollider MeshCollider() { return meshCollider; }
    public List<Vector3> PathPoints => pathPoints;

    public static explicit operator TerrainChunk(GameObject v)
    {
        throw new NotImplementedException();
    }

    public void PassPointsToMaterial()
    {
        for (int i = 1; i < pathPoints.Count; i++)
        {
            MeshRenderer().material.SetVector("_PathPoint" + i, pathPoints[i-1]);
        }
    }
}
