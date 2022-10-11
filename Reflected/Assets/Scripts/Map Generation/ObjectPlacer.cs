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

    [Header("Values")]
    [Range(0f, 1f)]
    [SerializeField] float chancePerVertex;

    [Header("Decorations")]
    [SerializeField] ObjectList[] objects;

    public void Place(Map map)
    {
        foreach(Room room in map.Rooms)
        {
            TerrainChunk[] chunks = room.GetComponentsInChildren<TerrainChunk>();
            for (int i = 0; i < chunks.Length; i++)
            {
                PlaceDecorations(chunks[i], room);
            }
        }
    }

    private void PlaceDecorations(TerrainChunk terrainChunk, Room room)
    {
        Vector3[] meshVertices = terrainChunk.MeshFilter().mesh.vertices;
        Vector3[] visitedVertices = new Vector3[meshVertices.Length];

        float offsetX = -terrainChunk.transform.position.x + MapGenerator.ChunkSize * 0.5f;
        float offsetZ = -terrainChunk.transform.position.z + MapGenerator.ChunkSize * 0.5f;
        TerrainType[] terrainTypes = terrainGenerator.TerrainTypes();

        if (room.Rect.Contains(new Vector2Int((int)(terrainChunk.transform.position.x - MapGenerator.ChunkSize) / MapGenerator.ChunkSize, (int)(terrainChunk.transform.position.z - MapGenerator.ChunkSize) / MapGenerator.ChunkSize)))
        {
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
                                    if (Random.Range(0f, 1f) < chancePerVertex)
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

    private void NewPlaceDecorations(Room room)
    {
        Vector3 start = new Vector3(room.Rect.position.x, 50, room.Rect.position.y);
        Vector3 end = new Vector3(room.Rect.position.x + room.Rect.width, 50, room.Rect.position.y + room.Rect.height);
        TerrainType[] terrainTypes = terrainGenerator.TerrainTypes();

        for (float x = start.x; x < end.x; x++)
        {
            for (float z = start.z; z < end.z; z++)
            {
                if (Random.Range(0f, 1f) < chancePerVertex)
                {
                    Ray ray = new Ray(new Vector3(x, start.y, z), -transform.up);
                    RaycastHit hit;
                    Physics.Raycast(ray, out hit);

                    foreach (TerrainType terrain in terrainTypes)
                    {
                        if (hit.point.y <= terrainGenerator.HeightCurve().Evaluate(terrain.height) * terrainGenerator.HeightMultiplier())
                        {

                        }
                    }
                }
            }
        }
    }
}
