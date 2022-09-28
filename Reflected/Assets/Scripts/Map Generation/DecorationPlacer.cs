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
    [SerializeField]
    TileGeneration tileGeneration;
    [SerializeField]
    DecorationList[] decorations;

    // Start is called before the first frame update
    void Start()
    {
        SpawnDecorations();
    }

    void SpawnDecorations()
    {
        Vector3[] meshVertices = this.tileGeneration.MeshFilter().mesh.vertices;
        Vector3[] visitedVertices = new Vector3[meshVertices.Length];

        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;
        TerrainType[] terrainTypes = this.tileGeneration.TerrainTypes();

        for (int i = 0; i < meshVertices.Length; i++)
        {
            foreach(TerrainType terrain in terrainTypes)
            {
                if (meshVertices[i].y  <= this.tileGeneration.HeightCurve().Evaluate(terrain.height) * this.tileGeneration.HeightMultiplier())
                {
                    if(meshVertices[i] != visitedVertices[i])
                    {
                        foreach (DecorationList decorationList in decorations)
                        {
                            if (decorationList.terrain == terrain.name)
                            {
                                if (Random.Range(1, 50) == 1)
                                {
                                    Instantiate(decorationList.gameObject[Random.Range(0, decorationList.gameObject.Length)].gameObject, new Vector3(meshVertices[i].x - offsetX, meshVertices[i].y, meshVertices[i].z - offsetZ), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                                }
                            }
                        }
                        visitedVertices[i] = meshVertices[i];
                    }
                    
                }
            } 
        }
 

        //float[,] heightMap = this.tileGeneration.GenerateHeightMap();

        //int tileDepth = heightMap.GetLength(0);
        //int tileWidth = heightMap.GetLength(1);
        //Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        //// iterate through all the heightMap coordinates, updating the vertex index
        //int vertexIndex = 0;
        //for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        //{
        //    for (int xIndex = 0; xIndex < tileWidth; xIndex++)
        //    {
        //        float height = heightMap[zIndex, xIndex];
        //        Vector3 vertex = meshVertices[vertexIndex];
        //        // change the vertex Y coordinate, proportional to the height value
        //        meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);
        //        vertexIndex++;
        //    }
        //}
    }
}
