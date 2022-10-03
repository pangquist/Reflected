using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecorationList
{
    public string terrain;
    public GameObject[] gameObject;
}

public class DecorationPlacer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TerrainGenerator terrainGenerator;

    [Header("Decorations")]
    [SerializeField] DecorationList[] decorations;

    public void PlaceDecorations(TerrainChunk terrainChunk)
    {
        Vector3[] meshVertices = terrainChunk.MeshFilter().mesh.vertices;
        Vector3[] visitedVertices = new Vector3[meshVertices.Length];

        float offsetX = -terrainChunk.transform.position.x;
        float offsetZ = -terrainChunk.transform.position.z;
        TerrainType[] terrainTypes = terrainGenerator.TerrainTypes();

        for (int i = 0; i < meshVertices.Length; i++)
        {
            foreach (TerrainType terrain in terrainTypes)
            {
                if (meshVertices[i].y <= terrainGenerator.HeightCurve().Evaluate(terrain.height) * terrainGenerator.HeightMultiplier())
                {
                    if (meshVertices[i] != visitedVertices[i])
                    {
                        foreach (DecorationList decorationList in decorations)
                        {
                            if (decorationList.terrain == terrain.name)
                            {
                                if (Random.Range(1, 50) == 1)
                                {
                                    Instantiate(decorationList.gameObject[Random.Range(0, decorationList.gameObject.Length)].gameObject, new Vector3(meshVertices[i].x - offsetX, meshVertices[i].y, meshVertices[i].z - offsetZ), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), terrainChunk.transform);
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
