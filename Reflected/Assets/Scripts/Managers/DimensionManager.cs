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

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SetTrueDimension()
    {
        volume.profile = trueProfile;
    }

    public void SetMirrorDimension()
    {
        volume.profile = mirrorProfile;
    }
}