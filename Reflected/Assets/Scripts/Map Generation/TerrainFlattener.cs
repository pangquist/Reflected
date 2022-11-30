using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFlattener : MonoBehaviour
{
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private float innerRadius = 6;
    [SerializeField] private float fade = 10;
    [SerializeField] private LayerMask layerMask;

    private static Vector3 terrainChunkOffset;

    private float outerRadius => innerRadius + fade;

    private void Awake()
    {
        StructurePlacer.Finished.AddListener(Trigger);
        terrainChunkOffset = new Vector3(MapGenerator.ChunkSize * 0.5f, 0, MapGenerator.ChunkSize * 0.5f);
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        // Determine level

        RaycastHit raycastHit;
        Physics.Raycast(new Vector3(transform.position.x, 100f, transform.position.z), Vector3.down, out raycastHit, 150f, layerMask);

        // Find terrainChunks

        Collider[] colliders = Physics.OverlapSphere(transform.position, outerRadius, layerMask);
        TerrainChunk terrainChunk;

        foreach (Collider collider in colliders)
        {
            if (terrainChunk = collider.transform.parent.GetComponent<TerrainChunk>())
            {
                Flatten(terrainChunk, raycastHit.point.y);
            }
        }
    }

    private void Flatten(TerrainChunk terrainChunk, float level)
    {
        Vector3[] meshVertices = terrainChunk.MeshFilter().mesh.vertices;
        Matrix4x4 matrix = terrainChunk.transform.localToWorldMatrix;
        Vector3 vertexWorldPos;
        float distance, adaption, adaptionAmount;
        bool modified = false;

        for (int i = 0; i < meshVertices.Length; ++i)
        {
            vertexWorldPos = matrix.MultiplyPoint3x4(meshVertices[i]) - terrainChunkOffset;
            distance = Vector2.Distance(vertexWorldPos.XZ(), transform.position.XZ());

            if (distance < outerRadius)
            {
                modified = true;

                // How much to affect the vertex based of its distance to origin
                adaptionAmount = (distance - innerRadius) / (outerRadius - innerRadius);
                adaptionAmount = -Mathf.Clamp01(adaptionAmount) + 1f;
                adaptionAmount = adaptionAmount.Smoothstep();

                // The final height adaption
                adaption = (level - meshVertices[i].y) * adaptionAmount;

                // Apply adaption
                meshVertices[i].y += adaption;
            }
        }

        if (modified)
            terrainChunk.UpdateMesh(ref meshVertices);
    }

    [ExecuteInEditMode]
    private void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
            Gizmos.DrawWireSphere(transform.position, innerRadius);
            Gizmos.DrawWireSphere(transform.position, outerRadius);
        }
    }

}
