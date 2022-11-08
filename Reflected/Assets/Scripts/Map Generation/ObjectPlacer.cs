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

    public void Place(Map map, float pathRadius)
    {
        foreach(Room room in map.Rooms)
        {
            PlaceDecorations(room, pathRadius);
        }
    }

    private void PlaceDecorations(Room room, float pathRadius)
    {
        List<List<Vector3>> terrainObjectPoints = CreateRayCastPoints(room, pathRadius);
        Rect center = new Rect(room.Rect.position + room.Rect.size / 4, room.Rect.size / 2);

        int terrainNr = 0;

        foreach (ObjectList objectList in objects)
        {
            List<Vector3> pointList = terrainObjectPoints[terrainNr];

            if(pointList.Count > 0)
            {
                foreach (WeightedRandomList<UnityEngine.GameObject>.Pair pair in objectList.terrainObjects.list)
                {
                    for (int i = 0; i < pair.weight * objectMultiplier * 10; i++)
                    {
                        Vector3 point = pointList[Random.Range(0, pointList.Count)];

                        bool canPlace = true;

                        if (pair.item.gameObject.GetComponent<NavMeshObstacle>())
                        {
                            Collider[] closeObjects = Physics.OverlapSphere(point, obstacleDistance);

                            foreach (Collider collider in closeObjects)
                            {
                                if (collider.gameObject.GetComponent<NavMeshObstacle>())
                                {
                                    canPlace = false;
                                }
                            }
                            if (avoidCenter)
                            {
                                if (center.Contains(new Vector2(point.x, point.z)))
                                {
                                    //Debug.Log("Blocked placement center");
                                    canPlace = false;
                                }
                            }
                        }
                        if (canPlace)
                            Instantiate(pair.item, point, Quaternion.identity, room.transform);
                    }
                }
            }
            terrainNr++;
        }
        for (int i = 1; i < terrainObjectPoints.Count; i++)
        {
            terrainObjectPoints[0].AddRange(terrainObjectPoints[i]);
        }
        PlaceEnemySpawnPoints(terrainObjectPoints[0], room);
    }

    private List<List<Vector3>> CreateRayCastPoints(Room room, float pathRadius)
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
            Debug.Log(terrainHeights[terrainHeights.Count - 1]);
        }

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
                {
                    Ray ray = new Ray(coordinate, -transform.up);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponentInParent<TerrainChunk>())
                    {
                        float otherHeight = 0;
                        for (int k = 0; k < terrainHeights.Count; k++)
                        {
                            if (otherHeight < hit.point.y && hit.point.y <= terrainHeights[k])
                            {
                                terrainObjectPoints[k].Add(hit.point);
                                Debug.Log("Point found: " + terrainHeights[k]);
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
}
