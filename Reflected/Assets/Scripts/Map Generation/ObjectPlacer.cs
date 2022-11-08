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
    [SerializeField] MapGenerator mapGenerator;

    [Header("Values")]
    [Range(1, 20)]
    [SerializeField] int objectMultiplier;
    [Range(2f, 8f)]
    [SerializeField] float wallPadding;
    [Range(3f, 15f)]
    [SerializeField] float obstacleDistance;
    [SerializeField] bool avoidCenter;

    [SerializeField] GameObject enemySpawnPoint;

    [Header("Objects")]
    [SerializeField] ObjectList[] objects;

    public void Place(Map map)
    {
        foreach (Room room in map.Rooms)
        {
            PlaceDecorations(room);
        }
    }

    private void PlaceDecorations(Room room)
    {
        List<Vector3> raycastOrigins = CreateRayCastPoints(room, mapGenerator.PathGenerator.Radius);
        Rect center = new Rect(room.Rect.position + room.Rect.size / 4, room.Rect.size / 2);
        TerrainType[] terrainTypes = terrainGenerator.TerrainTypes();

        foreach (ObjectList objectList in objects)
        {
            int terrainNr = 0;
            foreach (TerrainType terrain in terrainTypes)
            {
                if (objectList.terrain == terrain.name && objectList.terrainObjects.Count > 0)
                {
                    float height = terrainGenerator.HeightCurve().Evaluate(terrainTypes[terrainNr].height) * terrainGenerator.HeightMultiplier();

                    float otherHeight = 0;
                    if (terrainNr > 0)
                        otherHeight = terrainGenerator.HeightCurve().Evaluate(terrainTypes[terrainNr - 1].height) * terrainGenerator.HeightMultiplier();

                    foreach (WeightedRandomList<UnityEngine.GameObject>.Pair pair in objectList.terrainObjects.list)
                    {
                        for (int i = 0; i < pair.weight * objectMultiplier * 10; i++)
                        {
                            Ray ray = new Ray(raycastOrigins[(int)Random.Range(0, raycastOrigins.Count)], -transform.up);

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
                                                //Debug.Log("Blocked placement center");
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
        PlaceEnemySpawnPoints(raycastOrigins, room);
    }

    private List<Vector3> CreateRayCastPoints(Room room, float pathRadius)
    {
        Vector3 start = new Vector3(room.Rect.position.x + wallPadding, 0, room.Rect.position.y + wallPadding);
        Vector3 end = new Vector3(room.Rect.position.x + room.Rect.width - wallPadding, 0, room.Rect.position.y + room.Rect.height - wallPadding);

        List<Vector3> raycastOrigins = new List<Vector3>();

        for (float i = start.x; i < end.x; i++)
        {
            for (float j = start.z; j < end.z; j++)
            {
                bool canPlace = true;
                Vector3 coordinate = new Vector3(i, room.PathPoints[0].y, j);
                foreach (Vector3 pathPoint in room.PathPoints)
                {
                    if (Vector3.Distance(pathPoint, coordinate) < pathRadius)
                    {
                        canPlace = false;
                    }
                }
                if (canPlace)
                    raycastOrigins.Add(coordinate);
            }
        }
        return raycastOrigins;
    }

    private void PlaceEnemySpawnPoints(List<Vector3> raycastOrigins, Room room)
    {
        for (int i = 0; i < objectMultiplier * 10; i++)
        {
            Ray ray = new Ray(raycastOrigins[(int)Random.Range(0, raycastOrigins.Count)], -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponentInParent<TerrainChunk>())
            {
                Collider[] closeObjects = Physics.OverlapSphere(hit.point, obstacleDistance);

                foreach (Collider collider in closeObjects)
                {
                    if (!collider.gameObject.GetComponent<NavMeshObstacle>())
                    {
                        Instantiate(enemySpawnPoint, hit.point, Quaternion.identity, room.transform);
                    }
                }
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

        if (room.Rect.Contains(new Vector2(terrainChunk.transform.position.x - MapGenerator.ChunkSize, terrainChunk.transform.position.z - MapGenerator.ChunkSize)))
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
