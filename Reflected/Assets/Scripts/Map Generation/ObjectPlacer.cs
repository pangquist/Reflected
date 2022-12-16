using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
    [SerializeField] GameObject enemySpawnPoint;

    [Header("Values")]
    [SerializeField] float objectMultiplier;
    [Range(2f, 8f)]
    [SerializeField] float wallPadding;
    [Range(3f, 15f)]
    [SerializeField] float obstacleDistance;
    [SerializeField] bool avoidCenter;
    [SerializeField] LayerMask layerMask;
    [SerializeField] int enemySpawnPoints;
    [SerializeField] int maxSpawnPointAttempts;

    [Header("Objects")]
    [SerializeField] ObjectList[] objects;
    [SerializeField] ObjectList[] bossRangeObjects;

    public static UnityEvent Finished = new UnityEvent();

    public void Place(Map map)
    {
        foreach (Room room in map.Rooms)
        {
            PlaceDecorations(room);
        }

        Finished.Invoke();
        Physics.SyncTransforms();
    }

    private void PlaceDecorations(Room room)
    {
        List<List<Vector3>> terrainObjectPoints = CreateRayCastPoints(room);
        List<Vector3> enemySpawns = new List<Vector3>();
        List<WeightedRandomList<UnityEngine.GameObject>.Pair> spawnObjects;

        Rect center = new Rect(room.Rect.position + room.Rect.size / 4, room.Rect.size / 2);

        for(int k = 0; k < objects.Length; k++)
        {
            List<Vector3> pointList = terrainObjectPoints[k];

            if(pointList.Count > 0)
            {
                enemySpawns.AddRange(pointList);

                if(room.BossDistance < 200 && bossRangeObjects[k].terrainObjects.Count > 0)
                {
                    spawnObjects = bossRangeObjects[k].terrainObjects.list;
                }
                else
                {
                    spawnObjects = objects[k].terrainObjects.list;
                }

                foreach (WeightedRandomList<UnityEngine.GameObject>.Pair pair in spawnObjects)
                {
                    for (int i = 0; i < pair.weight * objectMultiplier * room.Rect.Area() * 0.001f; i++)
                    {
                        Vector3 point = pointList[Random.Range(0, pointList.Count)];
                        bool canPlace = true;

                        if (pair.item.gameObject.GetComponent<NavMeshObstacle>())
                        {
                            Collider[] closeObjects = Physics.OverlapSphere(point, obstacleDistance);

                            foreach (Collider collider in closeObjects)
                            {
                                if (collider.gameObject.GetComponent<NavMeshObstacle>() || collider.transform.parent.parent.GetComponent<Structure>())
                                    canPlace = false;
                            }
                            if (avoidCenter)
                            {
                                if (center.Contains(new Vector2(point.x, point.z)))
                                    canPlace = false;
                            }
                        }
                        if (canPlace)
                            Instantiate(pair.item, point, Quaternion.identity, room.DecorationsParent);
                    }
                }
            }
        }
        PlaceEnemySpawnPoints(enemySpawns, room);
    }

    private List<List<Vector3>> CreateRayCastPoints(Room room)
    {
        Vector3 start = new Vector3(room.Rect.position.x + wallPadding, 0, room.Rect.position.y + wallPadding);
        Vector3 end = new Vector3(room.Rect.position.x + room.Rect.width - wallPadding, 0, room.Rect.position.y + room.Rect.height - wallPadding);
        TerrainType[] terrainTypes = terrainGenerator.TerrainTypes();

        List<float> terrainHeights = new List<float>();
        List<List<Vector3>> terrainObjectPoints = new List<List<Vector3>>();
        foreach(ObjectList objectList in objects)
        {
            terrainObjectPoints.Add(new List<Vector3>());
        }
        foreach (TerrainType terrain in terrainTypes)
        {
            terrainHeights.Add(terrainGenerator.HeightCurve().Evaluate(terrain.height) * terrainGenerator.HeightMultiplier());
        }

        for (float i = start.x; i < end.x; i++)
        {
            for (float j = start.z; j < end.z; j++)
            {
                bool canPlace = true;
                Vector3 coordinate = new Vector3(i, 100f, j);

                foreach (Vector3 pathPoint in room.PathPoints)
                {
                    if (Vector2.Distance(pathPoint.XZ(), coordinate.XZ()) < PathGenerator.Radius)
                    {
                        canPlace = false;
                        break;
                    } 
                }

                if (canPlace)
                {
                    Ray ray = new Ray(coordinate, -transform.up);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 150f, layerMask) && hit.collider.gameObject.GetComponentInParent<TerrainChunk>())
                    {
                        float otherHeight = 0;
                        for (int k = 0; k < terrainHeights.Count; k++)
                        {
                            if (otherHeight < hit.point.y && hit.point.y <= terrainHeights[k])
                            {
                                terrainObjectPoints[k].Add(hit.point);
                                break;
                            }
                            otherHeight = terrainHeights[k];
                        }
                    }
                }
            }
        }
        return terrainObjectPoints;
    }

    private void PlaceEnemySpawnPoints(List<Vector3> raycastOrigins, Room room)
    {
        int spawnPoints = 0;
        int attempts = 0;

        while (spawnPoints < enemySpawnPoints && attempts < maxSpawnPointAttempts)
        {
            ++attempts;
            Ray ray = new Ray(raycastOrigins.GetRandom() + new Vector3(0, 1, 0), Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponentInParent<TerrainChunk>())
            {
                Collider[] closeObjects = Physics.OverlapSphere(hit.point, obstacleDistance);

                if (CanPlace())
                {
                    Instantiate(enemySpawnPoint, hit.point, Quaternion.identity, room.SpawnPointsParent);
                    ++spawnPoints;
                }

                bool CanPlace()
                {
                    foreach (Collider collider in closeObjects)
                        if (collider.gameObject.GetComponent<NavMeshObstacle>())
                            return false;
                    return true;   
                }
            }
        }

        if (spawnPoints < enemySpawnPoints)
            Debug.LogWarning(room + " only got " + spawnPoints + "/" + enemySpawnPoints + " enemy spawn points after " + attempts + "/" + maxSpawnPointAttempts + " attempts.");
    }

}
