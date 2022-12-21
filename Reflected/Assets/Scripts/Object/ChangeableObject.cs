//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChangeableObject description
/// </summary>
public class ChangeableObject : MonoBehaviour
{
    [SerializeField] int numberOfChildren;
    [SerializeField] GameObject[] objects;

    [SerializeField] List<Mesh> trueMeshes = new List<Mesh>();
    [SerializeField] List<Mesh> mirrorMeshes = new List<Mesh>();

    [SerializeField] bool hasChildren;

    void Awake()
    {
        if (hasChildren)
            objects = new GameObject[numberOfChildren + 1];
        else
            objects = new GameObject[1];
    }

    void Start()
    {
        GameObject.Find("Dimension Manager").GetComponent<DimensionManager>().AddChangeableObject(this);
        objects[0] = gameObject;

        if (!hasChildren)
            return;

        for (int i = 0; i < numberOfChildren; i++)
        {
            objects[i + 1] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    public void ChangeToTrueMesh()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i])
                objects[i].GetComponent<MeshFilter>().mesh = trueMeshes[i];
        }
    }

    public void ChangeToMirrorMesh()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i])
                objects[i].GetComponent<MeshFilter>().mesh = mirrorMeshes[i];
        }
    }

    public void UpdateMesh()
    {
        if (DimensionManager.CurrentDimension == Dimension.True)
            ChangeToTrueMesh();
        else
            ChangeToMirrorMesh();
    }
}