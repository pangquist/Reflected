using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StructurePlacer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private MapGenerator mapGenerator;

    [Header("Predetermined structures")]
    [SerializeField] private WeightedRandomList<GameObject> shopStructurePrefabs;
    [SerializeField] private WeightedRandomList<GameObject> bossStructurePrefabs;

    [Header("Values")]
    [SerializeField] private ObjectList[] objectLists;
    [SerializeField] private float raysPerChunk;
    [SerializeField] private float wallPadding;
    [Range(0f, 1f)]
    [SerializeField] private float maxCoverage;
    [SerializeField] private float chamberRadius;

    public static UnityEvent Finished = new UnityEvent();

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

        // Updates all colliders without having to wait for an automatic physics update
        Physics.SyncTransforms();
        Finished.Invoke();
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
        List<GameObject> placedPrefabs = new List<GameObject>();

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

            // Ensure structure has not been placed in this room already
            if (placedPrefabs.Contains(structure.gameObject))
                continue;

            // Ensure structure can be placed
            if (!FitsInsideRoom() || BlocksChamber() || OverlapsOtherStructures() || OverlapsPath())
                continue;

            // Instantiate structure
            placedPrefabs.Add(structure.gameObject);
            InstantiateStructure(structure.gameObject, room, raycastHit);
        }

        bool FitsInsideRoom()
        {
            return room.Rect.Inflated(-structure.ObstructiveRadius, -structure.ObstructiveRadius).Contains(raycastHit.point.XZ());
        }

        bool BlocksChamber()
        {
            foreach (Chamber chamber in room.Chambers)
                if (Vector2.Distance(raycastHit.point.XZ(), chamber.Rect.center) < structure.ObstructiveRadius + chamberRadius)
                    return true;
            return false;
        }

        bool OverlapsPath()
        {
            if (!structure.AvoidPaths)
                return false;

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
            InstantiateStructure(shopStructurePrefabs.GetRandom(), room, room.Rect.center, true);
        }
        else if (room.Type == RoomType.Boss)
        {
            InstantiateStructure(bossStructurePrefabs.GetRandom(), room, room.Rect.center, true);
        }
    }

    private void InstantiateStructure(GameObject structurePrefab, Room room, RaycastHit raycastHit)
    {
        Structure structure = Instantiate(structurePrefab.gameObject, room.StructuresParent).GetComponent<Structure>();
        structure.transform.position = raycastHit.point;
        structure.TerrainFlattener.Trigger();
        room.Structures.Add(structure);
    }

    private void InstantiateStructure(GameObject structurePrefab, Room room, Vector2 position, bool aboveWater)
    {
        RaycastHit raycastHit;
        Physics.Raycast(new Vector3(position.x, 100f, position.y), Vector3.down, out raycastHit);

        if (aboveWater && raycastHit.point.y < mapGenerator.WaterGenerator.WaterY + 0.2f)
            raycastHit.point = new Vector3(raycastHit.point.x, mapGenerator.WaterGenerator.WaterY + 0.2f, raycastHit.point.z);

        InstantiateStructure(structurePrefab, room, raycastHit);
    }

}
