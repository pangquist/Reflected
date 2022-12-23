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

    public static UnityEvent Finished = new UnityEvent();

    public void Place(Map map)
    {
        foreach (Room room in map.Rooms)
        {
            PlaceDecorations(true, room.Rect, room.DecorationsParent, room.SpawnPointsParent, room.PathPoints);
        }

        foreach (Chamber chamber in map.Chambers)
        {
            PlaceDecorations(false, chamber.Rect, chamber.DecorationsParent, chamber.SpawnPointsParent, chamber.Room1.PathPoints.And(chamber.Room2.PathPoints));
        }

        Finished.Invoke();
        Physics.SyncTransforms();
    }

    private void PlaceDecorations(bool room, Rect rect, Transform decorationsParent, Transform spawnPointsParent, List<Vector3> pathPoints)
    {
        List<List<Vector3>> terrainObjectPoints = CreateRayCastPoints(rect, pathPoints);
        List<Vector3> enemySpawns = new List<Vector3>();
        List<WeightedRandomList<UnityEngine.GameObject>.Pair> spawnObjects;

        Rect center = new Rect(rect.position + rect.size / 4, rect.size / 2);

        for (int k = 0; k < objects.Length; k++)
        {
            List<Vector3> pointList = terrainObjectPoints[k];

            if (pointList.Count > 0)
            {
                enemySpawns.AddRange(pointList);
                spawnObjects = objects[k].terrainObjects.list;

                foreach (WeightedRandomList<UnityEngine.GameObject>.Pair pair in spawnObjects)
                {
                    for (int i = 0; i < pair.weight * objectMultiplier * rect.Area() * 0.001f; i++)
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
                            Instantiate(pair.item, point, Quaternion.identity, decorationsParent);
                    }
                }
            }
        }

        if (room)
            PlaceEnemySpawnPoints(enemySpawns, spawnPointsParent);
    }

    private List<List<Vector3>> CreateRayCastPoints(Rect rect, List<Vector3> pathPoints)
    {
        Vector3 start = new Vector3(rect.position.x + wallPadding, 0, rect.position.y + wallPadding);
        Vector3 end = new Vector3(rect.position.x + rect.width - wallPadding, 0, rect.position.y + rect.height - wallPadding);
        TerrainType[] terrainTypes = terrainGenerator.TerrainTypes();

        List<float> terrainHeights = new List<float>();
        List<List<Vector3>> terrainObjectPoints = new List<List<Vector3>>();

        foreach (ObjectList objectList in objects)
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

                foreach (Vector3 pathPoint in pathPoints)
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

    private void PlaceEnemySpawnPoints(List<Vector3> raycastOrigins, Transform spawnPointsParent)
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
                    Instantiate(enemySpawnPoint, hit.point, Quaternion.identity, spawnPointsParent);
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
            Debug.LogWarning(spawnPointsParent + " from " + spawnPointsParent.parent + " only got " + spawnPoints + "/" + enemySpawnPoints + " enemy spawn points after " + attempts + "/" + maxSpawnPointAttempts + " attempts.");
    }

}
