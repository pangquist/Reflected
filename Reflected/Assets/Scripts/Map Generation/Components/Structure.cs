using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Structure : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TerrainFlattener terrainFlattener;

    [Header("Values")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private float obstructiveRadius = 6;
    [SerializeField] private bool avoidPaths = true;

    [Header("Read Only")]
    [ReadOnly][SerializeField] private NavMeshObstacle[] navMeshObstacles;

    // Properties

    public TerrainFlattener TerrainFlattener => terrainFlattener;
    public NavMeshObstacle[] NavMeshObstacles => navMeshObstacles;
    public float ObstructiveRadius => obstructiveRadius;
    public bool AvoidPaths => avoidPaths;

    private void Awake()
    {
        navMeshObstacles = transform.Find("NavMeshObstacles").GetComponentsInChildren<NavMeshObstacle>();
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

}
