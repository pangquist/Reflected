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

        foreach(Room room in map.Rooms)
        {
            SetPathPoints(room);
        }

        Finished.Invoke();
    }

    private void GenerateTerrainChunk(TerrainChunk terrainChunk, Room room1, Room room2 = null)
    {
        float[,] heightMap = GenerateHeightMap(terrainChunk);
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

    private void SetPathPoints(Room room)
    {
        foreach (PathCreator path in room.Paths)
        {
            float pathLength = path.path.length;
            for (int i = 0; i < pathLength; i+=4)
            {
                Vector3 point = path.path.GetPointAtDistance(i);
                Ray ray = new Ray(point, -transform.up);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponentInParent<TerrainChunk>())
                {
                    Collider[] closeObjects = Physics.OverlapSphere(hit.point, PathGenerator.Radius * 1.5f);

                    foreach (Collider collider in closeObjects)
                    {
                        if (collider.gameObject.GetComponentInParent<TerrainChunk>())
                        {
                            TerrainChunk chunk = collider.gameObject.GetComponentInParent<TerrainChunk>();
                            chunk.PathPoints.Add(point);
                        }
                    }
                }
            }
        }

        TerrainChunk[] chunks = room.gameObject.GetComponentsInChildren<TerrainChunk>();

        foreach (TerrainChunk chunk in chunks)
        {
            chunk.MeshRenderer().material.SetFloat("_PathRadius", PathGenerator.Radius);
            chunk.MeshRenderer().material.SetFloat("_PathPointHeight", PathGenerator.Level);
            chunk.PassPointsToMaterial();
        }

        foreach (Chamber chamber in room.Chambers)
        {
            chunks = chamber.gameObject.GetComponentsInChildren<TerrainChunk>();
            foreach (TerrainChunk chunk in chunks)
            {
                chunk.MeshRenderer().material.SetFloat("_PathRadius", PathGenerator.Radius);
                chunk.MeshRenderer().material.SetFloat("_PathPointHeight", PathGenerator.Level);
                chunk.PassPointsToMaterial();
            }
        }
    }
}
