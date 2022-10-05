//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChangeableObject description
/// </summary>
public class ChangeableObject : MonoBehaviour
{
    [SerializeField] GameObject[] objects;

    [SerializeField] List<Mesh> trueMeshes = new List<Mesh>();
    [SerializeField] List<Mesh> mirrorMeshes = new List<Mesh>();

    [SerializeField] bool hasChildren;

    void Awake()
    {
        if (hasChildren)
            objects = new GameObject[transform.childCount + 1];
        else
            objects = new GameObject[1];
    }

    void Start()
    {
        GameObject.Find("Dimension Manager").GetComponent<DimensionManager>().AddChangeableObject(this);
        objects[0] = gameObject;

        if (!hasChildren)
            return;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            objects[i + 1] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    public void ChangeToTrueMesh()
    {
        Debug.Log("CHANGING TO TRUE MESH");
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<MeshFilter>().mesh = trueMeshes[i];
        }
    }

    public void ChangeToMirrorMesh()
    {
        Debug.Log("CHANGING TO MIRROR MESH");
        for (int i = 0; i < objects.Length; i++)
        {
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