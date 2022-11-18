using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlacer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private MapGenerator mapGenerator;

    [Header("Predetermined structures")]
    [SerializeField] private GameObject shopPrefab;

    [Header("Values")]
    [SerializeField] private ObjectList[] objectLists;
    [SerializeField] private float raysPerChunk;
    [SerializeField] private float wallPadding;
    [Range(0f, 1f)]
    [SerializeField] private float maxCoverage;

    public void Place(Map map)
    {
        foreach (Room room in map.Rooms)
        {
            PredeterminedStructures(room);

            foreach (ObjectList objectList in objectLists)
            {
                for (int i = 0; i < terrainGenerator.TerrainTypes().Length; ++i)
                {
                    if (terrainGenerator.TerrainTypes()[i].name.Contains(objectList.terrain) == false || objectList.terrainObjects.Count == 0)
                        continue;

                    PlaceStructures(room, i, objectList.terrainObjects);
                }
            }
        }
    }

    private void PlaceStructures(Room room, int terrainTypeIndex, WeightedRandomList<GameObject> structures)
    {
        // Find height range

        float minHeight = 0f;
        float maxHeight = terrainGenerator.HeightCurve().Evaluate(terrainGenerator.TerrainTypes()[terrainTypeIndex].height) * terrainGenerator.HeightMultiplier();

        if (terrainTypeIndex > 0)
            minHeight = terrainGenerator.HeightCurve().Evaluate(terrainGenerator.TerrainTypes()[terrainTypeIndex - 1].height) * terrainGenerator.HeightMultiplier();

        // Calculate number of rays

        float roomChunkArea = room.Rect.Area() / Mathf.Pow(MapGenerator.ChunkSize, 2f);
        int nrOfRays = (int)(roomChunkArea * raysPerChunk);

        // Perform raycasts

        RaycastHit raycastHit;
        Rect rayRect = room.Rect.Inflated(-wallPadding, -wallPadding);
        Vector2 rayPosition;
        Structure structure;

        for (int i = 0; i < nrOfRays; ++i)
        {
            // Ensure max coverage it not exceeded
            if (Coverage(room) > maxCoverage)
                break;

            rayPosition = rayRect.RandomPoint();
            Physics.Raycast(new Vector3(rayPosition.x, 100f, rayPosition.y), Vector3.down, out raycastHit);

            // Ensure point is within the terrainType's height range
            if (raycastHit.point.y < minHeight || raycastHit.point.y > maxHeight)
                continue;

            // Get random structure
            structure = structures.GetRandom().GetComponent<Structure>();

            // Ensure structure fits inside the room
            if (!FitsInsideRoom())
                continue;

            // Ensure structure doesn't overlap paths
            if (structure.AvoidPaths && OverlapsPath())
                continue;

            // Ensure structure doesn't overlap other structures
            if (OverlapsOtherStructures())
                continue;

            // Instantiate structure
            InstantiateStructure(structure.gameObject, room, raycastHit);
        }

        bool FitsInsideRoom()
        {
            return room.Rect.Inflated(-structure.ObstructiveRadius, -structure.ObstructiveRadius).Contains(raycastHit.point.XZ());
        }

        bool OverlapsPath()
        {
            foreach (Vector3 pathPoint in room.PathPoints)
                if (Vector2.Distance(raycastHit.point.XZ(), pathPoint.XZ()) < structure.ObstructiveRadius + PathGenerator.Radius)
                    return true;
            return false;
        }

        bool OverlapsOtherStructures()
        {
            foreach (Structure otherStructure in room.Structures)
                if (Vector2.Distance(raycastHit.point.XZ(), otherStructure.transform.position.XZ()) < structure.ObstructiveRadius + otherStructure.ObstructiveRadius)
                    return true;
            return false;
        }

    }

    private float Coverage(Room room)
    {
        float totalCoveredArea = 0f;

        foreach (Structure structure1 in room.Structures)
            totalCoveredArea += structure1.ObstructiveArea();

        return totalCoveredArea / room.Rect.Area();
    }

    private void PredeterminedStructures(Room room)
    {
        if (room.Type == RoomType.Shop)
        {
            InstantiateStructure(shopPrefab, room, room.Rect.center);
        }
    }

    private void InstantiateStructure(GameObject structurePrefab, Room room, RaycastHit raycastHit)
    {
        Structure structure = Instantiate(structurePrefab.gameObject, room.ObjectsChild).GetComponent<Structure>();
        Debug.Log("RaycastHit.point: " + raycastHit.point);
        structure.transform.position = raycastHit.point;
        Debug.Log("Position: " + structure.transform.position);
        structure.TerrainFlattener.Trigger();
        room.Structures.Add(structure);
    }

    private void InstantiateStructure(GameObject structurePrefab, Room room, Vector2 position)
    {
        RaycastHit raycastHit;
        Physics.Raycast(new Vector3(position.x, 100f, position.y), Vector3.down, out raycastHit);
        InstantiateStructure(structurePrefab, room, raycastHit);
    }

}
