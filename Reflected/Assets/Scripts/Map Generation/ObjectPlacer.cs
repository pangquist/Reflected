using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [Range(1, 5)]
    [SerializeField] int objectMultiplier;
    [Range(2f, 8f)]
    [SerializeField] float wallPadding;
    [Range(3f, 15f)]
    [SerializeField] float obstacleDistance;
    [SerializeField] bool avoidCenter;

    [Header("Decorations")]
    [SerializeField] ObjectList[] objects;

    public void Place(Map map)
    {
        foreach(Room room in map.Rooms)
        {
            PlaceDecorations(room);
        }
    }

    private void PlaceDecorations(Room room)
    {
        Vector3 start = new Vector3(room.Rect.position.x, 0, room.Rect.position.y);
        Vector3 end = new Vector3(room.Rect.position.x + room.Rect.width, 0, room.Rect.position.y + room.Rect.height);
        Rect center = new Rect(room.Rect.position + room.Rect.size / 4, room.Rect.size / 2);
        TerrainType[] terrainTypes = terrainGenerator.TerrainTypes();

        foreach (ObjectList objectList in objects)
        {
            int terrainNr = 0;
            foreach (TerrainType terrain in terrainTypes)
            {
                if (objectList.terrain == terrain.name)
                {
                    float height = terrainGenerator.HeightCurve().Evaluate(terrainTypes[terrainNr].height) * terrainGenerator.HeightMultiplier();

                    float otherHeight = 0;
                    if (terrainNr > 0)
                        otherHeight = terrainGenerator.HeightCurve().Evaluate(terrainTypes[terrainNr - 1].height) * terrainGenerator.HeightMultiplier();

                    foreach (WeightedRandomList<UnityEngine.GameObject>.Pair pair in objectList.terrainObjects.list)
                    {
                        for (int i = 0; i < pair.weight * objectMultiplier * 10; i++)
                        {
                            Debug.Log(pair.item + " " + i);
                            Ray ray = new Ray(new Vector3(Random.Range(start.x + wallPadding, end.x - wallPadding), 20, Random.Range(start.z + wallPadding, end.z - wallPadding)), -transform.up);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponentInParent<TerrainChunk>())
                            {
                                if (otherHeight < hit.point.y && hit.point.y <= height)
                                {
                                    bool canPlace = true;

                                    if (pair.item.gameObject.GetComponent<NavMeshObstacle>())
                                    {
                                        Collider[] closeObjects = Physics.OverlapSphere(hit.point, obstacleDistance);

                                        foreach (Collider collider in closeObjects)
                                        {
                                            if (collider.gameObject.GetComponent<NavMeshObstacle>())
                                            {
                                                canPlace = false;
                                            }
                                        }

                                        if (avoidCenter)
                                        {
                                            if (center.Contains(new Vector2(hit.point.x, hit.point.z)))
                                            {
                                                Debug.Log("Blocked placement center");
                                                canPlace = false;
                                            }
                                        }
                                    }
                                    

                                    if (canPlace)
                                        Instantiate(pair.item, hit.point, Quaternion.identity, room.transform);
                                }
                            }
                        }
                    }
                }
                terrainNr++;
            }
        }
    }

    private void OldPlaceDecorations(TerrainChunk terrainChunk, Room room)
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
                                    //if (Random.Range(0f, 1f) < chancePerVertex)
                                    //{
                                    //    Matrix4x4 localToWorld = transform.localToWorldMatrix;
                                    //    Vector3 position = terrainChunk.transform.rotation * localToWorld.MultiplyPoint3x4(meshVertices[i]);
                                    //    position = new Vector3(position.x - offsetX, position.y, position.z - offsetZ);
                                    //    Instantiate(objectList.terrainObjects.GetRandom(), position, Quaternion.identity, terrainChunk.transform);
                                    //}
                                }
                            }
                            visitedVertices[i] = meshVertices[i];
                        }

                    }
                }
            }
        }
       
    }
}
