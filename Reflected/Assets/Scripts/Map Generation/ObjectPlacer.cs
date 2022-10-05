using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectList
{
    public string terrain;
    public WeightedRandomList<GameObject> terrainObjects;
}

public class ObjectPlacer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TerrainGenerator terrainGenerator;

    [Header("Decorations")]
    [SerializeField] ObjectList[] objects;

    public void PlaceDecorations(TerrainChunk terrainChunk)
    {
        Vector3[] meshVertices = terrainChunk.MeshFilter().mesh.vertices;
        Vector3[] visitedVertices = new Vector3[meshVertices.Length];

        float offsetX = -terrainChunk.transform.position.x + MapGenerator.ChunkSize * 0.5f;
        float offsetZ = -terrainChunk.transform.position.z + MapGenerator.ChunkSize * 0.5f;
        TerrainType[] terrainTypes = terrainGenerator.TerrainTypes();

        for (int i = 0; i < meshVertices.Length; i++)
        {
            foreach (TerrainType terrain in terrainTypes)
            {
                if (meshVertices[i].y <= terrainGenerator.HeightCurve().Evaluate(terrain.height) * terrainGenerator.HeightMultiplier())
                {
                    if (meshVertices[i] != visitedVertices[i])
                    {
                        foreach (ObjectList objectList in objects)
                        {
                            if (objectList.terrain == terrain.name)
                            {
                                if (Random.Range(1, 50) == 1)
                                {
                                    Matrix4x4 localToWorld = transform.localToWorldMatrix;
                                    Vector3 position = terrainChunk.transform.rotation * localToWorld.MultiplyPoint3x4(meshVertices[i]);
                                    position = new Vector3(position.x - offsetX, position.y, position.z - offsetZ);
                                    Instantiate(objectList.terrainObjects.GetRandom(), position, Quaternion.identity, terrainChunk.transform);
                                }
                            }
                        }
                        visitedVertices[i] = meshVertices[i];
                    }

                }
            }
        }
    }
}
