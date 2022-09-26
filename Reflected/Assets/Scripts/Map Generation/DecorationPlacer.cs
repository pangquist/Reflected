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
        Vector3[] meshVertices = this.tileGeneration.meshFilter.mesh.vertices;

        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;
        TerrainType[] terrainTypes = this.tileGeneration.terrainTypes;

        foreach (Vector3 vertex in meshVertices)
        {
            foreach(DecorationList decorationList in decorations)
            {
                for (int i = 0; i < terrainTypes.Length; i++)
                {
                    if(decorationList.terrain == terrainTypes[i].name)
                    {
                        if (Random.Range(1, 20) == 1)
                        {
                            if (terrainTypes[i].height < vertex.y && i== terrainTypes.Length || terrainTypes[i].height < vertex.y && vertex.y < terrainTypes[i + 1].height)
                            {
                                Instantiate(decorationList.gameObject[Random.Range(0, decorationList.gameObject.Length)].gameObject, new Vector3(vertex.x - offsetX, vertex.y, vertex.z - offsetZ), Quaternion.identity);
                            }
                        }
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
