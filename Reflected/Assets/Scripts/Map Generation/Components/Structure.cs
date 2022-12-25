using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Structure : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TerrainFlattener terrainFlattener;
    [SerializeField] private Transform navMeshObstacles;
    [SerializeField] private Transform decorationObstructors;

    [Header("Values")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private float obstructiveRadius = 6;
    [SerializeField] private bool avoidPaths = true;

    // Properties

    public TerrainFlattener TerrainFlattener => terrainFlattener;
    public float ObstructiveRadius => obstructiveRadius;
    public bool AvoidPaths => avoidPaths;

    private void Awake()
    {
        ObjectPlacer.Finished.AddListener(DestroyDecorationObstructors);
    }

    public float ObstructiveArea()
    {
        return obstructiveRadius * obstructiveRadius * Mathf.PI;
    }

    [ExecuteInEditMode]
    private void OnDrawGizmosSelected()
    {
        if (drawGizmos)
            Gizmos.DrawWireSphere(transform.position, obstructiveRadius);
    }

    private void DestroyDecorationObstructors()
    {
        if (decorationObstructors == null)
            return;

        foreach(Collider collider in decorationObstructors.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        Destroy(decorationObstructors.gameObject);
    }

}
