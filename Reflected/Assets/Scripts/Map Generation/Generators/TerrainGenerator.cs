using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.AI;
using System;
using Unity.Mathematics;
using UnityEngine.Events;
using Unity.VisualScripting;

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

    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject terrainChunkPrefab;

    [SerializeField] NoiseMapGenerator noiseMapGenerator;

    [Header("Terrain")]

    [SerializeField] private float startY;
    [SerializeField] private float mapScale;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private TerrainType[] terrainTypes;
    [SerializeField] private AnimationCurve heightCurve;
    [SerializeField] private Wave[] waves;

    [Header("Path adaption")]

    [SerializeField] private float pathY;
    [SerializeField] private float pathAdaptionRange;
    [SerializeField] private AnimationCurve pathAdaptionAmount;
    [SerializeField] private AnimationCurve pathHeightAdaption;

    private float randomSeed = 0;

    public TerrainType[] TerrainTypes() => terrainTypes;
    public float HeightMultiplier() => heightMultiplier;
    public AnimationCurve HeightCurve() => heightCurve;

    public static UnityEvent Finished = new UnityEvent();

    public void SetRandomSeed(float seed)
    {
        randomSeed = seed / 10000;
    }

    public void Generate(Map map)
    {
        Mesh mesh = PlaneMeshGenerator.GenerateHorizontal(MapGenerator.ChunkSize, MapGenerator.ChunkSize, 1, true);
        terrainChunkPrefab.GetComponentInChildren<MeshFilter>().mesh = mesh;
        terrainChunkPrefab.GetComponentInChildren<MeshCollider>().sharedMesh = mesh;
        terrainChunkPrefab.transform.GetChild(0).position = new Vector3(MapGenerator.ChunkSize * 0.5f, 0, MapGenerator.ChunkSize * 0.5f);

        for (int xChunkIndex = 0; xChunkIndex < map.SizeX; xChunkIndex++)
        {
            for (int zChunkIndex = 0; zChunkIndex < map.SizeZ; zChunkIndex++)
            {
                // Calculate the tile position based on the X and Z indices
                Vector3 tilePosition = new Vector3(
                    xChunkIndex * MapGenerator.ChunkSize + MapGenerator.ChunkSize,
                    startY,
                    zChunkIndex * MapGenerator.ChunkSize + MapGenerator.ChunkSize);

                Rect terrainRect = new Rect(
                    tilePosition.x - MapGenerator.ChunkSize,
                    tilePosition.z - MapGenerator.ChunkSize,
                    MapGenerator.ChunkSize,
                    MapGenerator.ChunkSize);

                foreach (Room room in map.Rooms)
                {
                    if (room.Rect.Inflated(1, 1).Overlaps(terrainRect))
                    {
                        // Instantiate a new TerrainChunk
                        GameObject terrainChunk = Instantiate(terrainChunkPrefab, tilePosition, Quaternion.Euler(0, 180, 0), room.TerrainChild);
                        GenerateTerrainChunk(terrainChunk.GetComponent<TerrainChunk>(), room);
                        break;
                    }
                }

                foreach (Chamber chamber in map.Chambers)
                {
                    Rect chamberRect;

                    if (chamber.Orientation == Orientation.Horizontal)
                        chamberRect = chamber.Rect.Inflated(0, 1);
                    else
                        chamberRect = chamber.Rect.Inflated(1, 0);

                    if (chamberRect.Overlaps(terrainRect))
                    {
                        // Instantiate a new TerrainChunk
                        GameObject terrainChunk = Instantiate(terrainChunkPrefab, tilePosition, Quaternion.Euler(0, 180, 0), chamber.TerrainChild);
                        GenerateTerrainChunk(terrainChunk.GetComponent<TerrainChunk>(), chamber.Room1, chamber.Room2);
                        break;
                    }
                }
            }
        }

        Finished.Invoke();
    }

    private void GenerateTerrainChunk(TerrainChunk terrainChunk, Room room1, Room room2 = null)
    {
        float[,] heightMap = GenerateHeightMap(terrainChunk);

        Texture2D chunkTexture = BuildTexture(heightMap, terrainChunk);
        terrainChunk.MeshRenderer().material.mainTexture = chunkTexture;
        UpdateMeshVertices(heightMap, terrainChunk, room1, room2);
    }

    public float[,] GenerateHeightMap(TerrainChunk terrainChunk)
    {
        // calculate chunk depth and width based on the mesh vertices
        Vector3[] meshVertices = terrainChunk.MeshFilter().mesh.vertices;
        int chunkDepth = (int)Mathf.Sqrt(meshVertices.Length);
        int chunkWidth = chunkDepth;

        // calculate the offsets based on the tile position
        float offsetX = -terrainChunk.transform.position.x;
        float offsetZ = -terrainChunk.transform.position.z;

        // generate a height map using noise
        return noiseMapGenerator.GenerateNoiseMap(chunkDepth, chunkWidth, mapScale, offsetX, offsetZ, waves, randomSeed);
    }

    private void UpdateMeshVertices(float[,] heightMap, TerrainChunk terrainChunk, Room room1, Room room2)
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
                meshVertices[vertexIndex] = new Vector3(vertex.x, heightCurve.Evaluate(height) * heightMultiplier, vertex.z);
                vertexIndex++;
            }
        }

        // modify vertices based of paths
        ModifyVerticesUsingPaths(terrainChunk, ref meshVertices, room1, room2);

        // update the vertices in the mesh and update its properties
        terrainChunk.UpdateMesh(ref meshVertices);
    }

    private void ModifyVerticesUsingPaths(TerrainChunk terrainChunk, ref Vector3[] meshVertices, Room room1, Room room2)
    {
        Vector3 offset = new Vector3(MapGenerator.ChunkSize * 0.5f, 0, MapGenerator.ChunkSize * 0.5f);
        Matrix4x4 matrix = terrainChunk.transform.localToWorldMatrix;

        Vector3 vertexWorldPos;
        float distance, adaption, adaptionAmount;

        for (int i = 0; i < meshVertices.Length; ++i)
        {
            vertexWorldPos = matrix.MultiplyPoint3x4(meshVertices[i]) - offset;

            ModifyVertexHeight(ref meshVertices, room1);

            if (room2 != null)
                ModifyVertexHeight(ref meshVertices, room2);

            void ModifyVertexHeight(ref Vector3[] meshVertices, Room room)
            {
                foreach (Vector3 pathPoint in room.PathPoints)
                {
                    distance = Vector2.Distance(vertexWorldPos.XZ(), pathPoint.XZ());

                    if (distance < pathAdaptionRange)
                    {
                        // how much to affect the vertex based of the distance to the path
                        adaptionAmount = pathAdaptionAmount.Evaluate(distance / pathAdaptionRange);

                        // how much to affect the vertex based of its current height
                        adaptionAmount *= pathHeightAdaption.Evaluate((meshVertices[i].y - startY) / heightMultiplier);

                        // the final height adaption
                        adaption = (pathY - meshVertices[i].y) * adaptionAmount;

                        // apply adaption
                        meshVertices[i].y += adaption;
                    }
                }
            }
        }
    }

    private Texture2D BuildTexture(float[,] heightMap, TerrainChunk terrainChunk)
    {
        int chunkDepth = heightMap.GetLength(0);
        int chunkWidth = heightMap.GetLength(1);
        Color[] colorMap = new Color[chunkDepth * chunkWidth];

        float worldPixelSize = (float)MapGenerator.ChunkSize / chunkDepth;

        // Get List of relevant paths

        List<PathCreator> paths;

        Room room;
        if (room = terrainChunk.transform.parent.parent.GetComponent<Room>())
                paths = room.Paths;

        else
        {
            Chamber chamber = terrainChunk.transform.parent.parent.GetComponent<Chamber>();
            paths = chamber.Room1.Paths.And(chamber.Room2.Paths);
        }

        // Determine color of each pixel

        for (int zIndex = 0; zIndex < chunkDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < chunkWidth; xIndex++)
            {
                // Transform the 2D map index is an Array index
                int colorIndex = zIndex * chunkWidth + xIndex;

                // Returns whether or not this pixel should be a path
                bool CheckPaths()
                {
                    // Calculate world position of pixel
                    Vector3 pixelPosition = new Vector3(
                        terrainChunk.transform.position.x - (xIndex + 0.5f) * worldPixelSize,
                        0f,
                        terrainChunk.transform.position.z - (zIndex + 0.5f) * worldPixelSize);

                    // Check paths
                    foreach (PathCreator path in paths)
                        if (Vector2.Distance(pixelPosition.XZ(), path.path.GetClosestPointOnPath(pixelPosition).XZ()) < PathGenerator.Radius)
                            return true;
                    return false;
                }

                // Check for paths
                if (CheckPaths())
                    colorMap[colorIndex] = PathGenerator.Color;

                // Or check height
                else
                {
                    float height = heightMap[zIndex, xIndex];
                    TerrainType terrain = PickTerrainType(height);
                    colorMap[colorIndex] = terrain.color;
                }
            }
        }

        // Create a new texture and set its pixel colors
        Texture2D chunkTexture = new Texture2D(chunkWidth, chunkDepth);
        chunkTexture.wrapMode = TextureWrapMode.Clamp;
        chunkTexture.SetPixels(colorMap);
        chunkTexture.Apply();
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
