using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFlattener : MonoBehaviour
{
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private float innerRadius = 6;
    [SerializeField] private float fade = 5;

    private float level;
    private Vector3 terrainChunkOffset;

    private float outerRadius => innerRadius + fade;

    private void Awake()
    {
        MapGenerator.Finished.AddListener(Trigger);
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        // Find terrain parent

        Room room = transform.parent.parent.GetComponent<Room>();

        if (room == null)
        {
            Debug.LogWarning("Could not find parent room.");
            return;
        }

        Transform terrainParent = room.TerrainChild;

        // Determine level

        RaycastHit raycastHit;
        Physics.Raycast(new Vector3(transform.position.x, 100f, transform.position.z), Vector3.down, out raycastHit);
        level = raycastHit.point.y;

        terrainChunkOffset = new Vector3(MapGenerator.ChunkSize * 0.5f, 0, MapGenerator.ChunkSize * 0.5f);

        foreach (TerrainChunk terrainChunk in terrainParent.GetComponentsInChildren<TerrainChunk>())
        {
            Flatten(terrainChunk);
        }
    }

    private void Flatten(TerrainChunk terrainChunk)
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
                adaptionAmount = adaptionAmount.LerpValueSmoothstep();

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
