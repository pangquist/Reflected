using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    [SerializeField]
    private int mapWidthInTiles, mapDepthInTiles;
    [SerializeField]
    private GameObject tilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRoom();
    }

    void GenerateRoom()
    {
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        int tileWidth = (int)tileSize.x;
        int tileDepth = (int)tileSize.z;

        for (int xTileIndex = 0; xTileIndex < mapWidthInTiles; xTileIndex++)
        {
            for (int zTileIndex = 0; zTileIndex < mapDepthInTiles; zTileIndex++)
            {
                // calculate the tile position based on the X and Z indices
                Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xTileIndex * tileWidth,
                  this.gameObject.transform.position.y,
                  this.gameObject.transform.position.z + zTileIndex * tileDepth);
                // instantiate a new Tile
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
            }
        }
    }
}
