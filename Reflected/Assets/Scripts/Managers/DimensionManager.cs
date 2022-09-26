//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// DimensionManager description
/// </summary>
public class DimensionManager : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] VolumeProfile trueProfile;
    [SerializeField] VolumeProfile mirrorProfile;

    [SerializeField] List<ChangeableObject> changeableObjects;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SetTrueDimension()
    {
        volume.profile = trueProfile;

        foreach (ChangeableObject changeableObject in changeableObjects)
            changeableObject.ChangeToTrueMesh();
    }

    public void SetMirrorDimension()
    {
        volume.profile = mirrorProfile;

        foreach (ChangeableObject changeableObject in changeableObjects)
            changeableObject.ChangeToMirrorMesh();
    }

    public void AddChangeableObject(ChangeableObject newObject)
    {
        changeableObjects.Add(newObject);
    }
}