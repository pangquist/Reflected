using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaryingTransform : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private bool randomizeRotationX = false;
    [SerializeField] private bool randomizeRotationY = true;
    [SerializeField] private bool randomizeRotationZ = false;

    [Header("Scale")]
    [SerializeField] private bool randomizeScale = true;
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;

    [Range(-0.98f, 0.98f)]
    [SerializeField] private float scaleBias = -0.4f;

    private void Awake()
    {
        NewRotation();
        NewScale();
        Destroy(this);
    }

    private void NewRotation()
    {
        transform.Rotate(
            randomizeRotationX ? Random.Range(0f, 360f) : 0,
            randomizeRotationY ? Random.Range(0f, 360f) : 0,
            randomizeRotationZ ? Random.Range(0f, 360f) : 0);
    }

    private void NewScale()
    {
        if (!randomizeScale)
            return;

        float scale = minScale + (maxScale - minScale) * Random.Range(0f, 1f).CustomSmoothstep(scaleBias);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
