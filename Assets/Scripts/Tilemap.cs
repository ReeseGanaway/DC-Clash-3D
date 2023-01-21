using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{

    public TileType[] tileTypes;
    int[,] tiles;

    int mapSizeX = 10;
    int mapSizeY = 10;

    void Start()
    {
        //Allocate our map tiles
        tiles = new int[mapSizeX, mapSizeY];

        //Initialize our grass tiles
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }

        //Change some tiles to swamp tiles
        for (int x = 3; x <= 5; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                tiles[x, y] = 2;
            }
        }

        //Change some tiles to mountain tiles 

        tiles[4, 4] = 1;
        tiles[5, 4] = 1;
        tiles[6, 4] = 1;
        tiles[7, 4] = 1;
        tiles[8, 4] = 1;
        tiles[4, 5] = 1;
        tiles[4, 6] = 1;
        tiles[8, 5] = 1;
        tiles[8, 6] = 1;

        //Spawn tiles
        GenerateMapVisuals();

    }

    void GenerateMapVisuals()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];
                Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }
}
