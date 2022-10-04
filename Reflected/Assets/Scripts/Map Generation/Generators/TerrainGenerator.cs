using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public Color color;
}
public class TerrainGenerator : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject terrainChunkPrefab;

    [SerializeField] NoiseMapGenerator noiseMapGenerator;
    [SerializeField] ObjectPlacer objectPlacer;

    [Header("Terrain")]

    [SerializeField] private float startY;

    [SerializeField] private float mapScale;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private TerrainType[] terrainTypes;
    [SerializeField] private AnimationCurve heightCurve;
    [SerializeField] private Wave[] waves;

    public TerrainType[] TerrainTypes() { return terrainTypes; }
    public float HeightMultiplier() { return heightMultiplier; }
    public AnimationCurve HeightCurve() { return heightCurve; }

    public void Generate(Map map)
    {

        foreach (Room room in map.Rooms)
        {
            RectInt roomRect = room.Rect.Inflated(1, 1);
            Mesh mesh = PlaneMeshGenerator.GenerateHorizontal(roomRect.width * MapGenerator.ChunkSize, roomRect.height * MapGenerator.ChunkSize, 1, true);
            

            Vector3 position = new Vector3(roomRect.center.x * MapGenerator.ChunkSize, startY, roomRect.center.y * MapGenerator.ChunkSize);
            // Instantiate a new TerrainChunk
            TerrainChunk terrainChunk = Instantiate(terrainChunkPrefab, position, Quaternion.Euler(0, 180, 0), room.transform).GetComponent<TerrainChunk>();
            terrainChunk.Initialize(mesh, roomRect);

            GenerateTerrainChunk(terrainChunk.GetComponent<TerrainChunk>());
            //BakeNavMesh(terrainChunk.GetComponentInChildren<NavMeshSurface>());
            objectPlacer.PlaceDecorations(terrainChunk.GetComponent<TerrainChunk>());
        }

        foreach (Chamber chamber in map.Chambers)
        {
            RectInt rect;

            if (chamber.Orientation == Orientation.Horizontal)
                rect = chamber.Rect.Inflated(0, 1);
            else
                rect = chamber.Rect.Inflated(1, 0);

            Mesh mesh = PlaneMeshGenerator.GenerateHorizontal(rect.width * MapGenerator.ChunkSize, rect.height * MapGenerator.ChunkSize, 1, true);

            Vector3 position = new Vector3(rect.center.x * MapGenerator.ChunkSize, startY, rect.center.y * MapGenerator.ChunkSize);
            // Instantiate a new TerrainChunk
            TerrainChunk terrainChunk = Instantiate(terrainChunkPrefab, position, Quaternion.Euler(0, 180, 0), chamber.transform).GetComponent<TerrainChunk>();
            terrainChunk.Initialize(mesh, rect);

            GenerateTerrainChunk(terrainChunk.GetComponent<TerrainChunk>());
        }
    }

    private void GenerateTerrainChunk(TerrainChunk terrainChunk)
    {
        float[,] heightMap = GenerateHeightMap(terrainChunk);

        Texture2D chunkTexture = BuildTexture(heightMap);
        terrainChunk.MeshRenderer().material.mainTexture = chunkTexture;
        UpdateMeshVertices(heightMap, terrainChunk);
    }

    public float[,] GenerateHeightMap(TerrainChunk terrainChunk)
    {
        // calculate chunk depth and width based on the mesh vertices
        Vector3[] meshVertices = terrainChunk.MeshFilter().mesh.vertices;
        int chunkDepth = terrainChunk.Rect().width * MapGenerator.ChunkSize + 1;
        int chunkWidth = terrainChunk.Rect().height * MapGenerator.ChunkSize + 1;
        // calculate the offsets based on the tile position
        float offsetX = -terrainChunk.transform.position.x;
        float offsetZ = -terrainChunk.transform.position.z;
        // generate a heightMap using noise
        return this.noiseMapGenerator.GenerateNoiseMap(chunkDepth, chunkWidth, this.mapScale, offsetX, offsetZ, waves);
    }

    private void UpdateMeshVertices(float[,] heightMap, TerrainChunk terrainChunk)
    {
        int chunkDepth = heightMap.GetLength(0);
        int chunkWidth = heightMap.GetLength(1);
        Vector3[] meshVertices = terrainChunk.MeshFilter().mesh.vertices;
        // iterate through all the heightMap coordinates, updating the vertex index
        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < chunkDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < chunkWidth; xIndex++)
            {
                float height = heightMap[zIndex, xIndex];
                Vector3 vertex = meshVertices[vertexIndex];
                // change the vertex Y coordinate, proportional to the height value
                meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);
                vertexIndex++;
            }
        }
        // update the vertices in the mesh and update its properties
        terrainChunk.MeshFilter().mesh.vertices = meshVertices;
        terrainChunk.MeshFilter().mesh.RecalculateBounds();
        terrainChunk.MeshFilter().mesh.RecalculateNormals();
        // update the mesh collider
        terrainChunk.MeshCollider().sharedMesh = terrainChunk.MeshFilter().mesh;
    }

    private void BakeNavMesh(NavMeshSurface surface)
    {
        surface.BuildNavMesh();
    }

    private Texture2D BuildTexture(float[,] heightMap)
    {
        int chunkDepth = heightMap.GetLength(0);
        int chunkWidth = heightMap.GetLength(1);
        Color[] colorMap = new Color[chunkDepth * chunkWidth];
        for (int zIndex = 0; zIndex < chunkDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < chunkWidth; xIndex++)
            {
                // transform the 2D map index is an Array index
                int colorIndex = zIndex * chunkWidth + xIndex;
                float height = heightMap[zIndex, xIndex];

                TerrainType terrain = PickTerrainType(height);

                colorMap[colorIndex] = new Color(height, height, height);
            }
        }
        // create a new texture and set its pixel colors
        Texture2D chunkTexture = new Texture2D(chunkWidth, chunkDepth);
        chunkTexture.wrapMode = TextureWrapMode.Clamp;
        chunkTexture.SetPixels(colorMap);
        chunkTexture.Apply();
        System.IO.File.WriteAllBytes("C:/Users/Pangquist/heightMap.png", chunkTexture.EncodeToPNG());
        return chunkTexture;
    }

    TerrainType PickTerrainType(float height)
    {
        // for each terrain type, check if the height is lower than the one for the terrain type
        foreach (TerrainType terrainType in terrainTypes)
        {
            // return the first terrain type whose height is higher than the generated one
            if (height < terrainType.height)
            {
                return terrainType;
            }
        }
        return terrainTypes[terrainTypes.Length - 1];
    }
}
