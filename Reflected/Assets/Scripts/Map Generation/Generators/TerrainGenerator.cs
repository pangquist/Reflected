using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject terrainChunkPrefab;

    [Header("Terrain")]

    [SerializeField] private float startY;

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
                Vector3 tilePosition = new Vector3(xChunkIndex * MapGenerator.ChunkSize + MapGenerator.ChunkSize, startY, zChunkIndex * MapGenerator.ChunkSize + MapGenerator.ChunkSize);

                foreach (Room room in map.Rooms)
                {
                    if (room.Rect.Inflated(1, 1).Contains(new Vector2Int(xChunkIndex, zChunkIndex)))
                    {
                        // Instantiate a new TerrainChunk
                        Instantiate(terrainChunkPrefab, tilePosition, Quaternion.Euler(0, 180, 0), room.transform);
                        break;
                    }
                }

                foreach (Chamber chamber in map.Chambers)
                {
                    RectInt rect;

                    if (chamber.Orientation == Orientation.Horizontal)
                        rect = chamber.Rect.Inflated(0, 1);
                    else
                        rect = chamber.Rect.Inflated(1, 0);

                    if (rect.Contains(new Vector2Int(xChunkIndex, zChunkIndex)))
                    {
                        // Instantiate a new TerrainChunk
                        Instantiate(terrainChunkPrefab, tilePosition, Quaternion.Euler(0, 180, 0), chamber.transform);
                        break;
                    }
                }
            }
        }
    }
}
